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
using Generator.IO;

namespace Generator.Decoder.Rust {
	sealed class LegacyDecoderTableSerializer : DecoderTableSerializer {
		public override string Name => "legacy";
		protected override (string name, object?[] handlers)[] Handlers => OpCodeHandlersTables_Legacy.GetHandlers();
		protected override (string origName, string newName)[] RootNames => new[] {
			(OpCodeHandlersTables_Legacy.OneByteHandlers, "HANDLERS_XX"),
		};

		protected override IEnumerable<string> ExtraUseStatements => new string[] {
			"use super::handlers_3dnow::OpCodeHandler_D3NOW;",
			"use super::handlers_evex::OpCodeHandler_EVEX;",
			"use super::handlers_fpu::*;",
			"use super::handlers_vex::{OpCodeHandler_VEX2, OpCodeHandler_VEX3, OpCodeHandler_XOP};",
		};

		protected override string GetHandlerTypeName(EnumValue handlerType) => GetHandlerTypeInfo(handlerType).name;

		(string name, bool hasModRM) GetHandlerTypeInfo(EnumValue handlerType) {
			if (handlerType.DeclaringType.TypeId != TypeIds.OpCodeHandlerKind)
				throw new InvalidOperationException();

			switch ((OpCodeHandlerKindEnum.Enum)handlerType.Value) {
			case OpCodeHandlerKindEnum.Enum.Bitness: return ("OpCodeHandler_Bitness", false);
			case OpCodeHandlerKindEnum.Enum.Bitness_DontReadModRM: return ("OpCodeHandler_Bitness_DontReadModRM", true);
			case OpCodeHandlerKindEnum.Enum.Invalid: return ("OpCodeHandler_Invalid", true);
			case OpCodeHandlerKindEnum.Enum.Invalid_NoModRM: return ("OpCodeHandler_Invalid", false);
			case OpCodeHandlerKindEnum.Enum.RM: return ("OpCodeHandler_RM", true);
			case OpCodeHandlerKindEnum.Enum.Options3: return ("OpCodeHandler_Options", false);
			case OpCodeHandlerKindEnum.Enum.Options5: return ("OpCodeHandler_Options", false);
			case OpCodeHandlerKindEnum.Enum.Options_DontReadModRM: return ("OpCodeHandler_Options_DontReadModRM", true);
			case OpCodeHandlerKindEnum.Enum.AnotherTable: return ("OpCodeHandler_AnotherTable", false);
			case OpCodeHandlerKindEnum.Enum.Group: return ("OpCodeHandler_Group", true);
			case OpCodeHandlerKindEnum.Enum.Group8x64: return ("OpCodeHandler_Group8x64", true);
			case OpCodeHandlerKindEnum.Enum.Group8x8: return ("OpCodeHandler_Group8x8", true);
			case OpCodeHandlerKindEnum.Enum.LegacyMandatoryPrefix_F3_F2: return ("OpCodeHandler_MandatoryPrefix_F3_F2", false);
			case OpCodeHandlerKindEnum.Enum.D3NOW: return ("OpCodeHandler_D3NOW", true);
			case OpCodeHandlerKindEnum.Enum.EVEX: return ("OpCodeHandler_EVEX", true);
			case OpCodeHandlerKindEnum.Enum.VEX2: return ("OpCodeHandler_VEX2", true);
			case OpCodeHandlerKindEnum.Enum.VEX3: return ("OpCodeHandler_VEX3", true);
			case OpCodeHandlerKindEnum.Enum.XOP: return ("OpCodeHandler_XOP", true);
			case OpCodeHandlerKindEnum.Enum.Mf_1: return ("OpCodeHandler_Mf", true);
			case OpCodeHandlerKindEnum.Enum.Mf_2a: return ("OpCodeHandler_Mf", true);
			case OpCodeHandlerKindEnum.Enum.Mf_2b: return ("OpCodeHandler_Mf", true);
			case OpCodeHandlerKindEnum.Enum.Simple: return ("OpCodeHandler_Simple", false);
			case OpCodeHandlerKindEnum.Enum.Simple_ModRM: return ("OpCodeHandler_Simple", true);
			case OpCodeHandlerKindEnum.Enum.ST_STi: return ("OpCodeHandler_ST_STi", true);
			case OpCodeHandlerKindEnum.Enum.STi: return ("OpCodeHandler_STi", true);
			case OpCodeHandlerKindEnum.Enum.STi_ST: return ("OpCodeHandler_STi_ST", true);
			case OpCodeHandlerKindEnum.Enum.Reg: return ("OpCodeHandler_Reg", false);
			case OpCodeHandlerKindEnum.Enum.RegIb: return ("OpCodeHandler_RegIb", false);
			case OpCodeHandlerKindEnum.Enum.IbReg: return ("OpCodeHandler_IbReg", false);
			case OpCodeHandlerKindEnum.Enum.AL_DX: return ("OpCodeHandler_AL_DX", false);
			case OpCodeHandlerKindEnum.Enum.DX_AL: return ("OpCodeHandler_DX_AL", false);
			case OpCodeHandlerKindEnum.Enum.Ib: return ("OpCodeHandler_Ib", false);
			case OpCodeHandlerKindEnum.Enum.Ib3: return ("OpCodeHandler_Ib3", true);
			case OpCodeHandlerKindEnum.Enum.MandatoryPrefix: return ("OpCodeHandler_MandatoryPrefix", true);
			case OpCodeHandlerKindEnum.Enum.MandatoryPrefix3: return ("OpCodeHandler_MandatoryPrefix3", true);
			case OpCodeHandlerKindEnum.Enum.MandatoryPrefix_F3_F2: return ("OpCodeHandler_MandatoryPrefix_F3_F2", false);
			case OpCodeHandlerKindEnum.Enum.MandatoryPrefix_NoModRM: return ("OpCodeHandler_MandatoryPrefix", false);
			case OpCodeHandlerKindEnum.Enum.NIb: return ("OpCodeHandler_NIb", true);
			case OpCodeHandlerKindEnum.Enum.ReservedNop: return ("OpCodeHandler_ReservedNop", true);
			case OpCodeHandlerKindEnum.Enum.Ev_Iz_3: return ("OpCodeHandler_Ev_Iz", true);
			case OpCodeHandlerKindEnum.Enum.Ev_Iz_4: return ("OpCodeHandler_Ev_Iz", true);
			case OpCodeHandlerKindEnum.Enum.Ev_Ib_3: return ("OpCodeHandler_Ev_Ib", true);
			case OpCodeHandlerKindEnum.Enum.Ev_Ib_4: return ("OpCodeHandler_Ev_Ib", true);
			case OpCodeHandlerKindEnum.Enum.Ev_Ib2_3: return ("OpCodeHandler_Ev_Ib2", true);
			case OpCodeHandlerKindEnum.Enum.Ev_Ib2_4: return ("OpCodeHandler_Ev_Ib2", true);
			case OpCodeHandlerKindEnum.Enum.Ev1: return ("OpCodeHandler_Ev_1", true);
			case OpCodeHandlerKindEnum.Enum.Ev_CL: return ("OpCodeHandler_Ev_CL", true);
			case OpCodeHandlerKindEnum.Enum.Ev_3a: return ("OpCodeHandler_Ev", true);
			case OpCodeHandlerKindEnum.Enum.Ev_3b: return ("OpCodeHandler_Ev", true);
			case OpCodeHandlerKindEnum.Enum.Ev_4: return ("OpCodeHandler_Ev", true);
			case OpCodeHandlerKindEnum.Enum.Rv: return ("OpCodeHandler_Rv", true);
			case OpCodeHandlerKindEnum.Enum.Rv_32_64: return ("OpCodeHandler_Rv_32_64", true);
			case OpCodeHandlerKindEnum.Enum.Ev_REXW: return ("OpCodeHandler_Ev_REXW", true);
			case OpCodeHandlerKindEnum.Enum.Evj: return ("OpCodeHandler_Evj", true);
			case OpCodeHandlerKindEnum.Enum.Ep: return ("OpCodeHandler_Ep", true);
			case OpCodeHandlerKindEnum.Enum.Evw: return ("OpCodeHandler_Evw", true);
			case OpCodeHandlerKindEnum.Enum.Ew: return ("OpCodeHandler_Ew", true);
			case OpCodeHandlerKindEnum.Enum.Ms: return ("OpCodeHandler_Ms", true);
			case OpCodeHandlerKindEnum.Enum.Gv_Ev_3a: return ("OpCodeHandler_Gv_Ev", true);
			case OpCodeHandlerKindEnum.Enum.Gv_Ev_3b: return ("OpCodeHandler_Gv_Ev", true);
			case OpCodeHandlerKindEnum.Enum.Gv_M_as: return ("OpCodeHandler_Gv_M_as", true);
			case OpCodeHandlerKindEnum.Enum.Gdq_Ev: return ("OpCodeHandler_Gdq_Ev", true);
			case OpCodeHandlerKindEnum.Enum.Gv_Ev3: return ("OpCodeHandler_Gv_Ev3", true);
			case OpCodeHandlerKindEnum.Enum.Gv_Ev2: return ("OpCodeHandler_Gv_Ev2", true);
			case OpCodeHandlerKindEnum.Enum.R_C_3a: return ("OpCodeHandler_R_C", true);
			case OpCodeHandlerKindEnum.Enum.R_C_3b: return ("OpCodeHandler_R_C", true);
			case OpCodeHandlerKindEnum.Enum.C_R_3a: return ("OpCodeHandler_C_R", true);
			case OpCodeHandlerKindEnum.Enum.C_R_3b: return ("OpCodeHandler_C_R", true);
			case OpCodeHandlerKindEnum.Enum.Jb: return ("OpCodeHandler_Jb", false);
			case OpCodeHandlerKindEnum.Enum.Jx: return ("OpCodeHandler_Jx", false);
			case OpCodeHandlerKindEnum.Enum.Jz: return ("OpCodeHandler_Jz", false);
			case OpCodeHandlerKindEnum.Enum.Jb2: return ("OpCodeHandler_Jb2", false);
			case OpCodeHandlerKindEnum.Enum.Jdisp: return ("OpCodeHandler_Jdisp", false);
			case OpCodeHandlerKindEnum.Enum.PushOpSizeReg_4a: return ("OpCodeHandler_PushOpSizeReg", false);
			case OpCodeHandlerKindEnum.Enum.PushOpSizeReg_4b: return ("OpCodeHandler_PushOpSizeReg", false);
			case OpCodeHandlerKindEnum.Enum.PushEv: return ("OpCodeHandler_PushEv", true);
			case OpCodeHandlerKindEnum.Enum.Ev_Gv_3a: return ("OpCodeHandler_Ev_Gv", true);
			case OpCodeHandlerKindEnum.Enum.Ev_Gv_3b: return ("OpCodeHandler_Ev_Gv", true);
			case OpCodeHandlerKindEnum.Enum.Ev_Gv_4: return ("OpCodeHandler_Ev_Gv", true);
			case OpCodeHandlerKindEnum.Enum.Ev_Gv_32_64: return ("OpCodeHandler_Ev_Gv_32_64", true);
			case OpCodeHandlerKindEnum.Enum.Ev_Gv_Ib: return ("OpCodeHandler_Ev_Gv_Ib", true);
			case OpCodeHandlerKindEnum.Enum.Ev_Gv_CL: return ("OpCodeHandler_Ev_Gv_CL", true);
			case OpCodeHandlerKindEnum.Enum.Gv_Mp_2: return ("OpCodeHandler_Gv_Mp", true);
			case OpCodeHandlerKindEnum.Enum.Gv_Mp_3: return ("OpCodeHandler_Gv_Mp", true);
			case OpCodeHandlerKindEnum.Enum.Gv_Eb: return ("OpCodeHandler_Gv_Eb", true);
			case OpCodeHandlerKindEnum.Enum.Gv_Ew: return ("OpCodeHandler_Gv_Ew", true);
			case OpCodeHandlerKindEnum.Enum.PushSimple2: return ("OpCodeHandler_PushSimple2", false);
			case OpCodeHandlerKindEnum.Enum.Simple2_3a: return ("OpCodeHandler_Simple2", false);
			case OpCodeHandlerKindEnum.Enum.Simple2_3b: return ("OpCodeHandler_Simple2", false);
			case OpCodeHandlerKindEnum.Enum.Simple2Iw: return ("OpCodeHandler_Simple2Iw", false);
			case OpCodeHandlerKindEnum.Enum.Simple3: return ("OpCodeHandler_Simple3", false);
			case OpCodeHandlerKindEnum.Enum.Simple5: return ("OpCodeHandler_Simple5", false);
			case OpCodeHandlerKindEnum.Enum.Simple5_ModRM_as: return ("OpCodeHandler_Simple5_ModRM_as", true);
			case OpCodeHandlerKindEnum.Enum.Simple4: return ("OpCodeHandler_Simple4", false);
			case OpCodeHandlerKindEnum.Enum.PushSimpleReg: return ("OpCodeHandler_PushSimpleReg", false);
			case OpCodeHandlerKindEnum.Enum.SimpleReg: return ("OpCodeHandler_SimpleReg", false);
			case OpCodeHandlerKindEnum.Enum.Xchg_Reg_rAX: return ("OpCodeHandler_Xchg_Reg_rAX", false);
			case OpCodeHandlerKindEnum.Enum.Reg_Iz: return ("OpCodeHandler_Reg_Iz", false);
			case OpCodeHandlerKindEnum.Enum.RegIb3: return ("OpCodeHandler_RegIb3", false);
			case OpCodeHandlerKindEnum.Enum.RegIz2: return ("OpCodeHandler_RegIz2", false);
			case OpCodeHandlerKindEnum.Enum.PushIb2: return ("OpCodeHandler_PushIb2", false);
			case OpCodeHandlerKindEnum.Enum.PushIz: return ("OpCodeHandler_PushIz", false);
			case OpCodeHandlerKindEnum.Enum.Gv_Ma: return ("OpCodeHandler_Gv_Ma", true);
			case OpCodeHandlerKindEnum.Enum.RvMw_Gw: return ("OpCodeHandler_RvMw_Gw", true);
			case OpCodeHandlerKindEnum.Enum.Gv_Ev_Ib: return ("OpCodeHandler_Gv_Ev_Ib", true);
			case OpCodeHandlerKindEnum.Enum.Gv_Ev_Ib_REX: return ("OpCodeHandler_Gv_Ev_Ib_REX", true);
			case OpCodeHandlerKindEnum.Enum.Gv_Ev_32_64: return ("OpCodeHandler_Gv_Ev_32_64", true);
			case OpCodeHandlerKindEnum.Enum.Gv_Ev_Iz: return ("OpCodeHandler_Gv_Ev_Iz", true);
			case OpCodeHandlerKindEnum.Enum.Yb_Reg: return ("OpCodeHandler_Yb_Reg", false);
			case OpCodeHandlerKindEnum.Enum.Yv_Reg: return ("OpCodeHandler_Yv_Reg", false);
			case OpCodeHandlerKindEnum.Enum.Yv_Reg2: return ("OpCodeHandler_Yv_Reg2", false);
			case OpCodeHandlerKindEnum.Enum.Reg_Xb: return ("OpCodeHandler_Reg_Xb", false);
			case OpCodeHandlerKindEnum.Enum.Reg_Xv: return ("OpCodeHandler_Reg_Xv", false);
			case OpCodeHandlerKindEnum.Enum.Reg_Xv2: return ("OpCodeHandler_Reg_Xv2", false);
			case OpCodeHandlerKindEnum.Enum.Reg_Yb: return ("OpCodeHandler_Reg_Yb", false);
			case OpCodeHandlerKindEnum.Enum.Reg_Yv: return ("OpCodeHandler_Reg_Yv", false);
			case OpCodeHandlerKindEnum.Enum.Yb_Xb: return ("OpCodeHandler_Yb_Xb", false);
			case OpCodeHandlerKindEnum.Enum.Yv_Xv: return ("OpCodeHandler_Yv_Xv", false);
			case OpCodeHandlerKindEnum.Enum.Xb_Yb: return ("OpCodeHandler_Xb_Yb", false);
			case OpCodeHandlerKindEnum.Enum.Xv_Yv: return ("OpCodeHandler_Xv_Yv", false);
			case OpCodeHandlerKindEnum.Enum.Ev_Sw: return ("OpCodeHandler_Ev_Sw", true);
			case OpCodeHandlerKindEnum.Enum.Gv_M: return ("OpCodeHandler_Gv_M", true);
			case OpCodeHandlerKindEnum.Enum.Sw_Ev: return ("OpCodeHandler_Sw_Ev", true);
			case OpCodeHandlerKindEnum.Enum.Ap: return ("OpCodeHandler_Ap", false);
			case OpCodeHandlerKindEnum.Enum.Reg_Ob: return ("OpCodeHandler_Reg_Ob", false);
			case OpCodeHandlerKindEnum.Enum.Ob_Reg: return ("OpCodeHandler_Ob_Reg", false);
			case OpCodeHandlerKindEnum.Enum.Reg_Ov: return ("OpCodeHandler_Reg_Ov", false);
			case OpCodeHandlerKindEnum.Enum.Ov_Reg: return ("OpCodeHandler_Ov_Reg", false);
			case OpCodeHandlerKindEnum.Enum.BranchIw: return ("OpCodeHandler_BranchIw", false);
			case OpCodeHandlerKindEnum.Enum.BranchSimple: return ("OpCodeHandler_BranchSimple", false);
			case OpCodeHandlerKindEnum.Enum.Iw_Ib: return ("OpCodeHandler_Iw_Ib", false);
			case OpCodeHandlerKindEnum.Enum.Reg_Ib2: return ("OpCodeHandler_Reg_Ib2", false);
			case OpCodeHandlerKindEnum.Enum.IbReg2: return ("OpCodeHandler_IbReg2", false);
			case OpCodeHandlerKindEnum.Enum.eAX_DX: return ("OpCodeHandler_eAX_DX", false);
			case OpCodeHandlerKindEnum.Enum.DX_eAX: return ("OpCodeHandler_DX_eAX", false);
			case OpCodeHandlerKindEnum.Enum.Eb_Ib_1: return ("OpCodeHandler_Eb_Ib", true);
			case OpCodeHandlerKindEnum.Enum.Eb_Ib_2: return ("OpCodeHandler_Eb_Ib", true);
			case OpCodeHandlerKindEnum.Enum.Eb1: return ("OpCodeHandler_Eb_1", true);
			case OpCodeHandlerKindEnum.Enum.Eb_CL: return ("OpCodeHandler_Eb_CL", true);
			case OpCodeHandlerKindEnum.Enum.Eb_1: return ("OpCodeHandler_Eb", true);
			case OpCodeHandlerKindEnum.Enum.Eb_2: return ("OpCodeHandler_Eb", true);
			case OpCodeHandlerKindEnum.Enum.Eb_Gb_1: return ("OpCodeHandler_Eb_Gb", true);
			case OpCodeHandlerKindEnum.Enum.Eb_Gb_2: return ("OpCodeHandler_Eb_Gb", true);
			case OpCodeHandlerKindEnum.Enum.Gb_Eb: return ("OpCodeHandler_Gb_Eb", true);
			case OpCodeHandlerKindEnum.Enum.M_1: return ("OpCodeHandler_M", true);
			case OpCodeHandlerKindEnum.Enum.M_2: return ("OpCodeHandler_M", true);
			case OpCodeHandlerKindEnum.Enum.M_REXW_2: return ("OpCodeHandler_M_REXW", true);
			case OpCodeHandlerKindEnum.Enum.M_REXW_4: return ("OpCodeHandler_M_REXW", true);
			case OpCodeHandlerKindEnum.Enum.MemBx: return ("OpCodeHandler_MemBx", false);
			case OpCodeHandlerKindEnum.Enum.VW_2: return ("OpCodeHandler_VW", true);
			case OpCodeHandlerKindEnum.Enum.VW_3: return ("OpCodeHandler_VW", true);
			case OpCodeHandlerKindEnum.Enum.WV: return ("OpCodeHandler_WV", true);
			case OpCodeHandlerKindEnum.Enum.rDI_VX_RX: return ("OpCodeHandler_rDI_VX_RX", true);
			case OpCodeHandlerKindEnum.Enum.rDI_P_N: return ("OpCodeHandler_rDI_P_N", true);
			case OpCodeHandlerKindEnum.Enum.VM: return ("OpCodeHandler_VM", true);
			case OpCodeHandlerKindEnum.Enum.MV: return ("OpCodeHandler_MV", true);
			case OpCodeHandlerKindEnum.Enum.VQ: return ("OpCodeHandler_VQ", true);
			case OpCodeHandlerKindEnum.Enum.P_Q: return ("OpCodeHandler_P_Q", true);
			case OpCodeHandlerKindEnum.Enum.Q_P: return ("OpCodeHandler_Q_P", true);
			case OpCodeHandlerKindEnum.Enum.MP: return ("OpCodeHandler_MP", true);
			case OpCodeHandlerKindEnum.Enum.P_Q_Ib: return ("OpCodeHandler_P_Q_Ib", true);
			case OpCodeHandlerKindEnum.Enum.P_W: return ("OpCodeHandler_P_W", true);
			case OpCodeHandlerKindEnum.Enum.P_R: return ("OpCodeHandler_P_R", true);
			case OpCodeHandlerKindEnum.Enum.P_Ev: return ("OpCodeHandler_P_Ev", true);
			case OpCodeHandlerKindEnum.Enum.P_Ev_Ib: return ("OpCodeHandler_P_Ev_Ib", true);
			case OpCodeHandlerKindEnum.Enum.Ev_P: return ("OpCodeHandler_Ev_P", true);
			case OpCodeHandlerKindEnum.Enum.Gv_W: return ("OpCodeHandler_Gv_W", true);
			case OpCodeHandlerKindEnum.Enum.V_Ev: return ("OpCodeHandler_V_Ev", true);
			case OpCodeHandlerKindEnum.Enum.VWIb_2: return ("OpCodeHandler_VWIb", true);
			case OpCodeHandlerKindEnum.Enum.VWIb_3: return ("OpCodeHandler_VWIb", true);
			case OpCodeHandlerKindEnum.Enum.VRIbIb: return ("OpCodeHandler_VRIbIb", true);
			case OpCodeHandlerKindEnum.Enum.RIbIb: return ("OpCodeHandler_RIbIb", true);
			case OpCodeHandlerKindEnum.Enum.RIb: return ("OpCodeHandler_RIb", true);
			case OpCodeHandlerKindEnum.Enum.Ed_V_Ib: return ("OpCodeHandler_Ed_V_Ib", true);
			case OpCodeHandlerKindEnum.Enum.VX_Ev: return ("OpCodeHandler_VX_Ev", true);
			case OpCodeHandlerKindEnum.Enum.Ev_VX: return ("OpCodeHandler_Ev_VX", true);
			case OpCodeHandlerKindEnum.Enum.VX_E_Ib: return ("OpCodeHandler_VX_E_Ib", true);
			case OpCodeHandlerKindEnum.Enum.Gv_RX: return ("OpCodeHandler_Gv_RX", true);
			case OpCodeHandlerKindEnum.Enum.B_MIB: return ("OpCodeHandler_B_MIB", true);
			case OpCodeHandlerKindEnum.Enum.MIB_B: return ("OpCodeHandler_MIB_B", true);
			case OpCodeHandlerKindEnum.Enum.B_BM: return ("OpCodeHandler_B_BM", true);
			case OpCodeHandlerKindEnum.Enum.BM_B: return ("OpCodeHandler_BM_B", true);
			case OpCodeHandlerKindEnum.Enum.B_Ev: return ("OpCodeHandler_B_Ev", true);
			case OpCodeHandlerKindEnum.Enum.Mv_Gv_REXW: return ("OpCodeHandler_Mv_Gv_REXW", true);
			case OpCodeHandlerKindEnum.Enum.Gv_N_Ib_REX: return ("OpCodeHandler_Gv_N_Ib_REX", true);
			case OpCodeHandlerKindEnum.Enum.Gv_N: return ("OpCodeHandler_Gv_N", true);
			case OpCodeHandlerKindEnum.Enum.VN: return ("OpCodeHandler_VN", true);
			case OpCodeHandlerKindEnum.Enum.Gv_Mv: return ("OpCodeHandler_Gv_Mv", true);
			case OpCodeHandlerKindEnum.Enum.Mv_Gv: return ("OpCodeHandler_Mv_Gv", true);
			case OpCodeHandlerKindEnum.Enum.Gv_Eb_REX: return ("OpCodeHandler_Gv_Eb_REX", true);
			case OpCodeHandlerKindEnum.Enum.Gv_Ev_REX: return ("OpCodeHandler_Gv_Ev_REX", true);
			case OpCodeHandlerKindEnum.Enum.Ev_Gv_REX: return ("OpCodeHandler_Ev_Gv_REX", true);
			case OpCodeHandlerKindEnum.Enum.GvM_VX_Ib: return ("OpCodeHandler_GvM_VX_Ib", true);
			case OpCodeHandlerKindEnum.Enum.Wbinvd: return ("OpCodeHandler_Wbinvd", false);

			case OpCodeHandlerKindEnum.Enum.Invalid2:
			case OpCodeHandlerKindEnum.Enum.Dup:
			case OpCodeHandlerKindEnum.Enum.Null:
			case OpCodeHandlerKindEnum.Enum.HandlerReference:
			case OpCodeHandlerKindEnum.Enum.ArrayReference:
			default: throw new InvalidOperationException();
			}
		}

