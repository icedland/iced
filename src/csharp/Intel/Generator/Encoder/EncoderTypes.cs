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
	sealed class EncoderTypes {
		public static (EnumValue value, uint size)[] ImmSizes { get; }
		public static EnumType EncFlags1 { get; }
		public static EnumType LegacyFlags3 { get; }
		public static EnumType VexFlags3 { get; }
		public static EnumType XopFlags3 { get; }
		public static EnumType EvexFlags3 { get; }
		public static EnumType AllowedPrefixes { get; }
		public static Dictionary<OpCodeFlags, EnumValue> AllowedPrefixesMap { get; }
		public static EnumType LegacyFlags { get; }
		public static EnumType VexFlags { get; }
		public static EnumType XopFlags { get; }
		public static EnumType EvexFlags { get; }
		public static EnumType D3nowFlags { get; }
		public static (EnumValue opCodeOperandKind, EnumValue legacyOpKind, OpHandlerKind opHandlerKind, object[] args)[] LegacyOpHandlers { get; } = new (EnumValue opCodeOperandKind, EnumValue legacyOpKind, OpHandlerKind opHandlerKind, object[] args)[] {
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.None)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.None)], OpHandlerKind.None, new object[] { }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.farbr2_2)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.Aww)], OpHandlerKind.OpA, new object[] { 2 }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.farbr4_2)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.Adw)], OpHandlerKind.OpA, new object[] { 4 }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.mem)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.M)], OpHandlerKind.OpModRM_rm_mem_only, new object[] { }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.mem)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.Mfbcd)], OpHandlerKind.OpModRM_rm_mem_only, new object[] { }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.mem)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.Mf32)], OpHandlerKind.OpModRM_rm_mem_only, new object[] { }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.mem)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.Mf64)], OpHandlerKind.OpModRM_rm_mem_only, new object[] { }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.mem)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.Mf80)], OpHandlerKind.OpModRM_rm_mem_only, new object[] { }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.mem)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.Mfi16)], OpHandlerKind.OpModRM_rm_mem_only, new object[] { }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.mem)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.Mfi32)], OpHandlerKind.OpModRM_rm_mem_only, new object[] { }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.mem)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.Mfi64)], OpHandlerKind.OpModRM_rm_mem_only, new object[] { }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.mem)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.M14)], OpHandlerKind.OpModRM_rm_mem_only, new object[] { }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.mem)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.M28)], OpHandlerKind.OpModRM_rm_mem_only, new object[] { }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.mem)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.M98)], OpHandlerKind.OpModRM_rm_mem_only, new object[] { }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.mem)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.M108)], OpHandlerKind.OpModRM_rm_mem_only, new object[] { }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.mem)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.Mp)], OpHandlerKind.OpModRM_rm_mem_only, new object[] { }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.mem)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.Ms)], OpHandlerKind.OpModRM_rm_mem_only, new object[] { }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.mem)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.Mo)], OpHandlerKind.OpModRM_rm_mem_only, new object[] { }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.mem)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.Mb)], OpHandlerKind.OpModRM_rm_mem_only, new object[] { }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.mem)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.Mw)], OpHandlerKind.OpModRM_rm_mem_only, new object[] { }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.mem)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.Md)], OpHandlerKind.OpModRM_rm_mem_only, new object[] { }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.mem_mpx)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.Md_MPX)], OpHandlerKind.OpModRM_rm_mem_only, new object[] { }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.mem)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.Mq)], OpHandlerKind.OpModRM_rm_mem_only, new object[] { }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.mem_mpx)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.Mq_MPX)], OpHandlerKind.OpModRM_rm_mem_only, new object[] { }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.mem)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.Mw2)], OpHandlerKind.OpModRM_rm_mem_only, new object[] { }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.mem)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.Md2)], OpHandlerKind.OpModRM_rm_mem_only, new object[] { }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.r8_or_mem)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.Eb)], OpHandlerKind.OpModRM_rm, new object[] { RegisterEnum.Instance[nameof(Register.AL)], RegisterEnum.Instance[nameof(Register.R15L)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.r16_or_mem)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.Ew)], OpHandlerKind.OpModRM_rm, new object[] { RegisterEnum.Instance[nameof(Register.AX)], RegisterEnum.Instance[nameof(Register.R15W)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.r32_or_mem)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.Ed)], OpHandlerKind.OpModRM_rm, new object[] { RegisterEnum.Instance[nameof(Register.EAX)], RegisterEnum.Instance[nameof(Register.R15D)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.r32_or_mem_mpx)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.Ed_MPX)], OpHandlerKind.OpModRM_rm, new object[] { RegisterEnum.Instance[nameof(Register.EAX)], RegisterEnum.Instance[nameof(Register.R15D)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.r32_or_mem)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.Ew_d)], OpHandlerKind.OpModRM_rm, new object[] { RegisterEnum.Instance[nameof(Register.EAX)], RegisterEnum.Instance[nameof(Register.R15D)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.r64_or_mem)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.Ew_q)], OpHandlerKind.OpModRM_rm, new object[] { RegisterEnum.Instance[nameof(Register.RAX)], RegisterEnum.Instance[nameof(Register.R15)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.r64_or_mem)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.Eq)], OpHandlerKind.OpModRM_rm, new object[] { RegisterEnum.Instance[nameof(Register.RAX)], RegisterEnum.Instance[nameof(Register.R15)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.r64_or_mem_mpx)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.Eq_MPX)], OpHandlerKind.OpModRM_rm, new object[] { RegisterEnum.Instance[nameof(Register.RAX)], RegisterEnum.Instance[nameof(Register.R15)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.mem)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.Eww)], OpHandlerKind.OpModRM_rm_mem_only, new object[] { }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.mem)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.Edw)], OpHandlerKind.OpModRM_rm_mem_only, new object[] { }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.mem)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.Eqw)], OpHandlerKind.OpModRM_rm_mem_only, new object[] { }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.r32_or_mem)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.RdMb)], OpHandlerKind.OpModRM_rm, new object[] { RegisterEnum.Instance[nameof(Register.EAX)], RegisterEnum.Instance[nameof(Register.R15D)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.r64_or_mem)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.RqMb)], OpHandlerKind.OpModRM_rm, new object[] { RegisterEnum.Instance[nameof(Register.RAX)], RegisterEnum.Instance[nameof(Register.R15)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.r32_or_mem)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.RdMw)], OpHandlerKind.OpModRM_rm, new object[] { RegisterEnum.Instance[nameof(Register.EAX)], RegisterEnum.Instance[nameof(Register.R15D)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.r64_or_mem)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.RqMw)], OpHandlerKind.OpModRM_rm, new object[] { RegisterEnum.Instance[nameof(Register.RAX)], RegisterEnum.Instance[nameof(Register.R15)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.r8_reg)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.Gb)], OpHandlerKind.OpModRM_reg, new object[] { RegisterEnum.Instance[nameof(Register.AL)], RegisterEnum.Instance[nameof(Register.R15L)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.r16_reg)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.Gw)], OpHandlerKind.OpModRM_reg, new object[] { RegisterEnum.Instance[nameof(Register.AX)], RegisterEnum.Instance[nameof(Register.R15W)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.r32_reg)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.Gd)], OpHandlerKind.OpModRM_reg, new object[] { RegisterEnum.Instance[nameof(Register.EAX)], RegisterEnum.Instance[nameof(Register.R15D)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.r64_reg)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.Gq)], OpHandlerKind.OpModRM_reg, new object[] { RegisterEnum.Instance[nameof(Register.RAX)], RegisterEnum.Instance[nameof(Register.R15)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.r16_reg_mem)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.Gw_mem)], OpHandlerKind.OpModRM_reg_mem, new object[] { RegisterEnum.Instance[nameof(Register.AX)], RegisterEnum.Instance[nameof(Register.R15W)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.r32_reg_mem)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.Gd_mem)], OpHandlerKind.OpModRM_reg_mem, new object[] { RegisterEnum.Instance[nameof(Register.EAX)], RegisterEnum.Instance[nameof(Register.R15D)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.r64_reg_mem)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.Gq_mem)], OpHandlerKind.OpModRM_reg_mem, new object[] { RegisterEnum.Instance[nameof(Register.RAX)], RegisterEnum.Instance[nameof(Register.R15)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.r16_rm)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.Rw)], OpHandlerKind.OpModRM_rm_reg_only, new object[] { RegisterEnum.Instance[nameof(Register.AX)], RegisterEnum.Instance[nameof(Register.R15W)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.r32_rm)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.Rd)], OpHandlerKind.OpModRM_rm_reg_only, new object[] { RegisterEnum.Instance[nameof(Register.EAX)], RegisterEnum.Instance[nameof(Register.R15D)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.r64_rm)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.Rq)], OpHandlerKind.OpModRM_rm_reg_only, new object[] { RegisterEnum.Instance[nameof(Register.RAX)], RegisterEnum.Instance[nameof(Register.R15)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.seg_reg)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.Sw)], OpHandlerKind.OpModRM_reg, new object[] { RegisterEnum.Instance[nameof(Register.ES)], RegisterEnum.Instance[nameof(Register.GS)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.cr_reg)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.Cd)], OpHandlerKind.OpModRM_regF0, new object[] { RegisterEnum.Instance[nameof(Register.CR0)], RegisterEnum.Instance[nameof(Register.CR15)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.cr_reg)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.Cq)], OpHandlerKind.OpModRM_reg, new object[] { RegisterEnum.Instance[nameof(Register.CR0)], RegisterEnum.Instance[nameof(Register.CR15)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.dr_reg)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.Dd)], OpHandlerKind.OpModRM_reg, new object[] { RegisterEnum.Instance[nameof(Register.DR0)], RegisterEnum.Instance[nameof(Register.DR15)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.dr_reg)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.Dq)], OpHandlerKind.OpModRM_reg, new object[] { RegisterEnum.Instance[nameof(Register.DR0)], RegisterEnum.Instance[nameof(Register.DR15)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.tr_reg)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.Td)], OpHandlerKind.OpModRM_reg, new object[] { RegisterEnum.Instance[nameof(Register.TR0)], RegisterEnum.Instance[nameof(Register.TR7)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.imm8)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.Ib)], OpHandlerKind.OpIb, new object[] { OpKindEnum.Instance[nameof(OpKind.Immediate8)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.imm8sex16)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.Ib16)], OpHandlerKind.OpIb, new object[] { OpKindEnum.Instance[nameof(OpKind.Immediate8to16)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.imm8sex32)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.Ib32)], OpHandlerKind.OpIb, new object[] { OpKindEnum.Instance[nameof(OpKind.Immediate8to32)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.imm8sex64)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.Ib64)], OpHandlerKind.OpIb, new object[] { OpKindEnum.Instance[nameof(OpKind.Immediate8to64)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.imm16)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.Iw)], OpHandlerKind.OpIw, new object[] { }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.imm32)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.Id)], OpHandlerKind.OpId, new object[] { OpKindEnum.Instance[nameof(OpKind.Immediate32)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.imm32sex64)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.Id64)], OpHandlerKind.OpId, new object[] { OpKindEnum.Instance[nameof(OpKind.Immediate32to64)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.imm64)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.Iq)], OpHandlerKind.OpIq, new object[] { }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.imm8)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.Ib21)], OpHandlerKind.OpIb21, new object[] { }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.imm8)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.Ib11)], OpHandlerKind.OpIb11, new object[] { }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.seg_rSI)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.Xb)], OpHandlerKind.OpX, new object[] { }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.seg_rSI)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.Xw)], OpHandlerKind.OpX, new object[] { }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.seg_rSI)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.Xd)], OpHandlerKind.OpX, new object[] { }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.seg_rSI)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.Xq)], OpHandlerKind.OpX, new object[] { }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.es_rDI)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.Yb)], OpHandlerKind.OpY, new object[] { }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.es_rDI)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.Yw)], OpHandlerKind.OpY, new object[] { }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.es_rDI)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.Yd)], OpHandlerKind.OpY, new object[] { }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.es_rDI)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.Yq)], OpHandlerKind.OpY, new object[] { }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.br16_1)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.wJb)], OpHandlerKind.OpJ, new object[] { OpKindEnum.Instance[nameof(OpKind.NearBranch16)], 1 }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.br32_1)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.dJb)], OpHandlerKind.OpJ, new object[] { OpKindEnum.Instance[nameof(OpKind.NearBranch32)], 1 }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.br64_1)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.qJb)], OpHandlerKind.OpJ, new object[] { OpKindEnum.Instance[nameof(OpKind.NearBranch64)], 1 }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.br16_2)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.Jw)], OpHandlerKind.OpJ, new object[] { OpKindEnum.Instance[nameof(OpKind.NearBranch16)], 2 }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.br32_4)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.wJd)], OpHandlerKind.OpJ, new object[] { OpKindEnum.Instance[nameof(OpKind.NearBranch32)], 4 }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.br32_4)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.dJd)], OpHandlerKind.OpJ, new object[] { OpKindEnum.Instance[nameof(OpKind.NearBranch32)], 4 }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.br64_4)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.qJd)], OpHandlerKind.OpJ, new object[] { OpKindEnum.Instance[nameof(OpKind.NearBranch64)], 4 }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.xbegin_2)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.Jxw)], OpHandlerKind.OpJx, new object[] { 2 }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.xbegin_4)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.Jxd)], OpHandlerKind.OpJx, new object[] { 4 }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.brdisp_2)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.Jdisp16)], OpHandlerKind.OpJdisp, new object[] { 2 }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.brdisp_4)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.Jdisp32)], OpHandlerKind.OpJdisp, new object[] { 4 }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.mem_offs)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.Ob)], OpHandlerKind.OpO, new object[] { }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.mem_offs)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.Ow)], OpHandlerKind.OpO, new object[] { }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.mem_offs)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.Od)], OpHandlerKind.OpO, new object[] { }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.mem_offs)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.Oq)], OpHandlerKind.OpO, new object[] { }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.imm8_const_1)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.Imm1)], OpHandlerKind.OpImm, new object[] { 1 }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.bnd_reg)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.B)], OpHandlerKind.OpModRM_reg, new object[] { RegisterEnum.Instance[nameof(Register.BND0)], RegisterEnum.Instance[nameof(Register.BND3)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.bnd_or_mem_mpx)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.BMq)], OpHandlerKind.OpModRM_rm, new object[] { RegisterEnum.Instance[nameof(Register.BND0)], RegisterEnum.Instance[nameof(Register.BND3)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.bnd_or_mem_mpx)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.BMo)], OpHandlerKind.OpModRM_rm, new object[] { RegisterEnum.Instance[nameof(Register.BND0)], RegisterEnum.Instance[nameof(Register.BND3)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.mem_mib)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.MIB)], OpHandlerKind.OpModRM_rm_mem_only, new object[] { }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.mm_rm)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.N)], OpHandlerKind.OpModRM_rm_reg_only, new object[] { RegisterEnum.Instance[nameof(Register.MM0)], RegisterEnum.Instance[nameof(Register.MM7)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.mm_reg)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.P)], OpHandlerKind.OpModRM_reg, new object[] { RegisterEnum.Instance[nameof(Register.MM0)], RegisterEnum.Instance[nameof(Register.MM7)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.mm_or_mem)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.Q)], OpHandlerKind.OpModRM_rm, new object[] { RegisterEnum.Instance[nameof(Register.MM0)], RegisterEnum.Instance[nameof(Register.MM7)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.xmm_rm)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.RX)], OpHandlerKind.OpModRM_rm_reg_only, new object[] { RegisterEnum.Instance[nameof(Register.XMM0)], RegisterEnum.Instance[nameof(Register.XMM15)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.xmm_reg)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.VX)], OpHandlerKind.OpModRM_reg, new object[] { RegisterEnum.Instance[nameof(Register.XMM0)], RegisterEnum.Instance[nameof(Register.XMM15)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.xmm_or_mem)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.WX)], OpHandlerKind.OpModRM_rm, new object[] { RegisterEnum.Instance[nameof(Register.XMM0)], RegisterEnum.Instance[nameof(Register.XMM15)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.seg_rDI)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.rDI)], OpHandlerKind.OprDI, new object[] { }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.seg_rBX_al)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.MRBX)], OpHandlerKind.OpMRBX, new object[] { }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.es)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.ES)], OpHandlerKind.OpReg, new object[] { RegisterEnum.Instance[nameof(Register.ES)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.cs)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.CS)], OpHandlerKind.OpReg, new object[] { RegisterEnum.Instance[nameof(Register.CS)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.ss)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.SS)], OpHandlerKind.OpReg, new object[] { RegisterEnum.Instance[nameof(Register.SS)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.ds)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.DS)], OpHandlerKind.OpReg, new object[] { RegisterEnum.Instance[nameof(Register.DS)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.fs)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.FS)], OpHandlerKind.OpReg, new object[] { RegisterEnum.Instance[nameof(Register.FS)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.gs)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.GS)], OpHandlerKind.OpReg, new object[] { RegisterEnum.Instance[nameof(Register.GS)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.al)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.AL)], OpHandlerKind.OpReg, new object[] { RegisterEnum.Instance[nameof(Register.AL)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.cl)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.CL)], OpHandlerKind.OpReg, new object[] { RegisterEnum.Instance[nameof(Register.CL)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.ax)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.AX)], OpHandlerKind.OpReg, new object[] { RegisterEnum.Instance[nameof(Register.AX)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.dx)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.DX)], OpHandlerKind.OpReg, new object[] { RegisterEnum.Instance[nameof(Register.DX)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.eax)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.EAX)], OpHandlerKind.OpReg, new object[] { RegisterEnum.Instance[nameof(Register.EAX)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.rax)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.RAX)], OpHandlerKind.OpReg, new object[] { RegisterEnum.Instance[nameof(Register.RAX)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.st0)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.ST)], OpHandlerKind.OpReg, new object[] { RegisterEnum.Instance[nameof(Register.ST0)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.sti_opcode)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.STi)], OpHandlerKind.OpRegSTi, new object[] { }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.r8_opcode)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.r8_rb)], OpHandlerKind.OpRegEmbed8, new object[] { RegisterEnum.Instance[nameof(Register.AL)], RegisterEnum.Instance[nameof(Register.R15L)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.r16_opcode)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.r16_rw)], OpHandlerKind.OpRegEmbed8, new object[] { RegisterEnum.Instance[nameof(Register.AX)], RegisterEnum.Instance[nameof(Register.R15W)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.r32_opcode)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.r32_rd)], OpHandlerKind.OpRegEmbed8, new object[] { RegisterEnum.Instance[nameof(Register.EAX)], RegisterEnum.Instance[nameof(Register.R15D)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.r64_opcode)], LegacyOpKindEnum.Instance[nameof(LegacyOpKind.r64_ro)], OpHandlerKind.OpRegEmbed8, new object[] { RegisterEnum.Instance[nameof(Register.RAX)], RegisterEnum.Instance[nameof(Register.R15)] }),
		};
		public static (EnumValue opCodeOperandKind, EnumValue vexOpKind, OpHandlerKind opHandlerKind, object[] args)[] VexOpHandlers { get; } = new (EnumValue opCodeOperandKind, EnumValue vexOpKind, OpHandlerKind opHandlerKind, object[] args)[] {
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.None)], VexOpKindEnum.Instance[nameof(VexOpKind.None)], OpHandlerKind.None, new object[] { }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.r32_or_mem)], VexOpKindEnum.Instance[nameof(VexOpKind.Ed)], OpHandlerKind.OpModRM_rm, new object[] { RegisterEnum.Instance[nameof(Register.EAX)], RegisterEnum.Instance[nameof(Register.R15D)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.r64_or_mem)], VexOpKindEnum.Instance[nameof(VexOpKind.Eq)], OpHandlerKind.OpModRM_rm, new object[] { RegisterEnum.Instance[nameof(Register.RAX)], RegisterEnum.Instance[nameof(Register.R15)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.r32_reg)], VexOpKindEnum.Instance[nameof(VexOpKind.Gd)], OpHandlerKind.OpModRM_reg, new object[] { RegisterEnum.Instance[nameof(Register.EAX)], RegisterEnum.Instance[nameof(Register.R15D)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.r64_reg)], VexOpKindEnum.Instance[nameof(VexOpKind.Gq)], OpHandlerKind.OpModRM_reg, new object[] { RegisterEnum.Instance[nameof(Register.RAX)], RegisterEnum.Instance[nameof(Register.R15)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.r32_or_mem)], VexOpKindEnum.Instance[nameof(VexOpKind.RdMb)], OpHandlerKind.OpModRM_rm, new object[] { RegisterEnum.Instance[nameof(Register.EAX)], RegisterEnum.Instance[nameof(Register.R15D)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.r64_or_mem)], VexOpKindEnum.Instance[nameof(VexOpKind.RqMb)], OpHandlerKind.OpModRM_rm, new object[] { RegisterEnum.Instance[nameof(Register.RAX)], RegisterEnum.Instance[nameof(Register.R15)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.r32_or_mem)], VexOpKindEnum.Instance[nameof(VexOpKind.RdMw)], OpHandlerKind.OpModRM_rm, new object[] { RegisterEnum.Instance[nameof(Register.EAX)], RegisterEnum.Instance[nameof(Register.R15D)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.r64_or_mem)], VexOpKindEnum.Instance[nameof(VexOpKind.RqMw)], OpHandlerKind.OpModRM_rm, new object[] { RegisterEnum.Instance[nameof(Register.RAX)], RegisterEnum.Instance[nameof(Register.R15)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.r32_rm)], VexOpKindEnum.Instance[nameof(VexOpKind.Rd)], OpHandlerKind.OpModRM_rm_reg_only, new object[] { RegisterEnum.Instance[nameof(Register.EAX)], RegisterEnum.Instance[nameof(Register.R15D)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.r64_rm)], VexOpKindEnum.Instance[nameof(VexOpKind.Rq)], OpHandlerKind.OpModRM_rm_reg_only, new object[] { RegisterEnum.Instance[nameof(Register.RAX)], RegisterEnum.Instance[nameof(Register.R15)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.r32_vvvv)], VexOpKindEnum.Instance[nameof(VexOpKind.Hd)], OpHandlerKind.OpHx, new object[] { RegisterEnum.Instance[nameof(Register.EAX)], RegisterEnum.Instance[nameof(Register.R15D)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.r64_vvvv)], VexOpKindEnum.Instance[nameof(VexOpKind.Hq)], OpHandlerKind.OpHx, new object[] { RegisterEnum.Instance[nameof(Register.RAX)], RegisterEnum.Instance[nameof(Register.R15)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.k_vvvv)], VexOpKindEnum.Instance[nameof(VexOpKind.HK)], OpHandlerKind.OpHx, new object[] { RegisterEnum.Instance[nameof(Register.K0)], RegisterEnum.Instance[nameof(Register.K7)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.xmm_vvvv)], VexOpKindEnum.Instance[nameof(VexOpKind.HX)], OpHandlerKind.OpHx, new object[] { RegisterEnum.Instance[nameof(Register.XMM0)], RegisterEnum.Instance[nameof(Register.XMM15)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.ymm_vvvv)], VexOpKindEnum.Instance[nameof(VexOpKind.HY)], OpHandlerKind.OpHx, new object[] { RegisterEnum.Instance[nameof(Register.YMM0)], RegisterEnum.Instance[nameof(Register.YMM15)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.imm8)], VexOpKindEnum.Instance[nameof(VexOpKind.Ib)], OpHandlerKind.OpIb, new object[] { OpKindEnum.Instance[nameof(OpKind.Immediate8)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.imm2_m2z)], VexOpKindEnum.Instance[nameof(VexOpKind.I2)], OpHandlerKind.OpI2, new object[] { }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.xmm_is4)], VexOpKindEnum.Instance[nameof(VexOpKind.Is4X)], OpHandlerKind.OpIs4x, new object[] { RegisterEnum.Instance[nameof(Register.XMM0)], RegisterEnum.Instance[nameof(Register.XMM15)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.ymm_is4)], VexOpKindEnum.Instance[nameof(VexOpKind.Is4Y)], OpHandlerKind.OpIs4x, new object[] { RegisterEnum.Instance[nameof(Register.YMM0)], RegisterEnum.Instance[nameof(Register.YMM15)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.xmm_is5)], VexOpKindEnum.Instance[nameof(VexOpKind.Is5X)], OpHandlerKind.OpIs4x, new object[] { RegisterEnum.Instance[nameof(Register.XMM0)], RegisterEnum.Instance[nameof(Register.XMM15)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.ymm_is5)], VexOpKindEnum.Instance[nameof(VexOpKind.Is5Y)], OpHandlerKind.OpIs4x, new object[] { RegisterEnum.Instance[nameof(Register.YMM0)], RegisterEnum.Instance[nameof(Register.YMM15)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.mem)], VexOpKindEnum.Instance[nameof(VexOpKind.M)], OpHandlerKind.OpModRM_rm_mem_only, new object[] { }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.mem)], VexOpKindEnum.Instance[nameof(VexOpKind.Md)], OpHandlerKind.OpModRM_rm_mem_only, new object[] { }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.mem)], VexOpKindEnum.Instance[nameof(VexOpKind.MK)], OpHandlerKind.OpModRM_rm_mem_only, new object[] { }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.seg_rDI)], VexOpKindEnum.Instance[nameof(VexOpKind.rDI)], OpHandlerKind.OprDI, new object[] { }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.k_rm)], VexOpKindEnum.Instance[nameof(VexOpKind.RK)], OpHandlerKind.OpModRM_rm_reg_only, new object[] { RegisterEnum.Instance[nameof(Register.K0)], RegisterEnum.Instance[nameof(Register.K7)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.xmm_rm)], VexOpKindEnum.Instance[nameof(VexOpKind.RX)], OpHandlerKind.OpModRM_rm_reg_only, new object[] { RegisterEnum.Instance[nameof(Register.XMM0)], RegisterEnum.Instance[nameof(Register.XMM15)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.ymm_rm)], VexOpKindEnum.Instance[nameof(VexOpKind.RY)], OpHandlerKind.OpModRM_rm_reg_only, new object[] { RegisterEnum.Instance[nameof(Register.YMM0)], RegisterEnum.Instance[nameof(Register.YMM15)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.k_reg)], VexOpKindEnum.Instance[nameof(VexOpKind.VK)], OpHandlerKind.OpModRM_reg, new object[] { RegisterEnum.Instance[nameof(Register.K0)], RegisterEnum.Instance[nameof(Register.K7)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.mem_vsib32x)], VexOpKindEnum.Instance[nameof(VexOpKind.VM32X)], OpHandlerKind.OpVMx, new object[] { RegisterEnum.Instance[nameof(Register.XMM0)], RegisterEnum.Instance[nameof(Register.XMM15)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.mem_vsib32y)], VexOpKindEnum.Instance[nameof(VexOpKind.VM32Y)], OpHandlerKind.OpVMx, new object[] { RegisterEnum.Instance[nameof(Register.YMM0)], RegisterEnum.Instance[nameof(Register.YMM15)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.mem_vsib64x)], VexOpKindEnum.Instance[nameof(VexOpKind.VM64X)], OpHandlerKind.OpVMx, new object[] { RegisterEnum.Instance[nameof(Register.XMM0)], RegisterEnum.Instance[nameof(Register.XMM15)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.mem_vsib64y)], VexOpKindEnum.Instance[nameof(VexOpKind.VM64Y)], OpHandlerKind.OpVMx, new object[] { RegisterEnum.Instance[nameof(Register.YMM0)], RegisterEnum.Instance[nameof(Register.YMM15)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.xmm_reg)], VexOpKindEnum.Instance[nameof(VexOpKind.VX)], OpHandlerKind.OpModRM_reg, new object[] { RegisterEnum.Instance[nameof(Register.XMM0)], RegisterEnum.Instance[nameof(Register.XMM15)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.ymm_reg)], VexOpKindEnum.Instance[nameof(VexOpKind.VY)], OpHandlerKind.OpModRM_reg, new object[] { RegisterEnum.Instance[nameof(Register.YMM0)], RegisterEnum.Instance[nameof(Register.YMM15)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.k_or_mem)], VexOpKindEnum.Instance[nameof(VexOpKind.WK)], OpHandlerKind.OpModRM_rm, new object[] { RegisterEnum.Instance[nameof(Register.K0)], RegisterEnum.Instance[nameof(Register.K7)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.xmm_or_mem)], VexOpKindEnum.Instance[nameof(VexOpKind.WX)], OpHandlerKind.OpModRM_rm, new object[] { RegisterEnum.Instance[nameof(Register.XMM0)], RegisterEnum.Instance[nameof(Register.XMM15)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.ymm_or_mem)], VexOpKindEnum.Instance[nameof(VexOpKind.WY)], OpHandlerKind.OpModRM_rm, new object[] { RegisterEnum.Instance[nameof(Register.YMM0)], RegisterEnum.Instance[nameof(Register.YMM15)] }),
		};
		public static (EnumValue opCodeOperandKind, EnumValue xopOpKind, OpHandlerKind opHandlerKind, object[] args)[] XopOpHandlers { get; } = new (EnumValue opCodeOperandKind, EnumValue xopOpKind, OpHandlerKind opHandlerKind, object[] args)[] {
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.None)], XopOpKindEnum.Instance[nameof(XopOpKind.None)], OpHandlerKind.None, new object[] { }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.r32_or_mem)], XopOpKindEnum.Instance[nameof(XopOpKind.Ed)], OpHandlerKind.OpModRM_rm, new object[] { RegisterEnum.Instance[nameof(Register.EAX)], RegisterEnum.Instance[nameof(Register.R15D)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.r64_or_mem)], XopOpKindEnum.Instance[nameof(XopOpKind.Eq)], OpHandlerKind.OpModRM_rm, new object[] { RegisterEnum.Instance[nameof(Register.RAX)], RegisterEnum.Instance[nameof(Register.R15)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.r32_reg)], XopOpKindEnum.Instance[nameof(XopOpKind.Gd)], OpHandlerKind.OpModRM_reg, new object[] { RegisterEnum.Instance[nameof(Register.EAX)], RegisterEnum.Instance[nameof(Register.R15D)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.r64_reg)], XopOpKindEnum.Instance[nameof(XopOpKind.Gq)], OpHandlerKind.OpModRM_reg, new object[] { RegisterEnum.Instance[nameof(Register.RAX)], RegisterEnum.Instance[nameof(Register.R15)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.r32_rm)], XopOpKindEnum.Instance[nameof(XopOpKind.Rd)], OpHandlerKind.OpModRM_rm_reg_only, new object[] { RegisterEnum.Instance[nameof(Register.EAX)], RegisterEnum.Instance[nameof(Register.R15D)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.r64_rm)], XopOpKindEnum.Instance[nameof(XopOpKind.Rq)], OpHandlerKind.OpModRM_rm_reg_only, new object[] { RegisterEnum.Instance[nameof(Register.RAX)], RegisterEnum.Instance[nameof(Register.R15)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.r32_vvvv)], XopOpKindEnum.Instance[nameof(XopOpKind.Hd)], OpHandlerKind.OpHx, new object[] { RegisterEnum.Instance[nameof(Register.EAX)], RegisterEnum.Instance[nameof(Register.R15D)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.r64_vvvv)], XopOpKindEnum.Instance[nameof(XopOpKind.Hq)], OpHandlerKind.OpHx, new object[] { RegisterEnum.Instance[nameof(Register.RAX)], RegisterEnum.Instance[nameof(Register.R15)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.xmm_vvvv)], XopOpKindEnum.Instance[nameof(XopOpKind.HX)], OpHandlerKind.OpHx, new object[] { RegisterEnum.Instance[nameof(Register.XMM0)], RegisterEnum.Instance[nameof(Register.XMM15)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.ymm_vvvv)], XopOpKindEnum.Instance[nameof(XopOpKind.HY)], OpHandlerKind.OpHx, new object[] { RegisterEnum.Instance[nameof(Register.YMM0)], RegisterEnum.Instance[nameof(Register.YMM15)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.imm8)], XopOpKindEnum.Instance[nameof(XopOpKind.Ib)], OpHandlerKind.OpIb, new object[] { OpKindEnum.Instance[nameof(OpKind.Immediate8)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.imm32)], XopOpKindEnum.Instance[nameof(XopOpKind.Id)], OpHandlerKind.OpId, new object[] { OpKindEnum.Instance[nameof(OpKind.Immediate32)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.xmm_is4)], XopOpKindEnum.Instance[nameof(XopOpKind.Is4X)], OpHandlerKind.OpIs4x, new object[] { RegisterEnum.Instance[nameof(Register.XMM0)], RegisterEnum.Instance[nameof(Register.XMM15)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.ymm_is4)], XopOpKindEnum.Instance[nameof(XopOpKind.Is4Y)], OpHandlerKind.OpIs4x, new object[] { RegisterEnum.Instance[nameof(Register.YMM0)], RegisterEnum.Instance[nameof(Register.YMM15)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.xmm_reg)], XopOpKindEnum.Instance[nameof(XopOpKind.VX)], OpHandlerKind.OpModRM_reg, new object[] { RegisterEnum.Instance[nameof(Register.XMM0)], RegisterEnum.Instance[nameof(Register.XMM15)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.ymm_reg)], XopOpKindEnum.Instance[nameof(XopOpKind.VY)], OpHandlerKind.OpModRM_reg, new object[] { RegisterEnum.Instance[nameof(Register.YMM0)], RegisterEnum.Instance[nameof(Register.YMM15)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.xmm_or_mem)], XopOpKindEnum.Instance[nameof(XopOpKind.WX)], OpHandlerKind.OpModRM_rm, new object[] { RegisterEnum.Instance[nameof(Register.XMM0)], RegisterEnum.Instance[nameof(Register.XMM15)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.ymm_or_mem)], XopOpKindEnum.Instance[nameof(XopOpKind.WY)], OpHandlerKind.OpModRM_rm, new object[] { RegisterEnum.Instance[nameof(Register.YMM0)], RegisterEnum.Instance[nameof(Register.YMM15)] }),
		};
		public static (EnumValue opCodeOperandKind, EnumValue evexOpKind, OpHandlerKind opHandlerKind, object[] args)[] EvexOpHandlers { get; } = new (EnumValue opCodeOperandKind, EnumValue evexOpKind, OpHandlerKind opHandlerKind, object[] args)[] {
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.None)], EvexOpKindEnum.Instance[nameof(EvexOpKind.None)], OpHandlerKind.None, new object[] { }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.r32_or_mem)], EvexOpKindEnum.Instance[nameof(EvexOpKind.Ed)], OpHandlerKind.OpModRM_rm, new object[] { RegisterEnum.Instance[nameof(Register.EAX)], RegisterEnum.Instance[nameof(Register.R15D)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.r64_or_mem)], EvexOpKindEnum.Instance[nameof(EvexOpKind.Eq)], OpHandlerKind.OpModRM_rm, new object[] { RegisterEnum.Instance[nameof(Register.RAX)], RegisterEnum.Instance[nameof(Register.R15)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.r32_reg)], EvexOpKindEnum.Instance[nameof(EvexOpKind.Gd)], OpHandlerKind.OpModRM_reg, new object[] { RegisterEnum.Instance[nameof(Register.EAX)], RegisterEnum.Instance[nameof(Register.R15D)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.r64_reg)], EvexOpKindEnum.Instance[nameof(EvexOpKind.Gq)], OpHandlerKind.OpModRM_reg, new object[] { RegisterEnum.Instance[nameof(Register.RAX)], RegisterEnum.Instance[nameof(Register.R15)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.r32_or_mem)], EvexOpKindEnum.Instance[nameof(EvexOpKind.RdMb)], OpHandlerKind.OpModRM_rm, new object[] { RegisterEnum.Instance[nameof(Register.EAX)], RegisterEnum.Instance[nameof(Register.R15D)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.r64_or_mem)], EvexOpKindEnum.Instance[nameof(EvexOpKind.RqMb)], OpHandlerKind.OpModRM_rm, new object[] { RegisterEnum.Instance[nameof(Register.RAX)], RegisterEnum.Instance[nameof(Register.R15)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.r32_or_mem)], EvexOpKindEnum.Instance[nameof(EvexOpKind.RdMw)], OpHandlerKind.OpModRM_rm, new object[] { RegisterEnum.Instance[nameof(Register.EAX)], RegisterEnum.Instance[nameof(Register.R15D)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.r64_or_mem)], EvexOpKindEnum.Instance[nameof(EvexOpKind.RqMw)], OpHandlerKind.OpModRM_rm, new object[] { RegisterEnum.Instance[nameof(Register.RAX)], RegisterEnum.Instance[nameof(Register.R15)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.xmm_vvvv)], EvexOpKindEnum.Instance[nameof(EvexOpKind.HX)], OpHandlerKind.OpHx, new object[] { RegisterEnum.Instance[nameof(Register.XMM0)], RegisterEnum.Instance[nameof(Register.XMM31)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.ymm_vvvv)], EvexOpKindEnum.Instance[nameof(EvexOpKind.HY)], OpHandlerKind.OpHx, new object[] { RegisterEnum.Instance[nameof(Register.YMM0)], RegisterEnum.Instance[nameof(Register.YMM31)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.zmm_vvvv)], EvexOpKindEnum.Instance[nameof(EvexOpKind.HZ)], OpHandlerKind.OpHx, new object[] { RegisterEnum.Instance[nameof(Register.ZMM0)], RegisterEnum.Instance[nameof(Register.ZMM31)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.xmmp3_vvvv)], EvexOpKindEnum.Instance[nameof(EvexOpKind.HXP3)], OpHandlerKind.OpHx, new object[] { RegisterEnum.Instance[nameof(Register.XMM0)], RegisterEnum.Instance[nameof(Register.XMM31)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.zmmp3_vvvv)], EvexOpKindEnum.Instance[nameof(EvexOpKind.HZP3)], OpHandlerKind.OpHx, new object[] { RegisterEnum.Instance[nameof(Register.ZMM0)], RegisterEnum.Instance[nameof(Register.ZMM31)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.imm8)], EvexOpKindEnum.Instance[nameof(EvexOpKind.Ib)], OpHandlerKind.OpIb, new object[] { OpKindEnum.Instance[nameof(OpKind.Immediate8)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.mem)], EvexOpKindEnum.Instance[nameof(EvexOpKind.M)], OpHandlerKind.OpModRM_rm_mem_only, new object[] { }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.r32_rm)], EvexOpKindEnum.Instance[nameof(EvexOpKind.Rd)], OpHandlerKind.OpModRM_rm_reg_only, new object[] { RegisterEnum.Instance[nameof(Register.EAX)], RegisterEnum.Instance[nameof(Register.R15D)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.r64_rm)], EvexOpKindEnum.Instance[nameof(EvexOpKind.Rq)], OpHandlerKind.OpModRM_rm_reg_only, new object[] { RegisterEnum.Instance[nameof(Register.RAX)], RegisterEnum.Instance[nameof(Register.R15)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.xmm_rm)], EvexOpKindEnum.Instance[nameof(EvexOpKind.RX)], OpHandlerKind.OpModRM_rm_reg_only, new object[] { RegisterEnum.Instance[nameof(Register.XMM0)], RegisterEnum.Instance[nameof(Register.XMM31)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.ymm_rm)], EvexOpKindEnum.Instance[nameof(EvexOpKind.RY)], OpHandlerKind.OpModRM_rm_reg_only, new object[] { RegisterEnum.Instance[nameof(Register.YMM0)], RegisterEnum.Instance[nameof(Register.YMM31)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.zmm_rm)], EvexOpKindEnum.Instance[nameof(EvexOpKind.RZ)], OpHandlerKind.OpModRM_rm_reg_only, new object[] { RegisterEnum.Instance[nameof(Register.ZMM0)], RegisterEnum.Instance[nameof(Register.ZMM31)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.k_rm)], EvexOpKindEnum.Instance[nameof(EvexOpKind.RK)], OpHandlerKind.OpModRM_rm_reg_only, new object[] { RegisterEnum.Instance[nameof(Register.K0)], RegisterEnum.Instance[nameof(Register.K7)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.mem_vsib32x)], EvexOpKindEnum.Instance[nameof(EvexOpKind.VM32X)], OpHandlerKind.OpVMx, new object[] { RegisterEnum.Instance[nameof(Register.XMM0)], RegisterEnum.Instance[nameof(Register.XMM31)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.mem_vsib32y)], EvexOpKindEnum.Instance[nameof(EvexOpKind.VM32Y)], OpHandlerKind.OpVMx, new object[] { RegisterEnum.Instance[nameof(Register.YMM0)], RegisterEnum.Instance[nameof(Register.YMM31)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.mem_vsib32z)], EvexOpKindEnum.Instance[nameof(EvexOpKind.VM32Z)], OpHandlerKind.OpVMx, new object[] { RegisterEnum.Instance[nameof(Register.ZMM0)], RegisterEnum.Instance[nameof(Register.ZMM31)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.mem_vsib64x)], EvexOpKindEnum.Instance[nameof(EvexOpKind.VM64X)], OpHandlerKind.OpVMx, new object[] { RegisterEnum.Instance[nameof(Register.XMM0)], RegisterEnum.Instance[nameof(Register.XMM31)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.mem_vsib64y)], EvexOpKindEnum.Instance[nameof(EvexOpKind.VM64Y)], OpHandlerKind.OpVMx, new object[] { RegisterEnum.Instance[nameof(Register.YMM0)], RegisterEnum.Instance[nameof(Register.YMM31)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.mem_vsib64z)], EvexOpKindEnum.Instance[nameof(EvexOpKind.VM64Z)], OpHandlerKind.OpVMx, new object[] { RegisterEnum.Instance[nameof(Register.ZMM0)], RegisterEnum.Instance[nameof(Register.ZMM31)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.k_reg)], EvexOpKindEnum.Instance[nameof(EvexOpKind.VK)], OpHandlerKind.OpModRM_reg, new object[] { RegisterEnum.Instance[nameof(Register.K0)], RegisterEnum.Instance[nameof(Register.K7)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.kp1_reg)], EvexOpKindEnum.Instance[nameof(EvexOpKind.VKP1)], OpHandlerKind.OpModRM_reg, new object[] { RegisterEnum.Instance[nameof(Register.K0)], RegisterEnum.Instance[nameof(Register.K7)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.xmm_reg)], EvexOpKindEnum.Instance[nameof(EvexOpKind.VX)], OpHandlerKind.OpModRM_reg, new object[] { RegisterEnum.Instance[nameof(Register.XMM0)], RegisterEnum.Instance[nameof(Register.XMM31)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.ymm_reg)], EvexOpKindEnum.Instance[nameof(EvexOpKind.VY)], OpHandlerKind.OpModRM_reg, new object[] { RegisterEnum.Instance[nameof(Register.YMM0)], RegisterEnum.Instance[nameof(Register.YMM31)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.zmm_reg)], EvexOpKindEnum.Instance[nameof(EvexOpKind.VZ)], OpHandlerKind.OpModRM_reg, new object[] { RegisterEnum.Instance[nameof(Register.ZMM0)], RegisterEnum.Instance[nameof(Register.ZMM31)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.xmm_or_mem)], EvexOpKindEnum.Instance[nameof(EvexOpKind.WX)], OpHandlerKind.OpModRM_rm, new object[] { RegisterEnum.Instance[nameof(Register.XMM0)], RegisterEnum.Instance[nameof(Register.XMM31)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.ymm_or_mem)], EvexOpKindEnum.Instance[nameof(EvexOpKind.WY)], OpHandlerKind.OpModRM_rm, new object[] { RegisterEnum.Instance[nameof(Register.YMM0)], RegisterEnum.Instance[nameof(Register.YMM31)] }),
			(OpCodeOperandKindEnum.Instance[nameof(OpCodeOperandKind.zmm_or_mem)], EvexOpKindEnum.Instance[nameof(EvexOpKind.WZ)], OpHandlerKind.OpModRM_rm, new object[] { RegisterEnum.Instance[nameof(Register.ZMM0)], RegisterEnum.Instance[nameof(Register.ZMM31)] }),
		};

		static EncoderTypes() {
			var gen = new EncoderTypesGen();
			gen.Generate();
			ImmSizes = gen.ImmSizes ?? throw new InvalidOperationException();
			EncFlags1 = gen.EncFlags1 ?? throw new InvalidOperationException();
			LegacyFlags3 = gen.LegacyFlags3 ?? throw new InvalidOperationException();
			VexFlags3 = gen.VexFlags3 ?? throw new InvalidOperationException();
			XopFlags3 = gen.XopFlags3 ?? throw new InvalidOperationException();
			EvexFlags3 = gen.EvexFlags3 ?? throw new InvalidOperationException();
			AllowedPrefixes = gen.AllowedPrefixes ?? throw new InvalidOperationException();
			AllowedPrefixesMap = gen.AllowedPrefixesMap ?? throw new InvalidOperationException();
			LegacyFlags = gen.LegacyFlags ?? throw new InvalidOperationException();
			VexFlags = gen.VexFlags ?? throw new InvalidOperationException();
			XopFlags = gen.XopFlags ?? throw new InvalidOperationException();
			EvexFlags = gen.EvexFlags ?? throw new InvalidOperationException();
			D3nowFlags = gen.D3nowFlags ?? throw new InvalidOperationException();
			Sort(LegacyOpHandlers);
			Sort(VexOpHandlers);
			Sort(XopOpHandlers);
			Sort(EvexOpHandlers);
			if (new HashSet<EnumValue>(LegacyOpHandlers.Select(a => a.legacyOpKind)).Count != LegacyOpKindEnum.Instance.Values.Length)
				throw new InvalidOperationException();
			if (new HashSet<EnumValue>(VexOpHandlers.Select(a => a.vexOpKind)).Count != VexOpKindEnum.Instance.Values.Length)
				throw new InvalidOperationException();
			if (new HashSet<EnumValue>(XopOpHandlers.Select(a => a.xopOpKind)).Count != XopOpKindEnum.Instance.Values.Length)
				throw new InvalidOperationException();
			if (new HashSet<EnumValue>(EvexOpHandlers.Select(a => a.evexOpKind)).Count != EvexOpKindEnum.Instance.Values.Length)
				throw new InvalidOperationException();

			static void Sort((EnumValue opCodeOperandKind, EnumValue opKind, OpHandlerKind opHandlerKind, object[] args)[] handlers) =>
				Array.Sort(handlers, (a, b) => a.opKind.Value.CompareTo(b.opKind.Value));
		}
	}
}
