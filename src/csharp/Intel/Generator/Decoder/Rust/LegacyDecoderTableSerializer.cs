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
using Generator.Enums.Decoder;
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

			switch ((OpCodeHandlerKind)handlerType.Value) {
			case OpCodeHandlerKind.Bitness: return ("OpCodeHandler_Bitness", false);
			case OpCodeHandlerKind.Bitness_DontReadModRM: return ("OpCodeHandler_Bitness_DontReadModRM", true);
			case OpCodeHandlerKind.Invalid: return ("OpCodeHandler_Invalid", true);
			case OpCodeHandlerKind.Invalid_NoModRM: return ("OpCodeHandler_Invalid", false);
			case OpCodeHandlerKind.RM: return ("OpCodeHandler_RM", true);
			case OpCodeHandlerKind.Options3: return ("OpCodeHandler_Options", false);
			case OpCodeHandlerKind.Options5: return ("OpCodeHandler_Options", false);
			case OpCodeHandlerKind.Options_DontReadModRM: return ("OpCodeHandler_Options_DontReadModRM", true);
			case OpCodeHandlerKind.AnotherTable: return ("OpCodeHandler_AnotherTable", false);
			case OpCodeHandlerKind.Group: return ("OpCodeHandler_Group", true);
			case OpCodeHandlerKind.Group8x64: return ("OpCodeHandler_Group8x64", true);
			case OpCodeHandlerKind.Group8x8: return ("OpCodeHandler_Group8x8", true);
			case OpCodeHandlerKind.LegacyMandatoryPrefix_F3_F2: return ("OpCodeHandler_MandatoryPrefix_F3_F2", false);
			case OpCodeHandlerKind.D3NOW: return ("OpCodeHandler_D3NOW", true);
			case OpCodeHandlerKind.EVEX: return ("OpCodeHandler_EVEX", true);
			case OpCodeHandlerKind.VEX2: return ("OpCodeHandler_VEX2", true);
			case OpCodeHandlerKind.VEX3: return ("OpCodeHandler_VEX3", true);
			case OpCodeHandlerKind.XOP: return ("OpCodeHandler_XOP", true);
			case OpCodeHandlerKind.Mf_1: return ("OpCodeHandler_Mf", true);
			case OpCodeHandlerKind.Mf_2a: return ("OpCodeHandler_Mf", true);
			case OpCodeHandlerKind.Mf_2b: return ("OpCodeHandler_Mf", true);
			case OpCodeHandlerKind.Simple: return ("OpCodeHandler_Simple", false);
			case OpCodeHandlerKind.Simple_ModRM: return ("OpCodeHandler_Simple", true);
			case OpCodeHandlerKind.ST_STi: return ("OpCodeHandler_ST_STi", true);
			case OpCodeHandlerKind.STi: return ("OpCodeHandler_STi", true);
			case OpCodeHandlerKind.STi_ST: return ("OpCodeHandler_STi_ST", true);
			case OpCodeHandlerKind.Reg: return ("OpCodeHandler_Reg", false);
			case OpCodeHandlerKind.RegIb: return ("OpCodeHandler_RegIb", false);
			case OpCodeHandlerKind.IbReg: return ("OpCodeHandler_IbReg", false);
			case OpCodeHandlerKind.AL_DX: return ("OpCodeHandler_AL_DX", false);
			case OpCodeHandlerKind.DX_AL: return ("OpCodeHandler_DX_AL", false);
			case OpCodeHandlerKind.Ib: return ("OpCodeHandler_Ib", false);
			case OpCodeHandlerKind.Ib3: return ("OpCodeHandler_Ib3", true);
			case OpCodeHandlerKind.MandatoryPrefix: return ("OpCodeHandler_MandatoryPrefix", true);
			case OpCodeHandlerKind.MandatoryPrefix3: return ("OpCodeHandler_MandatoryPrefix3", true);
			case OpCodeHandlerKind.MandatoryPrefix_F3_F2: return ("OpCodeHandler_MandatoryPrefix_F3_F2", false);
			case OpCodeHandlerKind.MandatoryPrefix_NoModRM: return ("OpCodeHandler_MandatoryPrefix", false);
			case OpCodeHandlerKind.NIb: return ("OpCodeHandler_NIb", true);
			case OpCodeHandlerKind.ReservedNop: return ("OpCodeHandler_ReservedNop", true);
			case OpCodeHandlerKind.Ev_Iz_3: return ("OpCodeHandler_Ev_Iz", true);
			case OpCodeHandlerKind.Ev_Iz_4: return ("OpCodeHandler_Ev_Iz", true);
			case OpCodeHandlerKind.Ev_Ib_3: return ("OpCodeHandler_Ev_Ib", true);
			case OpCodeHandlerKind.Ev_Ib_4: return ("OpCodeHandler_Ev_Ib", true);
			case OpCodeHandlerKind.Ev_Ib2_3: return ("OpCodeHandler_Ev_Ib2", true);
			case OpCodeHandlerKind.Ev_Ib2_4: return ("OpCodeHandler_Ev_Ib2", true);
			case OpCodeHandlerKind.Ev1: return ("OpCodeHandler_Ev_1", true);
			case OpCodeHandlerKind.Ev_CL: return ("OpCodeHandler_Ev_CL", true);
			case OpCodeHandlerKind.Ev_3a: return ("OpCodeHandler_Ev", true);
			case OpCodeHandlerKind.Ev_3b: return ("OpCodeHandler_Ev", true);
			case OpCodeHandlerKind.Ev_4: return ("OpCodeHandler_Ev", true);
			case OpCodeHandlerKind.Rv: return ("OpCodeHandler_Rv", true);
			case OpCodeHandlerKind.Rv_32_64: return ("OpCodeHandler_Rv_32_64", true);
			case OpCodeHandlerKind.Ev_REXW: return ("OpCodeHandler_Ev_REXW", true);
			case OpCodeHandlerKind.Evj: return ("OpCodeHandler_Evj", true);
			case OpCodeHandlerKind.Ep: return ("OpCodeHandler_Ep", true);
			case OpCodeHandlerKind.Evw: return ("OpCodeHandler_Evw", true);
			case OpCodeHandlerKind.Ew: return ("OpCodeHandler_Ew", true);
			case OpCodeHandlerKind.Ms: return ("OpCodeHandler_Ms", true);
			case OpCodeHandlerKind.Gv_Ev_3a: return ("OpCodeHandler_Gv_Ev", true);
			case OpCodeHandlerKind.Gv_Ev_3b: return ("OpCodeHandler_Gv_Ev", true);
			case OpCodeHandlerKind.Gv_M_as: return ("OpCodeHandler_Gv_M_as", true);
			case OpCodeHandlerKind.Gdq_Ev: return ("OpCodeHandler_Gdq_Ev", true);
			case OpCodeHandlerKind.Gv_Ev3: return ("OpCodeHandler_Gv_Ev3", true);
			case OpCodeHandlerKind.Gv_Ev2: return ("OpCodeHandler_Gv_Ev2", true);
			case OpCodeHandlerKind.R_C_3a: return ("OpCodeHandler_R_C", true);
			case OpCodeHandlerKind.R_C_3b: return ("OpCodeHandler_R_C", true);
			case OpCodeHandlerKind.C_R_3a: return ("OpCodeHandler_C_R", true);
			case OpCodeHandlerKind.C_R_3b: return ("OpCodeHandler_C_R", true);
			case OpCodeHandlerKind.Jb: return ("OpCodeHandler_Jb", false);
			case OpCodeHandlerKind.Jx: return ("OpCodeHandler_Jx", false);
			case OpCodeHandlerKind.Jz: return ("OpCodeHandler_Jz", false);
			case OpCodeHandlerKind.Jb2: return ("OpCodeHandler_Jb2", false);
			case OpCodeHandlerKind.Jdisp: return ("OpCodeHandler_Jdisp", false);
			case OpCodeHandlerKind.PushOpSizeReg_4a: return ("OpCodeHandler_PushOpSizeReg", false);
			case OpCodeHandlerKind.PushOpSizeReg_4b: return ("OpCodeHandler_PushOpSizeReg", false);
			case OpCodeHandlerKind.PushEv: return ("OpCodeHandler_PushEv", true);
			case OpCodeHandlerKind.Ev_Gv_3a: return ("OpCodeHandler_Ev_Gv", true);
			case OpCodeHandlerKind.Ev_Gv_3b: return ("OpCodeHandler_Ev_Gv", true);
			case OpCodeHandlerKind.Ev_Gv_4: return ("OpCodeHandler_Ev_Gv", true);
			case OpCodeHandlerKind.Ev_Gv_32_64: return ("OpCodeHandler_Ev_Gv_32_64", true);
			case OpCodeHandlerKind.Ev_Gv_Ib: return ("OpCodeHandler_Ev_Gv_Ib", true);
			case OpCodeHandlerKind.Ev_Gv_CL: return ("OpCodeHandler_Ev_Gv_CL", true);
			case OpCodeHandlerKind.Gv_Mp_2: return ("OpCodeHandler_Gv_Mp", true);
			case OpCodeHandlerKind.Gv_Mp_3: return ("OpCodeHandler_Gv_Mp", true);
			case OpCodeHandlerKind.Gv_Eb: return ("OpCodeHandler_Gv_Eb", true);
			case OpCodeHandlerKind.Gv_Ew: return ("OpCodeHandler_Gv_Ew", true);
			case OpCodeHandlerKind.PushSimple2: return ("OpCodeHandler_PushSimple2", false);
			case OpCodeHandlerKind.Simple2_3a: return ("OpCodeHandler_Simple2", false);
			case OpCodeHandlerKind.Simple2_3b: return ("OpCodeHandler_Simple2", false);
			case OpCodeHandlerKind.Simple2Iw: return ("OpCodeHandler_Simple2Iw", false);
			case OpCodeHandlerKind.Simple3: return ("OpCodeHandler_Simple3", false);
			case OpCodeHandlerKind.Simple5: return ("OpCodeHandler_Simple5", false);
			case OpCodeHandlerKind.Simple5_ModRM_as: return ("OpCodeHandler_Simple5_ModRM_as", true);
			case OpCodeHandlerKind.Simple4: return ("OpCodeHandler_Simple4", false);
			case OpCodeHandlerKind.PushSimpleReg: return ("OpCodeHandler_PushSimpleReg", false);
			case OpCodeHandlerKind.SimpleReg: return ("OpCodeHandler_SimpleReg", false);
			case OpCodeHandlerKind.Xchg_Reg_rAX: return ("OpCodeHandler_Xchg_Reg_rAX", false);
			case OpCodeHandlerKind.Reg_Iz: return ("OpCodeHandler_Reg_Iz", false);
			case OpCodeHandlerKind.RegIb3: return ("OpCodeHandler_RegIb3", false);
			case OpCodeHandlerKind.RegIz2: return ("OpCodeHandler_RegIz2", false);
			case OpCodeHandlerKind.PushIb2: return ("OpCodeHandler_PushIb2", false);
			case OpCodeHandlerKind.PushIz: return ("OpCodeHandler_PushIz", false);
			case OpCodeHandlerKind.Gv_Ma: return ("OpCodeHandler_Gv_Ma", true);
			case OpCodeHandlerKind.RvMw_Gw: return ("OpCodeHandler_RvMw_Gw", true);
			case OpCodeHandlerKind.Gv_Ev_Ib: return ("OpCodeHandler_Gv_Ev_Ib", true);
			case OpCodeHandlerKind.Gv_Ev_Ib_REX: return ("OpCodeHandler_Gv_Ev_Ib_REX", true);
			case OpCodeHandlerKind.Gv_Ev_32_64: return ("OpCodeHandler_Gv_Ev_32_64", true);
			case OpCodeHandlerKind.Gv_Ev_Iz: return ("OpCodeHandler_Gv_Ev_Iz", true);
			case OpCodeHandlerKind.Yb_Reg: return ("OpCodeHandler_Yb_Reg", false);
			case OpCodeHandlerKind.Yv_Reg: return ("OpCodeHandler_Yv_Reg", false);
			case OpCodeHandlerKind.Yv_Reg2: return ("OpCodeHandler_Yv_Reg2", false);
			case OpCodeHandlerKind.Reg_Xb: return ("OpCodeHandler_Reg_Xb", false);
			case OpCodeHandlerKind.Reg_Xv: return ("OpCodeHandler_Reg_Xv", false);
			case OpCodeHandlerKind.Reg_Xv2: return ("OpCodeHandler_Reg_Xv2", false);
			case OpCodeHandlerKind.Reg_Yb: return ("OpCodeHandler_Reg_Yb", false);
			case OpCodeHandlerKind.Reg_Yv: return ("OpCodeHandler_Reg_Yv", false);
			case OpCodeHandlerKind.Yb_Xb: return ("OpCodeHandler_Yb_Xb", false);
			case OpCodeHandlerKind.Yv_Xv: return ("OpCodeHandler_Yv_Xv", false);
			case OpCodeHandlerKind.Xb_Yb: return ("OpCodeHandler_Xb_Yb", false);
			case OpCodeHandlerKind.Xv_Yv: return ("OpCodeHandler_Xv_Yv", false);
			case OpCodeHandlerKind.Ev_Sw: return ("OpCodeHandler_Ev_Sw", true);
			case OpCodeHandlerKind.Gv_M: return ("OpCodeHandler_Gv_M", true);
			case OpCodeHandlerKind.Sw_Ev: return ("OpCodeHandler_Sw_Ev", true);
			case OpCodeHandlerKind.Ap: return ("OpCodeHandler_Ap", false);
			case OpCodeHandlerKind.Reg_Ob: return ("OpCodeHandler_Reg_Ob", false);
			case OpCodeHandlerKind.Ob_Reg: return ("OpCodeHandler_Ob_Reg", false);
			case OpCodeHandlerKind.Reg_Ov: return ("OpCodeHandler_Reg_Ov", false);
			case OpCodeHandlerKind.Ov_Reg: return ("OpCodeHandler_Ov_Reg", false);
			case OpCodeHandlerKind.BranchIw: return ("OpCodeHandler_BranchIw", false);
			case OpCodeHandlerKind.BranchSimple: return ("OpCodeHandler_BranchSimple", false);
			case OpCodeHandlerKind.Iw_Ib: return ("OpCodeHandler_Iw_Ib", false);
			case OpCodeHandlerKind.Reg_Ib2: return ("OpCodeHandler_Reg_Ib2", false);
			case OpCodeHandlerKind.IbReg2: return ("OpCodeHandler_IbReg2", false);
			case OpCodeHandlerKind.eAX_DX: return ("OpCodeHandler_eAX_DX", false);
			case OpCodeHandlerKind.DX_eAX: return ("OpCodeHandler_DX_eAX", false);
			case OpCodeHandlerKind.Eb_Ib_1: return ("OpCodeHandler_Eb_Ib", true);
			case OpCodeHandlerKind.Eb_Ib_2: return ("OpCodeHandler_Eb_Ib", true);
			case OpCodeHandlerKind.Eb1: return ("OpCodeHandler_Eb_1", true);
			case OpCodeHandlerKind.Eb_CL: return ("OpCodeHandler_Eb_CL", true);
			case OpCodeHandlerKind.Eb_1: return ("OpCodeHandler_Eb", true);
			case OpCodeHandlerKind.Eb_2: return ("OpCodeHandler_Eb", true);
			case OpCodeHandlerKind.Eb_Gb_1: return ("OpCodeHandler_Eb_Gb", true);
			case OpCodeHandlerKind.Eb_Gb_2: return ("OpCodeHandler_Eb_Gb", true);
			case OpCodeHandlerKind.Gb_Eb: return ("OpCodeHandler_Gb_Eb", true);
			case OpCodeHandlerKind.M_1: return ("OpCodeHandler_M", true);
			case OpCodeHandlerKind.M_2: return ("OpCodeHandler_M", true);
			case OpCodeHandlerKind.M_REXW_2: return ("OpCodeHandler_M_REXW", true);
			case OpCodeHandlerKind.M_REXW_4: return ("OpCodeHandler_M_REXW", true);
			case OpCodeHandlerKind.MemBx: return ("OpCodeHandler_MemBx", false);
			case OpCodeHandlerKind.VW_2: return ("OpCodeHandler_VW", true);
			case OpCodeHandlerKind.VW_3: return ("OpCodeHandler_VW", true);
			case OpCodeHandlerKind.WV: return ("OpCodeHandler_WV", true);
			case OpCodeHandlerKind.rDI_VX_RX: return ("OpCodeHandler_rDI_VX_RX", true);
			case OpCodeHandlerKind.rDI_P_N: return ("OpCodeHandler_rDI_P_N", true);
			case OpCodeHandlerKind.VM: return ("OpCodeHandler_VM", true);
			case OpCodeHandlerKind.MV: return ("OpCodeHandler_MV", true);
			case OpCodeHandlerKind.VQ: return ("OpCodeHandler_VQ", true);
			case OpCodeHandlerKind.P_Q: return ("OpCodeHandler_P_Q", true);
			case OpCodeHandlerKind.Q_P: return ("OpCodeHandler_Q_P", true);
			case OpCodeHandlerKind.MP: return ("OpCodeHandler_MP", true);
			case OpCodeHandlerKind.P_Q_Ib: return ("OpCodeHandler_P_Q_Ib", true);
			case OpCodeHandlerKind.P_W: return ("OpCodeHandler_P_W", true);
			case OpCodeHandlerKind.P_R: return ("OpCodeHandler_P_R", true);
			case OpCodeHandlerKind.P_Ev: return ("OpCodeHandler_P_Ev", true);
			case OpCodeHandlerKind.P_Ev_Ib: return ("OpCodeHandler_P_Ev_Ib", true);
			case OpCodeHandlerKind.Ev_P: return ("OpCodeHandler_Ev_P", true);
			case OpCodeHandlerKind.Gv_W: return ("OpCodeHandler_Gv_W", true);
			case OpCodeHandlerKind.V_Ev: return ("OpCodeHandler_V_Ev", true);
			case OpCodeHandlerKind.VWIb_2: return ("OpCodeHandler_VWIb", true);
			case OpCodeHandlerKind.VWIb_3: return ("OpCodeHandler_VWIb", true);
			case OpCodeHandlerKind.VRIbIb: return ("OpCodeHandler_VRIbIb", true);
			case OpCodeHandlerKind.RIbIb: return ("OpCodeHandler_RIbIb", true);
			case OpCodeHandlerKind.RIb: return ("OpCodeHandler_RIb", true);
			case OpCodeHandlerKind.Ed_V_Ib: return ("OpCodeHandler_Ed_V_Ib", true);
			case OpCodeHandlerKind.VX_Ev: return ("OpCodeHandler_VX_Ev", true);
			case OpCodeHandlerKind.Ev_VX: return ("OpCodeHandler_Ev_VX", true);
			case OpCodeHandlerKind.VX_E_Ib: return ("OpCodeHandler_VX_E_Ib", true);
			case OpCodeHandlerKind.Gv_RX: return ("OpCodeHandler_Gv_RX", true);
			case OpCodeHandlerKind.B_MIB: return ("OpCodeHandler_B_MIB", true);
			case OpCodeHandlerKind.MIB_B: return ("OpCodeHandler_MIB_B", true);
			case OpCodeHandlerKind.B_BM: return ("OpCodeHandler_B_BM", true);
			case OpCodeHandlerKind.BM_B: return ("OpCodeHandler_BM_B", true);
			case OpCodeHandlerKind.B_Ev: return ("OpCodeHandler_B_Ev", true);
			case OpCodeHandlerKind.Mv_Gv_REXW: return ("OpCodeHandler_Mv_Gv_REXW", true);
			case OpCodeHandlerKind.Gv_N_Ib_REX: return ("OpCodeHandler_Gv_N_Ib_REX", true);
			case OpCodeHandlerKind.Gv_N: return ("OpCodeHandler_Gv_N", true);
			case OpCodeHandlerKind.VN: return ("OpCodeHandler_VN", true);
			case OpCodeHandlerKind.Gv_Mv: return ("OpCodeHandler_Gv_Mv", true);
			case OpCodeHandlerKind.Mv_Gv: return ("OpCodeHandler_Mv_Gv", true);
			case OpCodeHandlerKind.Gv_Eb_REX: return ("OpCodeHandler_Gv_Eb_REX", true);
			case OpCodeHandlerKind.Gv_Ev_REX: return ("OpCodeHandler_Gv_Ev_REX", true);
			case OpCodeHandlerKind.Ev_Gv_REX: return ("OpCodeHandler_Ev_Gv_REX", true);
			case OpCodeHandlerKind.GvM_VX_Ib: return ("OpCodeHandler_GvM_VX_Ib", true);
			case OpCodeHandlerKind.Wbinvd: return ("OpCodeHandler_Wbinvd", false);

			case OpCodeHandlerKind.Invalid2:
			case OpCodeHandlerKind.Dup:
			case OpCodeHandlerKind.Null:
			case OpCodeHandlerKind.HandlerReference:
			case OpCodeHandlerKind.ArrayReference:
			default: throw new InvalidOperationException();
			}
		}

		protected override void WriteFieldsCore(FileWriter writer, object?[] handler) {
			if (!HandlerUtils.IsHandler(handler, out var enumValue))
				throw new InvalidOperationException();
			var typeInfo = GetHandlerTypeInfo(enumValue);
			WriteFirstFields(writer, typeInfo.name, typeInfo.hasModRM);

			switch ((OpCodeHandlerKind)enumValue.Value) {
			case OpCodeHandlerKind.Bitness:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "handler1632", handler[1]);
				WriteField(writer, "handler64", handler[2]);
				break;
			case OpCodeHandlerKind.Bitness_DontReadModRM:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "handler1632", handler[1]);
				WriteField(writer, "handler64", handler[2]);
				break;
			case OpCodeHandlerKind.Invalid:
			case OpCodeHandlerKind.Invalid_NoModRM:
				if (handler.Length != 1)
					throw new InvalidOperationException();
				break;
			case OpCodeHandlerKind.RM:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "reg", handler[1]);
				WriteField(writer, "mem", handler[2]);
				break;
			case OpCodeHandlerKind.Options3:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "default_handler", handler[1]);
				WriteField(writer, "infos", new object[] { (handler[2], handler[3]), ((object)GetInvalid(), (object)0U) });
				break;
			case OpCodeHandlerKind.Options5:
				if (handler.Length != 6)
					throw new InvalidOperationException();
				WriteField(writer, "default_handler", handler[1]);
				WriteField(writer, "infos", new object[] { (handler[2], handler[3]), (handler[4], handler[5]) });
				break;
			case OpCodeHandlerKind.Options_DontReadModRM:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "default_handler", handler[1]);
				WriteField(writer, "opt_handler", handler[2]);
				WriteField(writer, "flags", handler[3]);
				break;
			case OpCodeHandlerKind.AnotherTable:
				if (handler.Length != 2)
					throw new InvalidOperationException();
				WriteField(writer, "handlers", VerifyArray(0x100, handler[1]));
				break;
			case OpCodeHandlerKind.Group:
				if (handler.Length != 2)
					throw new InvalidOperationException();
				WriteField(writer, "group_handlers", VerifyArray(8, handler[1]));
				break;
			case OpCodeHandlerKind.Group8x64:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "table_low", VerifyArray(8, handler[1]));
				WriteField(writer, "table_high", VerifyArray(64, handler[2]));
				break;
			case OpCodeHandlerKind.Group8x8:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "table_low", VerifyArray(8, handler[1]));
				WriteField(writer, "table_high", VerifyArray(8, handler[2]));
				break;
			case OpCodeHandlerKind.MandatoryPrefix:
			case OpCodeHandlerKind.MandatoryPrefix_NoModRM:
				if (handler.Length != 5)
					throw new InvalidOperationException();
				WriteField(writer, "handlers", new object?[] { handler[1], handler[2], handler[3], handler[4] });
				break;
			case OpCodeHandlerKind.MandatoryPrefix3:
				if (handler.Length != 10)
					throw new InvalidOperationException();
				var flags = (LegacyHandlerFlags)((IEnumValue)handler[9]!).Value;
				WriteField(writer, "handlers_reg", new object[] { (handler[1], (object)((flags & LegacyHandlerFlags.HandlerReg) == 0)), (handler[3], (object)((flags & LegacyHandlerFlags.Handler66Reg) == 0)), (handler[5], (object)((flags & LegacyHandlerFlags.HandlerF3Reg) == 0)), (handler[7], (object)((flags & LegacyHandlerFlags.HandlerF2Reg) == 0)) });
				WriteField(writer, "handlers_mem", new object[] { (handler[2], (object)((flags & LegacyHandlerFlags.HandlerMem) == 0)), (handler[4], (object)((flags & LegacyHandlerFlags.Handler66Mem) == 0)), (handler[6], (object)((flags & LegacyHandlerFlags.HandlerF3Mem) == 0)), (handler[8], (object)((flags & LegacyHandlerFlags.HandlerF2Mem) == 0)) });
				break;
			case OpCodeHandlerKind.D3NOW:
				if (handler.Length != 1)
					throw new InvalidOperationException();
				break;
			case OpCodeHandlerKind.EVEX:
				if (handler.Length != 2)
					throw new InvalidOperationException();
				WriteField(writer, "handler_mem", handler[1]);
				break;
			case OpCodeHandlerKind.VEX2:
				if (handler.Length != 2)
					throw new InvalidOperationException();
				WriteField(writer, "handler_mem", handler[1]);
				break;
			case OpCodeHandlerKind.VEX3:
				if (handler.Length != 2)
					throw new InvalidOperationException();
				WriteField(writer, "handler_mem", handler[1]);
				break;
			case OpCodeHandlerKind.XOP:
				if (handler.Length != 2)
					throw new InvalidOperationException();
				WriteField(writer, "handler_reg0", handler[1]);
				break;
			case OpCodeHandlerKind.Simple:
			case OpCodeHandlerKind.Simple_ModRM:
				if (handler.Length != 2)
					throw new InvalidOperationException();
				WriteField(writer, "code", handler[1]);
				break;
			case OpCodeHandlerKind.Reg:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code", handler[1]);
				WriteField(writer, "reg", handler[2]);
				break;
			case OpCodeHandlerKind.RegIb:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code", handler[1]);
				WriteField(writer, "reg", handler[2]);
				break;
			case OpCodeHandlerKind.IbReg:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code", handler[1]);
				WriteField(writer, "reg", handler[2]);
				break;
			case OpCodeHandlerKind.AL_DX:
				if (handler.Length != 2)
					throw new InvalidOperationException();
				WriteField(writer, "code", handler[1]);
				break;
			case OpCodeHandlerKind.DX_AL:
				if (handler.Length != 2)
					throw new InvalidOperationException();
				WriteField(writer, "code", handler[1]);
				break;
			case OpCodeHandlerKind.Ib:
				if (handler.Length != 2)
					throw new InvalidOperationException();
				WriteField(writer, "code", handler[1]);
				break;
			case OpCodeHandlerKind.Ib3:
				if (handler.Length != 2)
					throw new InvalidOperationException();
				WriteField(writer, "code", handler[1]);
				break;
			case OpCodeHandlerKind.MandatoryPrefix_F3_F2:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "handler_normal", handler[1]);
				WriteField(writer, "handler_f3", handler[2]);
				WriteField(writer, "clear_f3", true);
				WriteField(writer, "handler_f2", handler[3]);
				WriteField(writer, "clear_f2", true);
				break;
			case OpCodeHandlerKind.LegacyMandatoryPrefix_F3_F2:
				if (handler.Length != 6)
					throw new InvalidOperationException();
				WriteField(writer, "handler_normal", handler[1]);
				WriteField(writer, "handler_f3", handler[2]);
				WriteField(writer, "clear_f3", handler[3]);
				WriteField(writer, "handler_f2", handler[4]);
				WriteField(writer, "clear_f2", handler[5]);
				break;
			case OpCodeHandlerKind.NIb:
				if (handler.Length != 2)
					throw new InvalidOperationException();
				WriteField(writer, "code", handler[1]);
				break;
			case OpCodeHandlerKind.ReservedNop:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "reserved_nop_handler", handler[1]);
				WriteField(writer, "other_handler", handler[2]);
				break;
			case OpCodeHandlerKind.Ev_Iz_3:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				WriteField(writer, "flags", 0U);
				break;
			case OpCodeHandlerKind.Ev_Iz_4:
				if (handler.Length != 5)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				WriteField(writer, "flags", handler[4]);
				break;
			case OpCodeHandlerKind.Ev_Ib_3:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				WriteField(writer, "flags", 0U);
				break;
			case OpCodeHandlerKind.Ev_Ib_4:
				if (handler.Length != 5)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				WriteField(writer, "flags", handler[4]);
				break;
			case OpCodeHandlerKind.Ev_Ib2_3:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				WriteField(writer, "flags", 0U);
				break;
			case OpCodeHandlerKind.Ev_Ib2_4:
				if (handler.Length != 5)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				WriteField(writer, "flags", handler[4]);
				break;
			case OpCodeHandlerKind.Ev1:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKind.Ev_CL:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKind.Ev_3a:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				WriteField(writer, "flags", 0U);
				break;
			case OpCodeHandlerKind.Ev_3b:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", CodeEnum.Instance[nameof(Code.INVALID)]);
				WriteField(writer, "flags", 0U);
				break;
			case OpCodeHandlerKind.Ev_4:
				if (handler.Length != 5)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				WriteField(writer, "flags", handler[4]);
				break;
			case OpCodeHandlerKind.Rv:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKind.Rv_32_64:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code32", handler[1]);
				WriteField(writer, "code64", handler[2]);
				break;
			case OpCodeHandlerKind.Ev_REXW:
				if (handler.Length != 5)
					throw new InvalidOperationException();
				WriteField(writer, "code32", handler[1]);
				WriteField(writer, "code64", handler[2]);
				WriteField(writer, "disallow_reg", (bool)handler[3]! ? 0 : uint.MaxValue);
				WriteField(writer, "disallow_mem", (bool)handler[4]! ? 0 : uint.MaxValue);
				break;
			case OpCodeHandlerKind.Evj:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKind.Ep:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKind.Evw:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKind.Ew:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKind.Ms:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKind.Gv_Ev_3a:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKind.Gv_Ev_3b:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", CodeEnum.Instance[nameof(Code.INVALID)]);
				break;
			case OpCodeHandlerKind.Gv_M_as:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKind.Gdq_Ev:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKind.Gv_Ev3:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKind.Gv_Ev2:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKind.R_C_3a:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code32", handler[1]);
				WriteField(writer, "code64", handler[2]);
				WriteField(writer, "base_reg", handler[3]);
				break;
			case OpCodeHandlerKind.R_C_3b:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code32", handler[1]);
				WriteField(writer, "code64", CodeEnum.Instance[nameof(Code.INVALID)]);
				WriteField(writer, "base_reg", handler[2]);
				break;
			case OpCodeHandlerKind.C_R_3a:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code32", handler[1]);
				WriteField(writer, "code64", handler[2]);
				WriteField(writer, "base_reg", handler[3]);
				break;
			case OpCodeHandlerKind.C_R_3b:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code32", handler[1]);
				WriteField(writer, "code64", CodeEnum.Instance[nameof(Code.INVALID)]);
				WriteField(writer, "base_reg", handler[2]);
				break;
			case OpCodeHandlerKind.Jb:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKind.Jx:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKind.Jz:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKind.Jb2:
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
			case OpCodeHandlerKind.Jdisp:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				break;
			case OpCodeHandlerKind.PushOpSizeReg_4a:
				if (handler.Length != 5)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				WriteField(writer, "reg", handler[4]);
				break;
			case OpCodeHandlerKind.PushOpSizeReg_4b:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", CodeEnum.Instance[nameof(Code.INVALID)]);
				WriteField(writer, "reg", handler[3]);
				break;
			case OpCodeHandlerKind.PushEv:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKind.Ev_Gv_3a:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				WriteField(writer, "flags", 0U);
				break;
			case OpCodeHandlerKind.Ev_Gv_3b:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", CodeEnum.Instance[nameof(Code.INVALID)]);
				WriteField(writer, "flags", 0U);
				break;
			case OpCodeHandlerKind.Ev_Gv_4:
				if (handler.Length != 5)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				WriteField(writer, "flags", handler[4]);
				break;
			case OpCodeHandlerKind.Ev_Gv_32_64:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code32", handler[1]);
				WriteField(writer, "code64", handler[2]);
				break;
			case OpCodeHandlerKind.Ev_Gv_Ib:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKind.Ev_Gv_CL:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKind.Gv_Mp_2:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", CodeEnum.Instance[nameof(Code.INVALID)]);
				break;
			case OpCodeHandlerKind.Gv_Mp_3:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKind.Gv_Eb:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKind.Gv_Ew:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKind.PushSimple2:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKind.Simple2_3a:
			case OpCodeHandlerKind.Simple2_3b:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKind.Simple2Iw:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKind.Simple3:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKind.Simple5:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKind.Simple5_ModRM_as:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKind.Simple4:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code32", handler[1]);
				WriteField(writer, "code64", handler[2]);
				break;
			case OpCodeHandlerKind.PushSimpleReg:
				if (handler.Length != 5)
					throw new InvalidOperationException();
				WriteField(writer, "index", handler[1]);
				WriteField(writer, "code16", handler[2]);
				WriteField(writer, "code32", handler[3]);
				WriteField(writer, "code64", handler[4]);
				break;
			case OpCodeHandlerKind.SimpleReg:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code", handler[1]);
				WriteField(writer, "index", handler[2]);
				break;
			case OpCodeHandlerKind.Xchg_Reg_rAX:
				if (handler.Length != 2)
					throw new InvalidOperationException();
				WriteField(writer, "index", handler[1]);
				break;
			case OpCodeHandlerKind.Reg_Iz:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKind.RegIb3:
				if (handler.Length != 2)
					throw new InvalidOperationException();
				WriteField(writer, "index", handler[1]);
				break;
			case OpCodeHandlerKind.RegIz2:
				if (handler.Length != 2)
					throw new InvalidOperationException();
				WriteField(writer, "index", handler[1]);
				break;
			case OpCodeHandlerKind.PushIb2:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKind.PushIz:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKind.Gv_Ma:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				break;
			case OpCodeHandlerKind.RvMw_Gw:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				break;
			case OpCodeHandlerKind.Gv_Ev_Ib:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKind.Gv_Ev_Ib_REX:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKind.Gv_Ev_32_64:
				if (handler.Length != 5)
					throw new InvalidOperationException();
				WriteField(writer, "code32", handler[1]);
				WriteField(writer, "code64", handler[2]);
				WriteField(writer, "disallow_mem", (bool)handler[4]! ? 0 : uint.MaxValue);
				WriteField(writer, "disallow_reg", (bool)handler[3]! ? 0 : uint.MaxValue);
				break;
			case OpCodeHandlerKind.Gv_Ev_Iz:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKind.Yb_Reg:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code", handler[1]);
				WriteField(writer, "reg", handler[2]);
				break;
			case OpCodeHandlerKind.Yv_Reg:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKind.Yv_Reg2:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				break;
			case OpCodeHandlerKind.Reg_Xb:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code", handler[1]);
				WriteField(writer, "reg", handler[2]);
				break;
			case OpCodeHandlerKind.Reg_Xv:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKind.Reg_Xv2:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				break;
			case OpCodeHandlerKind.Reg_Yb:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code", handler[1]);
				WriteField(writer, "reg", handler[2]);
				break;
			case OpCodeHandlerKind.Reg_Yv:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKind.Yb_Xb:
				if (handler.Length != 2)
					throw new InvalidOperationException();
				WriteField(writer, "code", handler[1]);
				break;
			case OpCodeHandlerKind.Yv_Xv:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKind.Xb_Yb:
				if (handler.Length != 2)
					throw new InvalidOperationException();
				WriteField(writer, "code", handler[1]);
				break;
			case OpCodeHandlerKind.Xv_Yv:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKind.Ev_Sw:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKind.Gv_M:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKind.Sw_Ev:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKind.Ap:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				break;
			case OpCodeHandlerKind.Reg_Ob:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code", handler[1]);
				WriteField(writer, "reg", handler[2]);
				break;
			case OpCodeHandlerKind.Ob_Reg:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code", handler[1]);
				WriteField(writer, "reg", handler[2]);
				break;
			case OpCodeHandlerKind.Reg_Ov:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKind.Ov_Reg:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKind.BranchIw:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKind.BranchSimple:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKind.Iw_Ib:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKind.Reg_Ib2:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				break;
			case OpCodeHandlerKind.IbReg2:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				break;
			case OpCodeHandlerKind.eAX_DX:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				break;
			case OpCodeHandlerKind.DX_eAX:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				break;
			case OpCodeHandlerKind.Eb_Ib_1:
				if (handler.Length != 2)
					throw new InvalidOperationException();
				WriteField(writer, "code", handler[1]);
				WriteField(writer, "flags", 0U);
				break;
			case OpCodeHandlerKind.Eb_Ib_2:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code", handler[1]);
				WriteField(writer, "flags", handler[2]);
				break;
			case OpCodeHandlerKind.Eb1:
				if (handler.Length != 2)
					throw new InvalidOperationException();
				WriteField(writer, "code", handler[1]);
				break;
			case OpCodeHandlerKind.Eb_CL:
				if (handler.Length != 2)
					throw new InvalidOperationException();
				WriteField(writer, "code", handler[1]);
				break;
			case OpCodeHandlerKind.Eb_1:
				if (handler.Length != 2)
					throw new InvalidOperationException();
				WriteField(writer, "code", handler[1]);
				WriteField(writer, "flags", 0U);
				break;
			case OpCodeHandlerKind.Eb_2:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code", handler[1]);
				WriteField(writer, "flags", handler[2]);
				break;
			case OpCodeHandlerKind.Eb_Gb_1:
				if (handler.Length != 2)
					throw new InvalidOperationException();
				WriteField(writer, "code", handler[1]);
				WriteField(writer, "flags", 0U);
				break;
			case OpCodeHandlerKind.Eb_Gb_2:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code", handler[1]);
				WriteField(writer, "flags", handler[2]);
				break;
			case OpCodeHandlerKind.Gb_Eb:
				if (handler.Length != 2)
					throw new InvalidOperationException();
				WriteField(writer, "code", handler[1]);
				break;
			case OpCodeHandlerKind.M_1:
				if (handler.Length != 2)
					throw new InvalidOperationException();
				WriteField(writer, "code_w0", handler[1]);
				WriteField(writer, "code_w1", handler[1]);
				break;
			case OpCodeHandlerKind.M_2:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code_w0", handler[1]);
				WriteField(writer, "code_w1", handler[2]);
				break;
			case OpCodeHandlerKind.M_REXW_2:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code32", handler[1]);
				WriteField(writer, "code64", handler[2]);
				WriteField(writer, "flags32", 0U);
				WriteField(writer, "flags64", 0U);
				break;
			case OpCodeHandlerKind.M_REXW_4:
				if (handler.Length != 5)
					throw new InvalidOperationException();
				WriteField(writer, "code32", handler[1]);
				WriteField(writer, "code64", handler[2]);
				WriteField(writer, "flags32", handler[3]);
				WriteField(writer, "flags64", handler[4]);
				break;
			case OpCodeHandlerKind.MemBx:
				if (handler.Length != 2)
					throw new InvalidOperationException();
				WriteField(writer, "code", handler[1]);
				break;
			case OpCodeHandlerKind.VW_2:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code_r", handler[2]);
				WriteField(writer, "code_m", handler[2]);
				break;
			case OpCodeHandlerKind.VW_3:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code_r", handler[2]);
				WriteField(writer, "code_m", handler[3]);
				break;
			case OpCodeHandlerKind.WV:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code", handler[2]);
				break;
			case OpCodeHandlerKind.rDI_VX_RX:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code", handler[2]);
				break;
			case OpCodeHandlerKind.rDI_P_N:
				if (handler.Length != 2)
					throw new InvalidOperationException();
				WriteField(writer, "code", handler[1]);
				break;
			case OpCodeHandlerKind.VM:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code", handler[2]);
				break;
			case OpCodeHandlerKind.MV:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code", handler[2]);
				break;
			case OpCodeHandlerKind.VQ:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code", handler[2]);
				break;
			case OpCodeHandlerKind.P_Q:
				if (handler.Length != 2)
					throw new InvalidOperationException();
				WriteField(writer, "code", handler[1]);
				break;
			case OpCodeHandlerKind.Q_P:
				if (handler.Length != 2)
					throw new InvalidOperationException();
				WriteField(writer, "code", handler[1]);
				break;
			case OpCodeHandlerKind.MP:
				if (handler.Length != 2)
					throw new InvalidOperationException();
				WriteField(writer, "code", handler[1]);
				break;
			case OpCodeHandlerKind.P_Q_Ib:
				if (handler.Length != 2)
					throw new InvalidOperationException();
				WriteField(writer, "code", handler[1]);
				break;
			case OpCodeHandlerKind.P_W:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code", handler[2]);
				break;
			case OpCodeHandlerKind.P_R:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code", handler[2]);
				break;
			case OpCodeHandlerKind.P_Ev:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code32", handler[1]);
				WriteField(writer, "code64", handler[2]);
				break;
			case OpCodeHandlerKind.P_Ev_Ib:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code32", handler[1]);
				WriteField(writer, "code64", handler[2]);
				break;
			case OpCodeHandlerKind.Ev_P:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code32", handler[1]);
				WriteField(writer, "code64", handler[2]);
				break;
			case OpCodeHandlerKind.Gv_W:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code_w0", handler[2]);
				WriteField(writer, "code_w1", handler[3]);
				break;
			case OpCodeHandlerKind.V_Ev:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code_w0", handler[2]);
				WriteField(writer, "code_w1", handler[3]);
				break;
			case OpCodeHandlerKind.VWIb_2:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code_w0", handler[2]);
				WriteField(writer, "code_w1", handler[2]);
				break;
			case OpCodeHandlerKind.VWIb_3:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code_w0", handler[2]);
				WriteField(writer, "code_w1", handler[3]);
				break;
			case OpCodeHandlerKind.VRIbIb:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code", handler[2]);
				break;
			case OpCodeHandlerKind.RIbIb:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code", handler[2]);
				break;
			case OpCodeHandlerKind.RIb:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code", handler[2]);
				break;
			case OpCodeHandlerKind.Ed_V_Ib:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKind.VX_Ev:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code32", handler[1]);
				WriteField(writer, "code64", handler[2]);
				break;
			case OpCodeHandlerKind.Ev_VX:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code32", handler[1]);
				WriteField(writer, "code64", handler[2]);
				break;
			case OpCodeHandlerKind.VX_E_Ib:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKind.Gv_RX:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKind.B_MIB:
				if (handler.Length != 2)
					throw new InvalidOperationException();
				WriteField(writer, "code", handler[1]);
				break;
			case OpCodeHandlerKind.MIB_B:
				if (handler.Length != 2)
					throw new InvalidOperationException();
				WriteField(writer, "code", handler[1]);
				break;
			case OpCodeHandlerKind.B_BM:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code32", handler[1]);
				WriteField(writer, "code64", handler[2]);
				break;
			case OpCodeHandlerKind.BM_B:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code32", handler[1]);
				WriteField(writer, "code64", handler[2]);
				break;
			case OpCodeHandlerKind.B_Ev:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code32", handler[1]);
				WriteField(writer, "code64", handler[2]);
				break;
			case OpCodeHandlerKind.Mv_Gv_REXW:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code32", handler[1]);
				WriteField(writer, "code64", handler[2]);
				break;
			case OpCodeHandlerKind.Gv_N_Ib_REX:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code32", handler[1]);
				WriteField(writer, "code64", handler[2]);
				break;
			case OpCodeHandlerKind.Gv_N:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code32", handler[1]);
				WriteField(writer, "code64", handler[2]);
				break;
			case OpCodeHandlerKind.VN:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code", handler[2]);
				break;
			case OpCodeHandlerKind.Gv_Mv:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKind.Mv_Gv:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKind.Gv_Eb_REX:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code32", handler[1]);
				WriteField(writer, "code64", handler[2]);
				break;
			case OpCodeHandlerKind.Gv_Ev_REX:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code32", handler[1]);
				WriteField(writer, "code64", handler[2]);
				break;
			case OpCodeHandlerKind.Ev_Gv_REX:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code32", handler[1]);
				WriteField(writer, "code64", handler[2]);
				break;
			case OpCodeHandlerKind.GvM_VX_Ib:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case OpCodeHandlerKind.Wbinvd:
				break;
			case OpCodeHandlerKind.ST_STi:
				if (handler.Length != 2)
					throw new InvalidOperationException();
				WriteField(writer, "code", handler[1]);
				break;
			case OpCodeHandlerKind.STi_ST:
				if (handler.Length != 2)
					throw new InvalidOperationException();
				WriteField(writer, "code", handler[1]);
				break;
			case OpCodeHandlerKind.STi:
				if (handler.Length != 2)
					throw new InvalidOperationException();
				WriteField(writer, "code", handler[1]);
				break;
			case OpCodeHandlerKind.Mf_1:
				if (handler.Length != 2)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[1]);
				break;
			case OpCodeHandlerKind.Mf_2a:
			case OpCodeHandlerKind.Mf_2b:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code16", handler[1]);
				WriteField(writer, "code32", handler[2]);
				break;
			case OpCodeHandlerKind.Invalid2:
			case OpCodeHandlerKind.Dup:
			case OpCodeHandlerKind.Null:
			case OpCodeHandlerKind.HandlerReference:
			case OpCodeHandlerKind.ArrayReference:
			default:
				throw new InvalidOperationException();
			}
		}
	}
}