		protected override void WriteFieldsCore(FileWriter writer, object?[] handler) {
			if (!HandlerUtils.IsHandler(handler, out var enumValue))
				throw new InvalidOperationException();
			var typeInfo = GetHandlerTypeInfo(enumValue);
			WriteFirstFields(writer, typeInfo.name, typeInfo.hasModRM);

			switch ((OpCodeHandlerKindEnum.Enum)enumValue.Value) {
			case OpCodeHandlerKindEnum.Enum.Bitness:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "handler1632", handler[1]);
				WriteField(writer, "handler64", handler[2]);
				break;
			case OpCodeHandlerKindEnum.Enum.Bitness_DontReadModRM:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "handler1632", handler[1]);
				WriteField(writer, "handler64", handler[2]);
				break;
			case OpCodeHandlerKindEnum.Enum.Invalid:
			case OpCodeHandlerKindEnum.Enum.Invalid_NoModRM:
				if (handler.Length != 1)
					throw new InvalidOperationException();
				break;
			case OpCodeHandlerKindEnum.Enum.RM:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "reg", handler[1]);
				WriteField(writer, "mem", handler[2]);
				break;
			case OpCodeHandlerKindEnum.Enum.Options3:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "default_handler", handler[1]);
				WriteField(writer, "infos", new object[] { (handler[2], handler[3]), ((object)GetInvalid(), (object)0U) });
				break;
			case OpCodeHandlerKindEnum.Enum.Options5:
				if (handler.Length != 6)
					throw new InvalidOperationException();
				WriteField(writer, "default_handler", handler[1]);
				WriteField(writer, "infos", new object[] { (handler[2], handler[3]), (handler[4], handler[5]) });
				break;
			case OpCodeHandlerKindEnum.Enum.Options_DontReadModRM:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "default_handler", handler[1]);
				WriteField(writer, "opt_handler", handler[2]);
				WriteField(writer, "flags", handler[3]);
				break;
			case OpCodeHandlerKindEnum.Enum.AnotherTable:
				if (handler.Length != 2)
					throw new InvalidOperationException();
				WriteField(writer, "handlers", VerifyArray(0x100, handler[1]));
				break;
			case OpCodeHandlerKindEnum.Enum.Group:
				if (handler.Length != 2)
					throw new InvalidOperationException();
				WriteField(writer, "group_handlers", VerifyArray(8, handler[1]));
				break;
			case OpCodeHandlerKindEnum.Enum.Group8x64:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "table_low", VerifyArray(8, handler[1]));
				WriteField(writer, "table_high", VerifyArray(64, handler[2]));
				break;
			case OpCodeHandlerKindEnum.Enum.Group8x8:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "table_low", VerifyArray(8, handler[1]));
				WriteField(writer, "table_high", VerifyArray(8, handler[2]));
				break;
			case OpCodeHandlerKindEnum.Enum.MandatoryPrefix:
			case OpCodeHandlerKindEnum.Enum.MandatoryPrefix_NoModRM:
				if (handler.Length != 5)
					throw new InvalidOperationException();
				WriteField(writer, "handlers", new object?[] { handler[1], handler[2], handler[3], handler[4] });
				break;
			case OpCodeHandlerKindEnum.Enum.MandatoryPrefix3:
				if (handler.Length != 10)
					throw new InvalidOperationException();
				var flags = (LegacyHandlerFlagsEnum.Enum)((IEnumValue)handler[9]!).Value;
				WriteField(writer, "handlers_reg", new object[] { (handler[1], (object)((flags & LegacyHandlerFlagsEnum.Enum.HandlerReg) == 0)), (handler[3], (object)((flags & LegacyHandlerFlagsEnum.Enum.Handler66Reg) == 0)), (handler[5], (object)((flags & LegacyHandlerFlagsEnum.Enum.HandlerF3Reg) == 0)), (handler[7], (object)((flags & LegacyHandlerFlagsEnum.Enum.HandlerF2Reg) == 0)) });
				WriteField(writer, "handlers_mem", new object[] { (handler[2], (object)((flags & LegacyHandlerFlagsEnum.Enum.HandlerMem) == 0)), (handler[4], (object)((flags & LegacyHandlerFlagsEnum.Enum.Handler66Mem) == 0)), (handler[6], (object)((flags & LegacyHandlerFlagsEnum.Enum.HandlerF3Mem) == 0)), (handler[8], (object)((flags & LegacyHandlerFlagsEnum.Enum.HandlerF2Mem) == 0)) });
				break;
			case OpCodeHandlerKindEnum.Enum.D3NOW:
				if (handler.Length != 1)
					throw new InvalidOperationException();
				break;
			case OpCodeHandlerKindEnum.Enum.EVEX:
				if (handler.Length != 2)
					throw new InvalidOperationException();
				WriteField(writer, "handler_mem", handler[1]);
				break;
			case OpCodeHandlerKindEnum.Enum.VEX2:
				if (handler.Length != 2)
					throw new InvalidOperationException();
				WriteField(writer, "handler_mem", handler[1]);
				break;
			case OpCodeHandlerKindEnum.Enum.VEX3:
				if (handler.Length != 2)
					throw new InvalidOperationException();
				WriteField(writer, "handler_mem", handler[1]);
				break;
			case OpCodeHandlerKindEnum.Enum.XOP:
				if (handler.Length != 2)
					throw new InvalidOperationException();
				WriteField(writer, "handler_reg0", handler[1]);
				break;
			case OpCodeHandlerKindEnum.Enum.Simple:
			case OpCodeHandlerKindEnum.Enum.Simple_ModRM:
				if (handler.Length != 2)
					throw new InvalidOperationException();
				WriteField(writer, "code", handler[1]);
				break;
			case OpCodeHandlerKindEnum.Enum.Reg:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code", handler[1]);
				WriteField(writer, "reg", handler[2]);
				break;
			case OpCodeHandlerKindEnum.Enum.RegIb:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code", handler[1]);
				WriteField(writer, "reg", handler[2]);
				break;
			case OpCodeHandlerKindEnum.Enum.IbReg:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code", handler[1]);
				WriteField(writer, "reg", handler[2]);
				break;
			case OpCodeHandlerKindEnum.Enum.AL_DX:
				if (handler.Length != 2)
					throw new InvalidOperationException();
				WriteField(writer, "code", handler[1]);
				break;
			case OpCodeHandlerKindEnum.Enum.DX_AL:
				if (handler.Length != 2)
					throw new InvalidOperationException();
				WriteField(writer, "code", handler[1]);
				break;
			case OpCodeHandlerKindEnum.Enum.Ib:
				if (handler.Length != 2)
					throw new InvalidOperationException();
				WriteField(writer, "code", handler[1]);
				break;
			case OpCodeHandlerKindEnum.Enum.Ib3:
				if (handler.Length != 2)
					throw new InvalidOperationException();
				WriteField(writer, "code", handler[1]);
				break;
			case OpCodeHandlerKindEnum.Enum.MandatoryPrefix_F3_F2:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "handler_normal", handler[1]);
				WriteField(writer, "handler_f3", handler[2]);
				WriteField(writer, "clear_f3", true);
				WriteField(writer, "handler_f2", handler[3]);
				WriteField(writer, "clear_f2", true);
				break;
			case OpCodeHandlerKindEnum.Enum.LegacyMandatoryPrefix_F3_F2:
				if (handler.Length != 6)
					throw new InvalidOperationException();
				WriteField(writer, "handler_normal", handler[1]);
				WriteField(writer, "handler_f3", handler[2]);
				WriteField(writer, "clear_f3", handler[3]);
				WriteField(writer, "handler_f2", handler[4]);
				WriteField(writer, "clear_f2", handler[5]);
				break;
			case OpCodeHandlerKindEnum.Enum.NIb:
				if (handler.Length != 2)
					throw new InvalidOperationException();
				WriteField(writer, "code", handler[1]);
				break;
			case OpCodeHandlerKindEnum.Enum.ReservedNop:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "reserved_nop_handler", handler[1]);
				WriteField(writer, "other_handler", handler[2]);
				break;
			case OpCodeHandlerKindEnum.Enum.Ev_Iz_3:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				WriteField(writer, "flags", 0U);
				break;
			case OpCodeHandlerKindEnum.Enum.Ev_Iz_4:
				if (handler.Length != 5)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				WriteField(writer, "flags", handler[4]);
				break;
			case OpCodeHandlerKindEnum.Enum.Ev_Ib_3:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				WriteField(writer, "flags", 0U);
				break;
			case OpCodeHandlerKindEnum.Enum.Ev_Ib_4:
				if (handler.Length != 5)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				WriteField(writer, "flags", handler[4]);
				break;
			case OpCodeHandlerKindEnum.Enum.Ev_Ib2_3:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				WriteField(writer, "flags", 0U);
				break;
			case OpCodeHandlerKindEnum.Enum.Ev_Ib2_4:
				if (handler.Length != 5)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				WriteField(writer, "flags", handler[4]);
				break;
			case OpCodeHandlerKindEnum.Enum.Ev1:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKindEnum.Enum.Ev_CL:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKindEnum.Enum.Ev_3a:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				WriteField(writer, "flags", 0U);
				break;
			case OpCodeHandlerKindEnum.Enum.Ev_3b:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", CodeEnum.Instance["INVALID"]);
				WriteField(writer, "flags", 0U);
				break;
			case OpCodeHandlerKindEnum.Enum.Ev_4:
				if (handler.Length != 5)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				WriteField(writer, "flags", handler[4]);
				break;
			case OpCodeHandlerKindEnum.Enum.Rv:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKindEnum.Enum.Rv_32_64:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code32", handler[1]);
				WriteField(writer, "code64", handler[2]);
				break;
			case OpCodeHandlerKindEnum.Enum.Ev_REXW:
				if (handler.Length != 5)
					throw new InvalidOperationException();
				WriteField(writer, "code32", handler[1]);
				WriteField(writer, "code64", handler[2]);
				WriteField(writer, "disallow_reg", (bool)handler[3]! ? 0 : uint.MaxValue);
				WriteField(writer, "disallow_mem", (bool)handler[4]! ? 0 : uint.MaxValue);
				break;
			case OpCodeHandlerKindEnum.Enum.Evj:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKindEnum.Enum.Ep:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKindEnum.Enum.Evw:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKindEnum.Enum.Ew:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKindEnum.Enum.Ms:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKindEnum.Enum.Gv_Ev_3a:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKindEnum.Enum.Gv_Ev_3b:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", CodeEnum.Instance["INVALID"]);
				break;
			case OpCodeHandlerKindEnum.Enum.Gv_M_as:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKindEnum.Enum.Gdq_Ev:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKindEnum.Enum.Gv_Ev3:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKindEnum.Enum.Gv_Ev2:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKindEnum.Enum.R_C_3a:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code32", handler[1]);
				WriteField(writer, "code64", handler[2]);
				WriteField(writer, "base_reg", handler[3]);
				break;
			case OpCodeHandlerKindEnum.Enum.R_C_3b:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code32", handler[1]);
				WriteField(writer, "code64", CodeEnum.Instance["INVALID"]);
				WriteField(writer, "base_reg", handler[2]);
				break;
			case OpCodeHandlerKindEnum.Enum.C_R_3a:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code32", handler[1]);
				WriteField(writer, "code64", handler[2]);
				WriteField(writer, "base_reg", handler[3]);
				break;
			case OpCodeHandlerKindEnum.Enum.C_R_3b:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code32", handler[1]);
				WriteField(writer, "code64", CodeEnum.Instance["INVALID"]);
				WriteField(writer, "base_reg", handler[2]);
				break;
			case OpCodeHandlerKindEnum.Enum.Jb:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKindEnum.Enum.Jx:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKindEnum.Enum.Jz:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKindEnum.Enum.Jb2:
				if (handler.Length != 8)
					throw new InvalidOperationException();
				WriteField(writer, "code16_16", handler[1]);
				WriteField(writer, "code16_32", handler[2]);
				WriteField(writer, "code16_64", handler[3]);
				WriteField(writer, "code32_16", handler[4]);
				WriteField(writer, "code32_32", handler[5]);
				WriteField(writer, "code64_32", handler[6]);
				WriteField(writer, "code64_64", handler[7]);
				break;
			case OpCodeHandlerKindEnum.Enum.Jdisp:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				break;
			case OpCodeHandlerKindEnum.Enum.PushOpSizeReg_4a:
				if (handler.Length != 5)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				WriteField(writer, "reg", handler[4]);
				break;
			case OpCodeHandlerKindEnum.Enum.PushOpSizeReg_4b:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", CodeEnum.Instance["INVALID"]);
				WriteField(writer, "reg", handler[3]);
				break;
			case OpCodeHandlerKindEnum.Enum.PushEv:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKindEnum.Enum.Ev_Gv_3a:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				WriteField(writer, "flags", 0U);
				break;
			case OpCodeHandlerKindEnum.Enum.Ev_Gv_3b:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", CodeEnum.Instance["INVALID"]);
				WriteField(writer, "flags", 0U);
				break;
			case OpCodeHandlerKindEnum.Enum.Ev_Gv_4:
				if (handler.Length != 5)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				WriteField(writer, "flags", handler[4]);
				break;
			case OpCodeHandlerKindEnum.Enum.Ev_Gv_32_64:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code32", handler[1]);
				WriteField(writer, "code64", handler[2]);
				break;
			case OpCodeHandlerKindEnum.Enum.Ev_Gv_Ib:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKindEnum.Enum.Ev_Gv_CL:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKindEnum.Enum.Gv_Mp_2:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", CodeEnum.Instance["INVALID"]);
				break;
			case OpCodeHandlerKindEnum.Enum.Gv_Mp_3:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKindEnum.Enum.Gv_Eb:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKindEnum.Enum.Gv_Ew:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKindEnum.Enum.PushSimple2:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKindEnum.Enum.Simple2_3a:
			case OpCodeHandlerKindEnum.Enum.Simple2_3b:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKindEnum.Enum.Simple2Iw:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKindEnum.Enum.Simple3:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKindEnum.Enum.Simple5:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKindEnum.Enum.Simple5_ModRM_as:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKindEnum.Enum.Simple4:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code32", handler[1]);
				WriteField(writer, "code64", handler[2]);
				break;
			case OpCodeHandlerKindEnum.Enum.PushSimpleReg:
				if (handler.Length != 5)
					throw new InvalidOperationException();
				WriteField(writer, "index", handler[1]);
				WriteField(writer, "code16", handler[2]);
				WriteField(writer, "code32", handler[3]);
				WriteField(writer, "code64", handler[4]);
				break;
			case OpCodeHandlerKindEnum.Enum.SimpleReg:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code", handler[1]);
				WriteField(writer, "index", handler[2]);
				break;
			case OpCodeHandlerKindEnum.Enum.Xchg_Reg_rAX:
				if (handler.Length != 2)
					throw new InvalidOperationException();
				WriteField(writer, "index", handler[1]);
				break;
			case OpCodeHandlerKindEnum.Enum.Reg_Iz:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKindEnum.Enum.RegIb3:
				if (handler.Length != 2)
					throw new InvalidOperationException();
				WriteField(writer, "index", handler[1]);
				break;
			case OpCodeHandlerKindEnum.Enum.RegIz2:
				if (handler.Length != 2)
					throw new InvalidOperationException();
				WriteField(writer, "index", handler[1]);
				break;
			case OpCodeHandlerKindEnum.Enum.PushIb2:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKindEnum.Enum.PushIz:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKindEnum.Enum.Gv_Ma:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				break;
			case OpCodeHandlerKindEnum.Enum.RvMw_Gw:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				break;
			case OpCodeHandlerKindEnum.Enum.Gv_Ev_Ib:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKindEnum.Enum.Gv_Ev_Ib_REX:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKindEnum.Enum.Gv_Ev_32_64:
				if (handler.Length != 5)
					throw new InvalidOperationException();
				WriteField(writer, "code32", handler[1]);
				WriteField(writer, "code64", handler[2]);
				WriteField(writer, "disallow_mem", (bool)handler[4]! ? 0 : uint.MaxValue);
				WriteField(writer, "disallow_reg", (bool)handler[3]! ? 0 : uint.MaxValue);
				break;
			case OpCodeHandlerKindEnum.Enum.Gv_Ev_Iz:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKindEnum.Enum.Yb_Reg:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code", handler[1]);
				WriteField(writer, "reg", handler[2]);
				break;
			case OpCodeHandlerKindEnum.Enum.Yv_Reg:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKindEnum.Enum.Yv_Reg2:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				break;
			case OpCodeHandlerKindEnum.Enum.Reg_Xb:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code", handler[1]);
				WriteField(writer, "reg", handler[2]);
				break;
			case OpCodeHandlerKindEnum.Enum.Reg_Xv:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKindEnum.Enum.Reg_Xv2:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				break;
			case OpCodeHandlerKindEnum.Enum.Reg_Yb:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code", handler[1]);
				WriteField(writer, "reg", handler[2]);
				break;
			case OpCodeHandlerKindEnum.Enum.Reg_Yv:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKindEnum.Enum.Yb_Xb:
				if (handler.Length != 2)
					throw new InvalidOperationException();
				WriteField(writer, "code", handler[1]);
				break;
			case OpCodeHandlerKindEnum.Enum.Yv_Xv:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKindEnum.Enum.Xb_Yb:
				if (handler.Length != 2)
					throw new InvalidOperationException();
				WriteField(writer, "code", handler[1]);
				break;
			case OpCodeHandlerKindEnum.Enum.Xv_Yv:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKindEnum.Enum.Ev_Sw:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKindEnum.Enum.Gv_M:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKindEnum.Enum.Sw_Ev:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKindEnum.Enum.Ap:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				break;
			case OpCodeHandlerKindEnum.Enum.Reg_Ob:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code", handler[1]);
				WriteField(writer, "reg", handler[2]);
				break;
			case OpCodeHandlerKindEnum.Enum.Ob_Reg:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code", handler[1]);
				WriteField(writer, "reg", handler[2]);
				break;
			case OpCodeHandlerKindEnum.Enum.Reg_Ov:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKindEnum.Enum.Ov_Reg:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKindEnum.Enum.BranchIw:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKindEnum.Enum.BranchSimple:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKindEnum.Enum.Iw_Ib:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKindEnum.Enum.Reg_Ib2:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				break;
			case OpCodeHandlerKindEnum.Enum.IbReg2:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				break;
			case OpCodeHandlerKindEnum.Enum.eAX_DX:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				break;
			case OpCodeHandlerKindEnum.Enum.DX_eAX:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				break;
			case OpCodeHandlerKindEnum.Enum.Eb_Ib_1:
				if (handler.Length != 2)
					throw new InvalidOperationException();
				WriteField(writer, "code", handler[1]);
				WriteField(writer, "flags", 0U);
				break;
			case OpCodeHandlerKindEnum.Enum.Eb_Ib_2:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code", handler[1]);
				WriteField(writer, "flags", handler[2]);
				break;
			case OpCodeHandlerKindEnum.Enum.Eb1:
				if (handler.Length != 2)
					throw new InvalidOperationException();
				WriteField(writer, "code", handler[1]);
				break;
			case OpCodeHandlerKindEnum.Enum.Eb_CL:
				if (handler.Length != 2)
					throw new InvalidOperationException();
				WriteField(writer, "code", handler[1]);
				break;
			case OpCodeHandlerKindEnum.Enum.Eb_1:
				if (handler.Length != 2)
					throw new InvalidOperationException();
				WriteField(writer, "code", handler[1]);
				WriteField(writer, "flags", 0U);
				break;
			case OpCodeHandlerKindEnum.Enum.Eb_2:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code", handler[1]);
				WriteField(writer, "flags", handler[2]);
				break;
			case OpCodeHandlerKindEnum.Enum.Eb_Gb_1:
				if (handler.Length != 2)
					throw new InvalidOperationException();
				WriteField(writer, "code", handler[1]);
				WriteField(writer, "flags", 0U);
				break;
			case OpCodeHandlerKindEnum.Enum.Eb_Gb_2:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code", handler[1]);
				WriteField(writer, "flags", handler[2]);
				break;
			case OpCodeHandlerKindEnum.Enum.Gb_Eb:
				if (handler.Length != 2)
					throw new InvalidOperationException();
				WriteField(writer, "code", handler[1]);
				break;
			case OpCodeHandlerKindEnum.Enum.M_1:
				if (handler.Length != 2)
					throw new InvalidOperationException();
				WriteField(writer, "code_w0", handler[1]);
				WriteField(writer, "code_w1", handler[1]);
				break;
			case OpCodeHandlerKindEnum.Enum.M_2:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code_w0", handler[1]);
				WriteField(writer, "code_w1", handler[2]);
				break;
			case OpCodeHandlerKindEnum.Enum.M_REXW_2:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code32", handler[1]);
				WriteField(writer, "code64", handler[2]);
				WriteField(writer, "flags32", 0U);
				WriteField(writer, "flags64", 0U);
				break;
			case OpCodeHandlerKindEnum.Enum.M_REXW_4:
				if (handler.Length != 5)
					throw new InvalidOperationException();
				WriteField(writer, "code32", handler[1]);
				WriteField(writer, "code64", handler[2]);
				WriteField(writer, "flags32", handler[3]);
				WriteField(writer, "flags64", handler[4]);
				break;
			case OpCodeHandlerKindEnum.Enum.MemBx:
				if (handler.Length != 2)
					throw new InvalidOperationException();
				WriteField(writer, "code", handler[1]);
				break;
			case OpCodeHandlerKindEnum.Enum.VW_2:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code_r", handler[2]);
				WriteField(writer, "code_m", handler[2]);
				break;
			case OpCodeHandlerKindEnum.Enum.VW_3:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code_r", handler[2]);
				WriteField(writer, "code_m", handler[3]);
				break;
			case OpCodeHandlerKindEnum.Enum.WV:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code", handler[2]);
				break;
			case OpCodeHandlerKindEnum.Enum.rDI_VX_RX:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code", handler[2]);
				break;
			case OpCodeHandlerKindEnum.Enum.rDI_P_N:
				if (handler.Length != 2)
					throw new InvalidOperationException();
				WriteField(writer, "code", handler[1]);
				break;
			case OpCodeHandlerKindEnum.Enum.VM:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code", handler[2]);
				break;
			case OpCodeHandlerKindEnum.Enum.MV:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code", handler[2]);
				break;
			case OpCodeHandlerKindEnum.Enum.VQ:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code", handler[2]);
				break;
			case OpCodeHandlerKindEnum.Enum.P_Q:
				if (handler.Length != 2)
					throw new InvalidOperationException();
				WriteField(writer, "code", handler[1]);
				break;
			case OpCodeHandlerKindEnum.Enum.Q_P:
				if (handler.Length != 2)
					throw new InvalidOperationException();
				WriteField(writer, "code", handler[1]);
				break;
			case OpCodeHandlerKindEnum.Enum.MP:
				if (handler.Length != 2)
					throw new InvalidOperationException();
				WriteField(writer, "code", handler[1]);
				break;
			case OpCodeHandlerKindEnum.Enum.P_Q_Ib:
				if (handler.Length != 2)
					throw new InvalidOperationException();
				WriteField(writer, "code", handler[1]);
				break;
			case OpCodeHandlerKindEnum.Enum.P_W:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code", handler[2]);
				break;
			case OpCodeHandlerKindEnum.Enum.P_R:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code", handler[2]);
				break;
			case OpCodeHandlerKindEnum.Enum.P_Ev:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code32", handler[1]);
				WriteField(writer, "code64", handler[2]);
				break;
			case OpCodeHandlerKindEnum.Enum.P_Ev_Ib:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code32", handler[1]);
				WriteField(writer, "code64", handler[2]);
				break;
			case OpCodeHandlerKindEnum.Enum.Ev_P:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code32", handler[1]);
				WriteField(writer, "code64", handler[2]);
				break;
			case OpCodeHandlerKindEnum.Enum.Gv_W:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code_w0", handler[2]);
				WriteField(writer, "code_w1", handler[3]);
				break;
			case OpCodeHandlerKindEnum.Enum.V_Ev:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code_w0", handler[2]);
				WriteField(writer, "code_w1", handler[3]);
				break;
			case OpCodeHandlerKindEnum.Enum.VWIb_2:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code_w0", handler[2]);
				WriteField(writer, "code_w1", handler[2]);
				break;
			case OpCodeHandlerKindEnum.Enum.VWIb_3:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code_w0", handler[2]);
				WriteField(writer, "code_w1", handler[3]);
				break;
			case OpCodeHandlerKindEnum.Enum.VRIbIb:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code", handler[2]);
				break;
			case OpCodeHandlerKindEnum.Enum.RIbIb:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code", handler[2]);
				break;
			case OpCodeHandlerKindEnum.Enum.RIb:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code", handler[2]);
				break;
			case OpCodeHandlerKindEnum.Enum.Ed_V_Ib:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKindEnum.Enum.VX_Ev:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code32", handler[1]);
				WriteField(writer, "code64", handler[2]);
				break;
			case OpCodeHandlerKindEnum.Enum.Ev_VX:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code32", handler[1]);
				WriteField(writer, "code64", handler[2]);
				break;
			case OpCodeHandlerKindEnum.Enum.VX_E_Ib:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKindEnum.Enum.Gv_RX:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKindEnum.Enum.B_MIB:
				if (handler.Length != 2)
					throw new InvalidOperationException();
				WriteField(writer, "code", handler[1]);
				break;
			case OpCodeHandlerKindEnum.Enum.MIB_B:
				if (handler.Length != 2)
					throw new InvalidOperationException();
				WriteField(writer, "code", handler[1]);
				break;
			case OpCodeHandlerKindEnum.Enum.B_BM:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code32", handler[1]);
				WriteField(writer, "code64", handler[2]);
				break;
			case OpCodeHandlerKindEnum.Enum.BM_B:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code32", handler[1]);
				WriteField(writer, "code64", handler[2]);
				break;
			case OpCodeHandlerKindEnum.Enum.B_Ev:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code32", handler[1]);
				WriteField(writer, "code64", handler[2]);
				break;
			case OpCodeHandlerKindEnum.Enum.Mv_Gv_REXW:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code32", handler[1]);
				WriteField(writer, "code64", handler[2]);
				break;
			case OpCodeHandlerKindEnum.Enum.Gv_N_Ib_REX:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code32", handler[1]);
				WriteField(writer, "code64", handler[2]);
				break;
			case OpCodeHandlerKindEnum.Enum.Gv_N:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code32", handler[1]);
				WriteField(writer, "code64", handler[2]);
				break;
			case OpCodeHandlerKindEnum.Enum.VN:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code", handler[2]);
				break;
			case OpCodeHandlerKindEnum.Enum.Gv_Mv:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKindEnum.Enum.Mv_Gv:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKindEnum.Enum.Gv_Eb_REX:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code32", handler[1]);
				WriteField(writer, "code64", handler[2]);
				break;
			case OpCodeHandlerKindEnum.Enum.Gv_Ev_REX:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code32", handler[1]);
				WriteField(writer, "code64", handler[2]);
				break;
			case OpCodeHandlerKindEnum.Enum.Ev_Gv_REX:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code32", handler[1]);
				WriteField(writer, "code64", handler[2]);
				break;
			case OpCodeHandlerKindEnum.Enum.GvM_VX_Ib:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKindEnum.Enum.Wbinvd:
				break;
			case OpCodeHandlerKindEnum.Enum.ST_STi:
				if (handler.Length != 2)
					throw new InvalidOperationException();
				WriteField(writer, "code", handler[1]);
				break;
			case OpCodeHandlerKindEnum.Enum.STi_ST:
				if (handler.Length != 2)
					throw new InvalidOperationException();
				WriteField(writer, "code", handler[1]);
				break;
			case OpCodeHandlerKindEnum.Enum.STi:
				if (handler.Length != 2)
					throw new InvalidOperationException();
				WriteField(writer, "code", handler[1]);
				break;
			case OpCodeHandlerKindEnum.Enum.Mf_1:
				if (handler.Length != 2)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[1]);
				break;
			case OpCodeHandlerKindEnum.Enum.Mf_2a:
			case OpCodeHandlerKindEnum.Enum.Mf_2b:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				break;
			case OpCodeHandlerKindEnum.Enum.Invalid2:
			case OpCodeHandlerKindEnum.Enum.Dup:
			case OpCodeHandlerKindEnum.Enum.Null:
			case OpCodeHandlerKindEnum.Enum.HandlerReference:
			case OpCodeHandlerKindEnum.Enum.ArrayReference:
			default:
				throw new InvalidOperationException();
			}
		}
	}
}
