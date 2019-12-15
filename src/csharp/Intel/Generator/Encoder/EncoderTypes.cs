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
			(OpCodeOperandKindEnum.Instance["None"], LegacyOpKindEnum.Instance["None"], OpHandlerKind.None, new object[] { }),
			(OpCodeOperandKindEnum.Instance["farbr2_2"], LegacyOpKindEnum.Instance["Aww"], OpHandlerKind.OpA, new object[] { 2 }),
			(OpCodeOperandKindEnum.Instance["farbr4_2"], LegacyOpKindEnum.Instance["Adw"], OpHandlerKind.OpA, new object[] { 4 }),
			(OpCodeOperandKindEnum.Instance["mem"], LegacyOpKindEnum.Instance["M"], OpHandlerKind.OpModRM_rm_mem_only, new object[] { }),
			(OpCodeOperandKindEnum.Instance["mem"], LegacyOpKindEnum.Instance["Mfbcd"], OpHandlerKind.OpModRM_rm_mem_only, new object[] { }),
			(OpCodeOperandKindEnum.Instance["mem"], LegacyOpKindEnum.Instance["Mf32"], OpHandlerKind.OpModRM_rm_mem_only, new object[] { }),
			(OpCodeOperandKindEnum.Instance["mem"], LegacyOpKindEnum.Instance["Mf64"], OpHandlerKind.OpModRM_rm_mem_only, new object[] { }),
			(OpCodeOperandKindEnum.Instance["mem"], LegacyOpKindEnum.Instance["Mf80"], OpHandlerKind.OpModRM_rm_mem_only, new object[] { }),
			(OpCodeOperandKindEnum.Instance["mem"], LegacyOpKindEnum.Instance["Mfi16"], OpHandlerKind.OpModRM_rm_mem_only, new object[] { }),
			(OpCodeOperandKindEnum.Instance["mem"], LegacyOpKindEnum.Instance["Mfi32"], OpHandlerKind.OpModRM_rm_mem_only, new object[] { }),
			(OpCodeOperandKindEnum.Instance["mem"], LegacyOpKindEnum.Instance["Mfi64"], OpHandlerKind.OpModRM_rm_mem_only, new object[] { }),
			(OpCodeOperandKindEnum.Instance["mem"], LegacyOpKindEnum.Instance["M14"], OpHandlerKind.OpModRM_rm_mem_only, new object[] { }),
			(OpCodeOperandKindEnum.Instance["mem"], LegacyOpKindEnum.Instance["M28"], OpHandlerKind.OpModRM_rm_mem_only, new object[] { }),
			(OpCodeOperandKindEnum.Instance["mem"], LegacyOpKindEnum.Instance["M98"], OpHandlerKind.OpModRM_rm_mem_only, new object[] { }),
			(OpCodeOperandKindEnum.Instance["mem"], LegacyOpKindEnum.Instance["M108"], OpHandlerKind.OpModRM_rm_mem_only, new object[] { }),
			(OpCodeOperandKindEnum.Instance["mem"], LegacyOpKindEnum.Instance["Mp"], OpHandlerKind.OpModRM_rm_mem_only, new object[] { }),
			(OpCodeOperandKindEnum.Instance["mem"], LegacyOpKindEnum.Instance["Ms"], OpHandlerKind.OpModRM_rm_mem_only, new object[] { }),
			(OpCodeOperandKindEnum.Instance["mem"], LegacyOpKindEnum.Instance["Mo"], OpHandlerKind.OpModRM_rm_mem_only, new object[] { }),
			(OpCodeOperandKindEnum.Instance["mem"], LegacyOpKindEnum.Instance["Mb"], OpHandlerKind.OpModRM_rm_mem_only, new object[] { }),
			(OpCodeOperandKindEnum.Instance["mem"], LegacyOpKindEnum.Instance["Mw"], OpHandlerKind.OpModRM_rm_mem_only, new object[] { }),
			(OpCodeOperandKindEnum.Instance["mem"], LegacyOpKindEnum.Instance["Md"], OpHandlerKind.OpModRM_rm_mem_only, new object[] { }),
			(OpCodeOperandKindEnum.Instance["mem_mpx"], LegacyOpKindEnum.Instance["Md_MPX"], OpHandlerKind.OpModRM_rm_mem_only, new object[] { }),
			(OpCodeOperandKindEnum.Instance["mem"], LegacyOpKindEnum.Instance["Mq"], OpHandlerKind.OpModRM_rm_mem_only, new object[] { }),
			(OpCodeOperandKindEnum.Instance["mem_mpx"], LegacyOpKindEnum.Instance["Mq_MPX"], OpHandlerKind.OpModRM_rm_mem_only, new object[] { }),
			(OpCodeOperandKindEnum.Instance["mem"], LegacyOpKindEnum.Instance["Mw2"], OpHandlerKind.OpModRM_rm_mem_only, new object[] { }),
			(OpCodeOperandKindEnum.Instance["mem"], LegacyOpKindEnum.Instance["Md2"], OpHandlerKind.OpModRM_rm_mem_only, new object[] { }),
			(OpCodeOperandKindEnum.Instance["r8_or_mem"], LegacyOpKindEnum.Instance["Eb"], OpHandlerKind.OpModRM_rm, new object[] { RegisterEnum.Instance["AL"], RegisterEnum.Instance["R15L"] }),
			(OpCodeOperandKindEnum.Instance["r16_or_mem"], LegacyOpKindEnum.Instance["Ew"], OpHandlerKind.OpModRM_rm, new object[] { RegisterEnum.Instance["AX"], RegisterEnum.Instance["R15W"] }),
			(OpCodeOperandKindEnum.Instance["r32_or_mem"], LegacyOpKindEnum.Instance["Ed"], OpHandlerKind.OpModRM_rm, new object[] { RegisterEnum.Instance["EAX"], RegisterEnum.Instance["R15D"] }),
			(OpCodeOperandKindEnum.Instance["r32_or_mem_mpx"], LegacyOpKindEnum.Instance["Ed_MPX"], OpHandlerKind.OpModRM_rm, new object[] { RegisterEnum.Instance["EAX"], RegisterEnum.Instance["R15D"] }),
			(OpCodeOperandKindEnum.Instance["r32_or_mem"], LegacyOpKindEnum.Instance["Ew_d"], OpHandlerKind.OpModRM_rm, new object[] { RegisterEnum.Instance["EAX"], RegisterEnum.Instance["R15D"] }),
			(OpCodeOperandKindEnum.Instance["r64_or_mem"], LegacyOpKindEnum.Instance["Ew_q"], OpHandlerKind.OpModRM_rm, new object[] { RegisterEnum.Instance["RAX"], RegisterEnum.Instance["R15"] }),
			(OpCodeOperandKindEnum.Instance["r64_or_mem"], LegacyOpKindEnum.Instance["Eq"], OpHandlerKind.OpModRM_rm, new object[] { RegisterEnum.Instance["RAX"], RegisterEnum.Instance["R15"] }),
			(OpCodeOperandKindEnum.Instance["r64_or_mem_mpx"], LegacyOpKindEnum.Instance["Eq_MPX"], OpHandlerKind.OpModRM_rm, new object[] { RegisterEnum.Instance["RAX"], RegisterEnum.Instance["R15"] }),
			(OpCodeOperandKindEnum.Instance["mem"], LegacyOpKindEnum.Instance["Eww"], OpHandlerKind.OpModRM_rm_mem_only, new object[] { }),
			(OpCodeOperandKindEnum.Instance["mem"], LegacyOpKindEnum.Instance["Edw"], OpHandlerKind.OpModRM_rm_mem_only, new object[] { }),
			(OpCodeOperandKindEnum.Instance["mem"], LegacyOpKindEnum.Instance["Eqw"], OpHandlerKind.OpModRM_rm_mem_only, new object[] { }),
			(OpCodeOperandKindEnum.Instance["r32_or_mem"], LegacyOpKindEnum.Instance["RdMb"], OpHandlerKind.OpModRM_rm, new object[] { RegisterEnum.Instance["EAX"], RegisterEnum.Instance["R15D"] }),
			(OpCodeOperandKindEnum.Instance["r64_or_mem"], LegacyOpKindEnum.Instance["RqMb"], OpHandlerKind.OpModRM_rm, new object[] { RegisterEnum.Instance["RAX"], RegisterEnum.Instance["R15"] }),
			(OpCodeOperandKindEnum.Instance["r32_or_mem"], LegacyOpKindEnum.Instance["RdMw"], OpHandlerKind.OpModRM_rm, new object[] { RegisterEnum.Instance["EAX"], RegisterEnum.Instance["R15D"] }),
			(OpCodeOperandKindEnum.Instance["r64_or_mem"], LegacyOpKindEnum.Instance["RqMw"], OpHandlerKind.OpModRM_rm, new object[] { RegisterEnum.Instance["RAX"], RegisterEnum.Instance["R15"] }),
			(OpCodeOperandKindEnum.Instance["r8_reg"], LegacyOpKindEnum.Instance["Gb"], OpHandlerKind.OpModRM_reg, new object[] { RegisterEnum.Instance["AL"], RegisterEnum.Instance["R15L"] }),
			(OpCodeOperandKindEnum.Instance["r16_reg"], LegacyOpKindEnum.Instance["Gw"], OpHandlerKind.OpModRM_reg, new object[] { RegisterEnum.Instance["AX"], RegisterEnum.Instance["R15W"] }),
			(OpCodeOperandKindEnum.Instance["r32_reg"], LegacyOpKindEnum.Instance["Gd"], OpHandlerKind.OpModRM_reg, new object[] { RegisterEnum.Instance["EAX"], RegisterEnum.Instance["R15D"] }),
			(OpCodeOperandKindEnum.Instance["r64_reg"], LegacyOpKindEnum.Instance["Gq"], OpHandlerKind.OpModRM_reg, new object[] { RegisterEnum.Instance["RAX"], RegisterEnum.Instance["R15"] }),
			(OpCodeOperandKindEnum.Instance["r16_rm"], LegacyOpKindEnum.Instance["Rw"], OpHandlerKind.OpModRM_rm_reg_only, new object[] { RegisterEnum.Instance["AX"], RegisterEnum.Instance["R15W"] }),
			(OpCodeOperandKindEnum.Instance["r32_rm"], LegacyOpKindEnum.Instance["Rd"], OpHandlerKind.OpModRM_rm_reg_only, new object[] { RegisterEnum.Instance["EAX"], RegisterEnum.Instance["R15D"] }),
			(OpCodeOperandKindEnum.Instance["r64_rm"], LegacyOpKindEnum.Instance["Rq"], OpHandlerKind.OpModRM_rm_reg_only, new object[] { RegisterEnum.Instance["RAX"], RegisterEnum.Instance["R15"] }),
			(OpCodeOperandKindEnum.Instance["seg_reg"], LegacyOpKindEnum.Instance["Sw"], OpHandlerKind.OpModRM_reg, new object[] { RegisterEnum.Instance["ES"], RegisterEnum.Instance["GS"] }),
			(OpCodeOperandKindEnum.Instance["cr_reg"], LegacyOpKindEnum.Instance["Cd"], OpHandlerKind.OpModRM_regF0, new object[] { RegisterEnum.Instance["CR0"], RegisterEnum.Instance["CR15"] }),
			(OpCodeOperandKindEnum.Instance["cr_reg"], LegacyOpKindEnum.Instance["Cq"], OpHandlerKind.OpModRM_reg, new object[] { RegisterEnum.Instance["CR0"], RegisterEnum.Instance["CR15"] }),
			(OpCodeOperandKindEnum.Instance["dr_reg"], LegacyOpKindEnum.Instance["Dd"], OpHandlerKind.OpModRM_reg, new object[] { RegisterEnum.Instance["DR0"], RegisterEnum.Instance["DR15"] }),
			(OpCodeOperandKindEnum.Instance["dr_reg"], LegacyOpKindEnum.Instance["Dq"], OpHandlerKind.OpModRM_reg, new object[] { RegisterEnum.Instance["DR0"], RegisterEnum.Instance["DR15"] }),
			(OpCodeOperandKindEnum.Instance["tr_reg"], LegacyOpKindEnum.Instance["Td"], OpHandlerKind.OpModRM_reg, new object[] { RegisterEnum.Instance["TR0"], RegisterEnum.Instance["TR7"] }),
			(OpCodeOperandKindEnum.Instance["imm8"], LegacyOpKindEnum.Instance["Ib"], OpHandlerKind.OpIb, new object[] { OpKindEnum.Instance["Immediate8"] }),
			(OpCodeOperandKindEnum.Instance["imm8sex16"], LegacyOpKindEnum.Instance["Ib16"], OpHandlerKind.OpIb, new object[] { OpKindEnum.Instance["Immediate8to16"] }),
			(OpCodeOperandKindEnum.Instance["imm8sex32"], LegacyOpKindEnum.Instance["Ib32"], OpHandlerKind.OpIb, new object[] { OpKindEnum.Instance["Immediate8to32"] }),
			(OpCodeOperandKindEnum.Instance["imm8sex64"], LegacyOpKindEnum.Instance["Ib64"], OpHandlerKind.OpIb, new object[] { OpKindEnum.Instance["Immediate8to64"] }),
			(OpCodeOperandKindEnum.Instance["imm16"], LegacyOpKindEnum.Instance["Iw"], OpHandlerKind.OpIw, new object[] { }),
			(OpCodeOperandKindEnum.Instance["imm32"], LegacyOpKindEnum.Instance["Id"], OpHandlerKind.OpId, new object[] { OpKindEnum.Instance["Immediate32"] }),
			(OpCodeOperandKindEnum.Instance["imm32sex64"], LegacyOpKindEnum.Instance["Id64"], OpHandlerKind.OpId, new object[] { OpKindEnum.Instance["Immediate32to64"] }),
			(OpCodeOperandKindEnum.Instance["imm64"], LegacyOpKindEnum.Instance["Iq"], OpHandlerKind.OpIq, new object[] { }),
			(OpCodeOperandKindEnum.Instance["imm8"], LegacyOpKindEnum.Instance["Ib21"], OpHandlerKind.OpIb21, new object[] { }),
			(OpCodeOperandKindEnum.Instance["imm8"], LegacyOpKindEnum.Instance["Ib11"], OpHandlerKind.OpIb11, new object[] { }),
			(OpCodeOperandKindEnum.Instance["seg_rSI"], LegacyOpKindEnum.Instance["Xb"], OpHandlerKind.OpX, new object[] { }),
			(OpCodeOperandKindEnum.Instance["seg_rSI"], LegacyOpKindEnum.Instance["Xw"], OpHandlerKind.OpX, new object[] { }),
			(OpCodeOperandKindEnum.Instance["seg_rSI"], LegacyOpKindEnum.Instance["Xd"], OpHandlerKind.OpX, new object[] { }),
			(OpCodeOperandKindEnum.Instance["seg_rSI"], LegacyOpKindEnum.Instance["Xq"], OpHandlerKind.OpX, new object[] { }),
			(OpCodeOperandKindEnum.Instance["es_rDI"], LegacyOpKindEnum.Instance["Yb"], OpHandlerKind.OpY, new object[] { }),
			(OpCodeOperandKindEnum.Instance["es_rDI"], LegacyOpKindEnum.Instance["Yw"], OpHandlerKind.OpY, new object[] { }),
			(OpCodeOperandKindEnum.Instance["es_rDI"], LegacyOpKindEnum.Instance["Yd"], OpHandlerKind.OpY, new object[] { }),
			(OpCodeOperandKindEnum.Instance["es_rDI"], LegacyOpKindEnum.Instance["Yq"], OpHandlerKind.OpY, new object[] { }),
			(OpCodeOperandKindEnum.Instance["br16_1"], LegacyOpKindEnum.Instance["wJb"], OpHandlerKind.OpJ, new object[] { OpKindEnum.Instance["NearBranch16"], 1 }),
			(OpCodeOperandKindEnum.Instance["br32_1"], LegacyOpKindEnum.Instance["dJb"], OpHandlerKind.OpJ, new object[] { OpKindEnum.Instance["NearBranch32"], 1 }),
			(OpCodeOperandKindEnum.Instance["br64_1"], LegacyOpKindEnum.Instance["qJb"], OpHandlerKind.OpJ, new object[] { OpKindEnum.Instance["NearBranch64"], 1 }),
			(OpCodeOperandKindEnum.Instance["br16_2"], LegacyOpKindEnum.Instance["Jw"], OpHandlerKind.OpJ, new object[] { OpKindEnum.Instance["NearBranch16"], 2 }),
			(OpCodeOperandKindEnum.Instance["br32_4"], LegacyOpKindEnum.Instance["wJd"], OpHandlerKind.OpJ, new object[] { OpKindEnum.Instance["NearBranch32"], 4 }),
			(OpCodeOperandKindEnum.Instance["br32_4"], LegacyOpKindEnum.Instance["dJd"], OpHandlerKind.OpJ, new object[] { OpKindEnum.Instance["NearBranch32"], 4 }),
			(OpCodeOperandKindEnum.Instance["br64_4"], LegacyOpKindEnum.Instance["qJd"], OpHandlerKind.OpJ, new object[] { OpKindEnum.Instance["NearBranch64"], 4 }),
			(OpCodeOperandKindEnum.Instance["xbegin_2"], LegacyOpKindEnum.Instance["Jxw"], OpHandlerKind.OpJx, new object[] { 2 }),
			(OpCodeOperandKindEnum.Instance["xbegin_4"], LegacyOpKindEnum.Instance["Jxd"], OpHandlerKind.OpJx, new object[] { 4 }),
			(OpCodeOperandKindEnum.Instance["brdisp_2"], LegacyOpKindEnum.Instance["Jdisp16"], OpHandlerKind.OpJdisp, new object[] { 2 }),
			(OpCodeOperandKindEnum.Instance["brdisp_4"], LegacyOpKindEnum.Instance["Jdisp32"], OpHandlerKind.OpJdisp, new object[] { 4 }),
			(OpCodeOperandKindEnum.Instance["mem_offs"], LegacyOpKindEnum.Instance["Ob"], OpHandlerKind.OpO, new object[] { }),
			(OpCodeOperandKindEnum.Instance["mem_offs"], LegacyOpKindEnum.Instance["Ow"], OpHandlerKind.OpO, new object[] { }),
			(OpCodeOperandKindEnum.Instance["mem_offs"], LegacyOpKindEnum.Instance["Od"], OpHandlerKind.OpO, new object[] { }),
			(OpCodeOperandKindEnum.Instance["mem_offs"], LegacyOpKindEnum.Instance["Oq"], OpHandlerKind.OpO, new object[] { }),
			(OpCodeOperandKindEnum.Instance["imm8_const_1"], LegacyOpKindEnum.Instance["Imm1"], OpHandlerKind.OpImm, new object[] { 1 }),
			(OpCodeOperandKindEnum.Instance["bnd_reg"], LegacyOpKindEnum.Instance["B"], OpHandlerKind.OpModRM_reg, new object[] { RegisterEnum.Instance["BND0"], RegisterEnum.Instance["BND3"] }),
			(OpCodeOperandKindEnum.Instance["bnd_or_mem_mpx"], LegacyOpKindEnum.Instance["BMq"], OpHandlerKind.OpModRM_rm, new object[] { RegisterEnum.Instance["BND0"], RegisterEnum.Instance["BND3"] }),
			(OpCodeOperandKindEnum.Instance["bnd_or_mem_mpx"], LegacyOpKindEnum.Instance["BMo"], OpHandlerKind.OpModRM_rm, new object[] { RegisterEnum.Instance["BND0"], RegisterEnum.Instance["BND3"] }),
			(OpCodeOperandKindEnum.Instance["mem_mib"], LegacyOpKindEnum.Instance["MIB"], OpHandlerKind.OpModRM_rm_mem_only, new object[] { }),
			(OpCodeOperandKindEnum.Instance["mm_rm"], LegacyOpKindEnum.Instance["N"], OpHandlerKind.OpModRM_rm_reg_only, new object[] { RegisterEnum.Instance["MM0"], RegisterEnum.Instance["MM7"] }),
			(OpCodeOperandKindEnum.Instance["mm_reg"], LegacyOpKindEnum.Instance["P"], OpHandlerKind.OpModRM_reg, new object[] { RegisterEnum.Instance["MM0"], RegisterEnum.Instance["MM7"] }),
			(OpCodeOperandKindEnum.Instance["mm_or_mem"], LegacyOpKindEnum.Instance["Q"], OpHandlerKind.OpModRM_rm, new object[] { RegisterEnum.Instance["MM0"], RegisterEnum.Instance["MM7"] }),
			(OpCodeOperandKindEnum.Instance["xmm_rm"], LegacyOpKindEnum.Instance["RX"], OpHandlerKind.OpModRM_rm_reg_only, new object[] { RegisterEnum.Instance["XMM0"], RegisterEnum.Instance["XMM15"] }),
			(OpCodeOperandKindEnum.Instance["xmm_reg"], LegacyOpKindEnum.Instance["VX"], OpHandlerKind.OpModRM_reg, new object[] { RegisterEnum.Instance["XMM0"], RegisterEnum.Instance["XMM15"] }),
			(OpCodeOperandKindEnum.Instance["xmm_or_mem"], LegacyOpKindEnum.Instance["WX"], OpHandlerKind.OpModRM_rm, new object[] { RegisterEnum.Instance["XMM0"], RegisterEnum.Instance["XMM15"] }),
			(OpCodeOperandKindEnum.Instance["seg_rDI"], LegacyOpKindEnum.Instance["rDI"], OpHandlerKind.OprDI, new object[] { }),
			(OpCodeOperandKindEnum.Instance["seg_rBX_al"], LegacyOpKindEnum.Instance["MRBX"], OpHandlerKind.OpMRBX, new object[] { }),
			(OpCodeOperandKindEnum.Instance["es"], LegacyOpKindEnum.Instance["ES"], OpHandlerKind.OpReg, new object[] { RegisterEnum.Instance["ES"] }),
			(OpCodeOperandKindEnum.Instance["cs"], LegacyOpKindEnum.Instance["CS"], OpHandlerKind.OpReg, new object[] { RegisterEnum.Instance["CS"] }),
			(OpCodeOperandKindEnum.Instance["ss"], LegacyOpKindEnum.Instance["SS"], OpHandlerKind.OpReg, new object[] { RegisterEnum.Instance["SS"] }),
			(OpCodeOperandKindEnum.Instance["ds"], LegacyOpKindEnum.Instance["DS"], OpHandlerKind.OpReg, new object[] { RegisterEnum.Instance["DS"] }),
			(OpCodeOperandKindEnum.Instance["fs"], LegacyOpKindEnum.Instance["FS"], OpHandlerKind.OpReg, new object[] { RegisterEnum.Instance["FS"] }),
			(OpCodeOperandKindEnum.Instance["gs"], LegacyOpKindEnum.Instance["GS"], OpHandlerKind.OpReg, new object[] { RegisterEnum.Instance["GS"] }),
			(OpCodeOperandKindEnum.Instance["al"], LegacyOpKindEnum.Instance["AL"], OpHandlerKind.OpReg, new object[] { RegisterEnum.Instance["AL"] }),
			(OpCodeOperandKindEnum.Instance["cl"], LegacyOpKindEnum.Instance["CL"], OpHandlerKind.OpReg, new object[] { RegisterEnum.Instance["CL"] }),
			(OpCodeOperandKindEnum.Instance["ax"], LegacyOpKindEnum.Instance["AX"], OpHandlerKind.OpReg, new object[] { RegisterEnum.Instance["AX"] }),
			(OpCodeOperandKindEnum.Instance["dx"], LegacyOpKindEnum.Instance["DX"], OpHandlerKind.OpReg, new object[] { RegisterEnum.Instance["DX"] }),
			(OpCodeOperandKindEnum.Instance["eax"], LegacyOpKindEnum.Instance["EAX"], OpHandlerKind.OpReg, new object[] { RegisterEnum.Instance["EAX"] }),
			(OpCodeOperandKindEnum.Instance["rax"], LegacyOpKindEnum.Instance["RAX"], OpHandlerKind.OpReg, new object[] { RegisterEnum.Instance["RAX"] }),
			(OpCodeOperandKindEnum.Instance["st0"], LegacyOpKindEnum.Instance["ST"], OpHandlerKind.OpReg, new object[] { RegisterEnum.Instance["ST0"] }),
			(OpCodeOperandKindEnum.Instance["sti_opcode"], LegacyOpKindEnum.Instance["STi"], OpHandlerKind.OpRegSTi, new object[] { }),
			(OpCodeOperandKindEnum.Instance["r8_opcode"], LegacyOpKindEnum.Instance["r8_rb"], OpHandlerKind.OpRegEmbed8, new object[] { RegisterEnum.Instance["AL"], RegisterEnum.Instance["R15L"] }),
			(OpCodeOperandKindEnum.Instance["r16_opcode"], LegacyOpKindEnum.Instance["r16_rw"], OpHandlerKind.OpRegEmbed8, new object[] { RegisterEnum.Instance["AX"], RegisterEnum.Instance["R15W"] }),
			(OpCodeOperandKindEnum.Instance["r32_opcode"], LegacyOpKindEnum.Instance["r32_rd"], OpHandlerKind.OpRegEmbed8, new object[] { RegisterEnum.Instance["EAX"], RegisterEnum.Instance["R15D"] }),
			(OpCodeOperandKindEnum.Instance["r64_opcode"], LegacyOpKindEnum.Instance["r64_ro"], OpHandlerKind.OpRegEmbed8, new object[] { RegisterEnum.Instance["RAX"], RegisterEnum.Instance["R15"] }),
		};
		public static (EnumValue opCodeOperandKind, EnumValue vexOpKind, OpHandlerKind opHandlerKind, object[] args)[] VexOpHandlers { get; } = new (EnumValue opCodeOperandKind, EnumValue vexOpKind, OpHandlerKind opHandlerKind, object[] args)[] {
			(OpCodeOperandKindEnum.Instance["None"], VexOpKindEnum.Instance["None"], OpHandlerKind.None, new object[] { }),
			(OpCodeOperandKindEnum.Instance["r32_or_mem"], VexOpKindEnum.Instance["Ed"], OpHandlerKind.OpModRM_rm, new object[] { RegisterEnum.Instance["EAX"], RegisterEnum.Instance["R15D"] }),
			(OpCodeOperandKindEnum.Instance["r64_or_mem"], VexOpKindEnum.Instance["Eq"], OpHandlerKind.OpModRM_rm, new object[] { RegisterEnum.Instance["RAX"], RegisterEnum.Instance["R15"] }),
			(OpCodeOperandKindEnum.Instance["r32_reg"], VexOpKindEnum.Instance["Gd"], OpHandlerKind.OpModRM_reg, new object[] { RegisterEnum.Instance["EAX"], RegisterEnum.Instance["R15D"] }),
			(OpCodeOperandKindEnum.Instance["r64_reg"], VexOpKindEnum.Instance["Gq"], OpHandlerKind.OpModRM_reg, new object[] { RegisterEnum.Instance["RAX"], RegisterEnum.Instance["R15"] }),
			(OpCodeOperandKindEnum.Instance["r32_or_mem"], VexOpKindEnum.Instance["RdMb"], OpHandlerKind.OpModRM_rm, new object[] { RegisterEnum.Instance["EAX"], RegisterEnum.Instance["R15D"] }),
			(OpCodeOperandKindEnum.Instance["r64_or_mem"], VexOpKindEnum.Instance["RqMb"], OpHandlerKind.OpModRM_rm, new object[] { RegisterEnum.Instance["RAX"], RegisterEnum.Instance["R15"] }),
			(OpCodeOperandKindEnum.Instance["r32_or_mem"], VexOpKindEnum.Instance["RdMw"], OpHandlerKind.OpModRM_rm, new object[] { RegisterEnum.Instance["EAX"], RegisterEnum.Instance["R15D"] }),
			(OpCodeOperandKindEnum.Instance["r64_or_mem"], VexOpKindEnum.Instance["RqMw"], OpHandlerKind.OpModRM_rm, new object[] { RegisterEnum.Instance["RAX"], RegisterEnum.Instance["R15"] }),
			(OpCodeOperandKindEnum.Instance["r32_rm"], VexOpKindEnum.Instance["Rd"], OpHandlerKind.OpModRM_rm_reg_only, new object[] { RegisterEnum.Instance["EAX"], RegisterEnum.Instance["R15D"] }),
			(OpCodeOperandKindEnum.Instance["r64_rm"], VexOpKindEnum.Instance["Rq"], OpHandlerKind.OpModRM_rm_reg_only, new object[] { RegisterEnum.Instance["RAX"], RegisterEnum.Instance["R15"] }),
			(OpCodeOperandKindEnum.Instance["r32_vvvv"], VexOpKindEnum.Instance["Hd"], OpHandlerKind.OpHx, new object[] { RegisterEnum.Instance["EAX"], RegisterEnum.Instance["R15D"] }),
			(OpCodeOperandKindEnum.Instance["r64_vvvv"], VexOpKindEnum.Instance["Hq"], OpHandlerKind.OpHx, new object[] { RegisterEnum.Instance["RAX"], RegisterEnum.Instance["R15"] }),
			(OpCodeOperandKindEnum.Instance["k_vvvv"], VexOpKindEnum.Instance["HK"], OpHandlerKind.OpHx, new object[] { RegisterEnum.Instance["K0"], RegisterEnum.Instance["K7"] }),
			(OpCodeOperandKindEnum.Instance["xmm_vvvv"], VexOpKindEnum.Instance["HX"], OpHandlerKind.OpHx, new object[] { RegisterEnum.Instance["XMM0"], RegisterEnum.Instance["XMM15"] }),
			(OpCodeOperandKindEnum.Instance["ymm_vvvv"], VexOpKindEnum.Instance["HY"], OpHandlerKind.OpHx, new object[] { RegisterEnum.Instance["YMM0"], RegisterEnum.Instance["YMM15"] }),
			(OpCodeOperandKindEnum.Instance["imm8"], VexOpKindEnum.Instance["Ib"], OpHandlerKind.OpIb, new object[] { OpKindEnum.Instance["Immediate8"] }),
			(OpCodeOperandKindEnum.Instance["imm2_m2z"], VexOpKindEnum.Instance["I2"], OpHandlerKind.OpI2, new object[] { }),
			(OpCodeOperandKindEnum.Instance["xmm_is4"], VexOpKindEnum.Instance["Is4X"], OpHandlerKind.OpIs4x, new object[] { RegisterEnum.Instance["XMM0"], RegisterEnum.Instance["XMM15"] }),
			(OpCodeOperandKindEnum.Instance["ymm_is4"], VexOpKindEnum.Instance["Is4Y"], OpHandlerKind.OpIs4x, new object[] { RegisterEnum.Instance["YMM0"], RegisterEnum.Instance["YMM15"] }),
			(OpCodeOperandKindEnum.Instance["xmm_is5"], VexOpKindEnum.Instance["Is5X"], OpHandlerKind.OpIs4x, new object[] { RegisterEnum.Instance["XMM0"], RegisterEnum.Instance["XMM15"] }),
			(OpCodeOperandKindEnum.Instance["ymm_is5"], VexOpKindEnum.Instance["Is5Y"], OpHandlerKind.OpIs4x, new object[] { RegisterEnum.Instance["YMM0"], RegisterEnum.Instance["YMM15"] }),
			(OpCodeOperandKindEnum.Instance["mem"], VexOpKindEnum.Instance["M"], OpHandlerKind.OpModRM_rm_mem_only, new object[] { }),
			(OpCodeOperandKindEnum.Instance["mem"], VexOpKindEnum.Instance["Md"], OpHandlerKind.OpModRM_rm_mem_only, new object[] { }),
			(OpCodeOperandKindEnum.Instance["mem"], VexOpKindEnum.Instance["MK"], OpHandlerKind.OpModRM_rm_mem_only, new object[] { }),
			(OpCodeOperandKindEnum.Instance["seg_rDI"], VexOpKindEnum.Instance["rDI"], OpHandlerKind.OprDI, new object[] { }),
			(OpCodeOperandKindEnum.Instance["k_rm"], VexOpKindEnum.Instance["RK"], OpHandlerKind.OpModRM_rm_reg_only, new object[] { RegisterEnum.Instance["K0"], RegisterEnum.Instance["K7"] }),
			(OpCodeOperandKindEnum.Instance["xmm_rm"], VexOpKindEnum.Instance["RX"], OpHandlerKind.OpModRM_rm_reg_only, new object[] { RegisterEnum.Instance["XMM0"], RegisterEnum.Instance["XMM15"] }),
			(OpCodeOperandKindEnum.Instance["ymm_rm"], VexOpKindEnum.Instance["RY"], OpHandlerKind.OpModRM_rm_reg_only, new object[] { RegisterEnum.Instance["YMM0"], RegisterEnum.Instance["YMM15"] }),
			(OpCodeOperandKindEnum.Instance["k_reg"], VexOpKindEnum.Instance["VK"], OpHandlerKind.OpModRM_reg, new object[] { RegisterEnum.Instance["K0"], RegisterEnum.Instance["K7"] }),
			(OpCodeOperandKindEnum.Instance["mem_vsib32x"], VexOpKindEnum.Instance["VM32X"], OpHandlerKind.OpVMx, new object[] { RegisterEnum.Instance["XMM0"], RegisterEnum.Instance["XMM15"] }),
			(OpCodeOperandKindEnum.Instance["mem_vsib32y"], VexOpKindEnum.Instance["VM32Y"], OpHandlerKind.OpVMx, new object[] { RegisterEnum.Instance["YMM0"], RegisterEnum.Instance["YMM15"] }),
			(OpCodeOperandKindEnum.Instance["mem_vsib64x"], VexOpKindEnum.Instance["VM64X"], OpHandlerKind.OpVMx, new object[] { RegisterEnum.Instance["XMM0"], RegisterEnum.Instance["XMM15"] }),
			(OpCodeOperandKindEnum.Instance["mem_vsib64y"], VexOpKindEnum.Instance["VM64Y"], OpHandlerKind.OpVMx, new object[] { RegisterEnum.Instance["YMM0"], RegisterEnum.Instance["YMM15"] }),
			(OpCodeOperandKindEnum.Instance["xmm_reg"], VexOpKindEnum.Instance["VX"], OpHandlerKind.OpModRM_reg, new object[] { RegisterEnum.Instance["XMM0"], RegisterEnum.Instance["XMM15"] }),
			(OpCodeOperandKindEnum.Instance["ymm_reg"], VexOpKindEnum.Instance["VY"], OpHandlerKind.OpModRM_reg, new object[] { RegisterEnum.Instance["YMM0"], RegisterEnum.Instance["YMM15"] }),
			(OpCodeOperandKindEnum.Instance["k_or_mem"], VexOpKindEnum.Instance["WK"], OpHandlerKind.OpModRM_rm, new object[] { RegisterEnum.Instance["K0"], RegisterEnum.Instance["K7"] }),
			(OpCodeOperandKindEnum.Instance["xmm_or_mem"], VexOpKindEnum.Instance["WX"], OpHandlerKind.OpModRM_rm, new object[] { RegisterEnum.Instance["XMM0"], RegisterEnum.Instance["XMM15"] }),
			(OpCodeOperandKindEnum.Instance["ymm_or_mem"], VexOpKindEnum.Instance["WY"], OpHandlerKind.OpModRM_rm, new object[] { RegisterEnum.Instance["YMM0"], RegisterEnum.Instance["YMM15"] }),
		};
		public static (EnumValue opCodeOperandKind, EnumValue xopOpKind, OpHandlerKind opHandlerKind, object[] args)[] XopOpHandlers { get; } = new (EnumValue opCodeOperandKind, EnumValue xopOpKind, OpHandlerKind opHandlerKind, object[] args)[] {
			(OpCodeOperandKindEnum.Instance["None"], XopOpKindEnum.Instance["None"], OpHandlerKind.None, new object[] { }),
			(OpCodeOperandKindEnum.Instance["r32_or_mem"], XopOpKindEnum.Instance["Ed"], OpHandlerKind.OpModRM_rm, new object[] { RegisterEnum.Instance["EAX"], RegisterEnum.Instance["R15D"] }),
			(OpCodeOperandKindEnum.Instance["r64_or_mem"], XopOpKindEnum.Instance["Eq"], OpHandlerKind.OpModRM_rm, new object[] { RegisterEnum.Instance["RAX"], RegisterEnum.Instance["R15"] }),
			(OpCodeOperandKindEnum.Instance["r32_reg"], XopOpKindEnum.Instance["Gd"], OpHandlerKind.OpModRM_reg, new object[] { RegisterEnum.Instance["EAX"], RegisterEnum.Instance["R15D"] }),
			(OpCodeOperandKindEnum.Instance["r64_reg"], XopOpKindEnum.Instance["Gq"], OpHandlerKind.OpModRM_reg, new object[] { RegisterEnum.Instance["RAX"], RegisterEnum.Instance["R15"] }),
			(OpCodeOperandKindEnum.Instance["r32_rm"], XopOpKindEnum.Instance["Rd"], OpHandlerKind.OpModRM_rm_reg_only, new object[] { RegisterEnum.Instance["EAX"], RegisterEnum.Instance["R15D"] }),
			(OpCodeOperandKindEnum.Instance["r64_rm"], XopOpKindEnum.Instance["Rq"], OpHandlerKind.OpModRM_rm_reg_only, new object[] { RegisterEnum.Instance["RAX"], RegisterEnum.Instance["R15"] }),
			(OpCodeOperandKindEnum.Instance["r32_vvvv"], XopOpKindEnum.Instance["Hd"], OpHandlerKind.OpHx, new object[] { RegisterEnum.Instance["EAX"], RegisterEnum.Instance["R15D"] }),
			(OpCodeOperandKindEnum.Instance["r64_vvvv"], XopOpKindEnum.Instance["Hq"], OpHandlerKind.OpHx, new object[] { RegisterEnum.Instance["RAX"], RegisterEnum.Instance["R15"] }),
			(OpCodeOperandKindEnum.Instance["xmm_vvvv"], XopOpKindEnum.Instance["HX"], OpHandlerKind.OpHx, new object[] { RegisterEnum.Instance["XMM0"], RegisterEnum.Instance["XMM15"] }),
			(OpCodeOperandKindEnum.Instance["ymm_vvvv"], XopOpKindEnum.Instance["HY"], OpHandlerKind.OpHx, new object[] { RegisterEnum.Instance["YMM0"], RegisterEnum.Instance["YMM15"] }),
			(OpCodeOperandKindEnum.Instance["imm8"], XopOpKindEnum.Instance["Ib"], OpHandlerKind.OpIb, new object[] { OpKindEnum.Instance["Immediate8"] }),
			(OpCodeOperandKindEnum.Instance["imm32"], XopOpKindEnum.Instance["Id"], OpHandlerKind.OpId, new object[] { OpKindEnum.Instance["Immediate32"] }),
			(OpCodeOperandKindEnum.Instance["xmm_is4"], XopOpKindEnum.Instance["Is4X"], OpHandlerKind.OpIs4x, new object[] { RegisterEnum.Instance["XMM0"], RegisterEnum.Instance["XMM15"] }),
			(OpCodeOperandKindEnum.Instance["ymm_is4"], XopOpKindEnum.Instance["Is4Y"], OpHandlerKind.OpIs4x, new object[] { RegisterEnum.Instance["YMM0"], RegisterEnum.Instance["YMM15"] }),
			(OpCodeOperandKindEnum.Instance["xmm_reg"], XopOpKindEnum.Instance["VX"], OpHandlerKind.OpModRM_reg, new object[] { RegisterEnum.Instance["XMM0"], RegisterEnum.Instance["XMM15"] }),
			(OpCodeOperandKindEnum.Instance["ymm_reg"], XopOpKindEnum.Instance["VY"], OpHandlerKind.OpModRM_reg, new object[] { RegisterEnum.Instance["YMM0"], RegisterEnum.Instance["YMM15"] }),
			(OpCodeOperandKindEnum.Instance["xmm_or_mem"], XopOpKindEnum.Instance["WX"], OpHandlerKind.OpModRM_rm, new object[] { RegisterEnum.Instance["XMM0"], RegisterEnum.Instance["XMM15"] }),
			(OpCodeOperandKindEnum.Instance["ymm_or_mem"], XopOpKindEnum.Instance["WY"], OpHandlerKind.OpModRM_rm, new object[] { RegisterEnum.Instance["YMM0"], RegisterEnum.Instance["YMM15"] }),
		};
		public static (EnumValue opCodeOperandKind, EnumValue evexOpKind, OpHandlerKind opHandlerKind, object[] args)[] EvexOpHandlers { get; } = new (EnumValue opCodeOperandKind, EnumValue evexOpKind, OpHandlerKind opHandlerKind, object[] args)[] {
			(OpCodeOperandKindEnum.Instance["None"], EvexOpKindEnum.Instance["None"], OpHandlerKind.None, new object[] { }),
			(OpCodeOperandKindEnum.Instance["r32_or_mem"], EvexOpKindEnum.Instance["Ed"], OpHandlerKind.OpModRM_rm, new object[] { RegisterEnum.Instance["EAX"], RegisterEnum.Instance["R15D"] }),
			(OpCodeOperandKindEnum.Instance["r64_or_mem"], EvexOpKindEnum.Instance["Eq"], OpHandlerKind.OpModRM_rm, new object[] { RegisterEnum.Instance["RAX"], RegisterEnum.Instance["R15"] }),
			(OpCodeOperandKindEnum.Instance["r32_reg"], EvexOpKindEnum.Instance["Gd"], OpHandlerKind.OpModRM_reg, new object[] { RegisterEnum.Instance["EAX"], RegisterEnum.Instance["R15D"] }),
			(OpCodeOperandKindEnum.Instance["r64_reg"], EvexOpKindEnum.Instance["Gq"], OpHandlerKind.OpModRM_reg, new object[] { RegisterEnum.Instance["RAX"], RegisterEnum.Instance["R15"] }),
			(OpCodeOperandKindEnum.Instance["r32_or_mem"], EvexOpKindEnum.Instance["RdMb"], OpHandlerKind.OpModRM_rm, new object[] { RegisterEnum.Instance["EAX"], RegisterEnum.Instance["R15D"] }),
			(OpCodeOperandKindEnum.Instance["r64_or_mem"], EvexOpKindEnum.Instance["RqMb"], OpHandlerKind.OpModRM_rm, new object[] { RegisterEnum.Instance["RAX"], RegisterEnum.Instance["R15"] }),
			(OpCodeOperandKindEnum.Instance["r32_or_mem"], EvexOpKindEnum.Instance["RdMw"], OpHandlerKind.OpModRM_rm, new object[] { RegisterEnum.Instance["EAX"], RegisterEnum.Instance["R15D"] }),
			(OpCodeOperandKindEnum.Instance["r64_or_mem"], EvexOpKindEnum.Instance["RqMw"], OpHandlerKind.OpModRM_rm, new object[] { RegisterEnum.Instance["RAX"], RegisterEnum.Instance["R15"] }),
			(OpCodeOperandKindEnum.Instance["xmm_vvvv"], EvexOpKindEnum.Instance["HX"], OpHandlerKind.OpHx, new object[] { RegisterEnum.Instance["XMM0"], RegisterEnum.Instance["XMM31"] }),
			(OpCodeOperandKindEnum.Instance["ymm_vvvv"], EvexOpKindEnum.Instance["HY"], OpHandlerKind.OpHx, new object[] { RegisterEnum.Instance["YMM0"], RegisterEnum.Instance["YMM31"] }),
			(OpCodeOperandKindEnum.Instance["zmm_vvvv"], EvexOpKindEnum.Instance["HZ"], OpHandlerKind.OpHx, new object[] { RegisterEnum.Instance["ZMM0"], RegisterEnum.Instance["ZMM31"] }),
			(OpCodeOperandKindEnum.Instance["xmmp3_vvvv"], EvexOpKindEnum.Instance["HXP3"], OpHandlerKind.OpHx, new object[] { RegisterEnum.Instance["XMM0"], RegisterEnum.Instance["XMM31"] }),
			(OpCodeOperandKindEnum.Instance["zmmp3_vvvv"], EvexOpKindEnum.Instance["HZP3"], OpHandlerKind.OpHx, new object[] { RegisterEnum.Instance["ZMM0"], RegisterEnum.Instance["ZMM31"] }),
			(OpCodeOperandKindEnum.Instance["imm8"], EvexOpKindEnum.Instance["Ib"], OpHandlerKind.OpIb, new object[] { OpKindEnum.Instance["Immediate8"] }),
			(OpCodeOperandKindEnum.Instance["mem"], EvexOpKindEnum.Instance["M"], OpHandlerKind.OpModRM_rm_mem_only, new object[] { }),
			(OpCodeOperandKindEnum.Instance["r32_rm"], EvexOpKindEnum.Instance["Rd"], OpHandlerKind.OpModRM_rm_reg_only, new object[] { RegisterEnum.Instance["EAX"], RegisterEnum.Instance["R15D"] }),
			(OpCodeOperandKindEnum.Instance["r64_rm"], EvexOpKindEnum.Instance["Rq"], OpHandlerKind.OpModRM_rm_reg_only, new object[] { RegisterEnum.Instance["RAX"], RegisterEnum.Instance["R15"] }),
			(OpCodeOperandKindEnum.Instance["xmm_rm"], EvexOpKindEnum.Instance["RX"], OpHandlerKind.OpModRM_rm_reg_only, new object[] { RegisterEnum.Instance["XMM0"], RegisterEnum.Instance["XMM31"] }),
			(OpCodeOperandKindEnum.Instance["ymm_rm"], EvexOpKindEnum.Instance["RY"], OpHandlerKind.OpModRM_rm_reg_only, new object[] { RegisterEnum.Instance["YMM0"], RegisterEnum.Instance["YMM31"] }),
			(OpCodeOperandKindEnum.Instance["zmm_rm"], EvexOpKindEnum.Instance["RZ"], OpHandlerKind.OpModRM_rm_reg_only, new object[] { RegisterEnum.Instance["ZMM0"], RegisterEnum.Instance["ZMM31"] }),
			(OpCodeOperandKindEnum.Instance["k_rm"], EvexOpKindEnum.Instance["RK"], OpHandlerKind.OpModRM_rm_reg_only, new object[] { RegisterEnum.Instance["K0"], RegisterEnum.Instance["K7"] }),
			(OpCodeOperandKindEnum.Instance["mem_vsib32x"], EvexOpKindEnum.Instance["VM32X"], OpHandlerKind.OpVMx, new object[] { RegisterEnum.Instance["XMM0"], RegisterEnum.Instance["XMM31"] }),
			(OpCodeOperandKindEnum.Instance["mem_vsib32y"], EvexOpKindEnum.Instance["VM32Y"], OpHandlerKind.OpVMx, new object[] { RegisterEnum.Instance["YMM0"], RegisterEnum.Instance["YMM31"] }),
			(OpCodeOperandKindEnum.Instance["mem_vsib32z"], EvexOpKindEnum.Instance["VM32Z"], OpHandlerKind.OpVMx, new object[] { RegisterEnum.Instance["ZMM0"], RegisterEnum.Instance["ZMM31"] }),
			(OpCodeOperandKindEnum.Instance["mem_vsib64x"], EvexOpKindEnum.Instance["VM64X"], OpHandlerKind.OpVMx, new object[] { RegisterEnum.Instance["XMM0"], RegisterEnum.Instance["XMM31"] }),
			(OpCodeOperandKindEnum.Instance["mem_vsib64y"], EvexOpKindEnum.Instance["VM64Y"], OpHandlerKind.OpVMx, new object[] { RegisterEnum.Instance["YMM0"], RegisterEnum.Instance["YMM31"] }),
			(OpCodeOperandKindEnum.Instance["mem_vsib64z"], EvexOpKindEnum.Instance["VM64Z"], OpHandlerKind.OpVMx, new object[] { RegisterEnum.Instance["ZMM0"], RegisterEnum.Instance["ZMM31"] }),
			(OpCodeOperandKindEnum.Instance["k_reg"], EvexOpKindEnum.Instance["VK"], OpHandlerKind.OpModRM_reg, new object[] { RegisterEnum.Instance["K0"], RegisterEnum.Instance["K7"] }),
			(OpCodeOperandKindEnum.Instance["kp1_reg"], EvexOpKindEnum.Instance["VKP1"], OpHandlerKind.OpModRM_reg, new object[] { RegisterEnum.Instance["K0"], RegisterEnum.Instance["K7"] }),
			(OpCodeOperandKindEnum.Instance["xmm_reg"], EvexOpKindEnum.Instance["VX"], OpHandlerKind.OpModRM_reg, new object[] { RegisterEnum.Instance["XMM0"], RegisterEnum.Instance["XMM31"] }),
			(OpCodeOperandKindEnum.Instance["ymm_reg"], EvexOpKindEnum.Instance["VY"], OpHandlerKind.OpModRM_reg, new object[] { RegisterEnum.Instance["YMM0"], RegisterEnum.Instance["YMM31"] }),
			(OpCodeOperandKindEnum.Instance["zmm_reg"], EvexOpKindEnum.Instance["VZ"], OpHandlerKind.OpModRM_reg, new object[] { RegisterEnum.Instance["ZMM0"], RegisterEnum.Instance["ZMM31"] }),
			(OpCodeOperandKindEnum.Instance["xmm_or_mem"], EvexOpKindEnum.Instance["WX"], OpHandlerKind.OpModRM_rm, new object[] { RegisterEnum.Instance["XMM0"], RegisterEnum.Instance["XMM31"] }),
			(OpCodeOperandKindEnum.Instance["ymm_or_mem"], EvexOpKindEnum.Instance["WY"], OpHandlerKind.OpModRM_rm, new object[] { RegisterEnum.Instance["YMM0"], RegisterEnum.Instance["YMM31"] }),
			(OpCodeOperandKindEnum.Instance["zmm_or_mem"], EvexOpKindEnum.Instance["WZ"], OpHandlerKind.OpModRM_rm, new object[] { RegisterEnum.Instance["ZMM0"], RegisterEnum.Instance["ZMM31"] }),
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
			if (LegacyOpHandlers.Length != LegacyOpKindEnum.Instance.Values.Length)
				throw new InvalidOperationException();
			if (VexOpHandlers.Length != VexOpKindEnum.Instance.Values.Length)
				throw new InvalidOperationException();
			if (XopOpHandlers.Length != XopOpKindEnum.Instance.Values.Length)
				throw new InvalidOperationException();
			if (EvexOpHandlers.Length != EvexOpKindEnum.Instance.Values.Length)
				throw new InvalidOperationException();

			static void Sort((EnumValue opCodeOperandKind, EnumValue opKind, OpHandlerKind opHandlerKind, object[] args)[] handlers) =>
				Array.Sort(handlers, (a, b) => a.opKind.Value.CompareTo(b.opKind.Value));
		}
	}
}
