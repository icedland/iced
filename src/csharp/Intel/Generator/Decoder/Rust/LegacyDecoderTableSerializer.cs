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

			return (OpCodeHandlerKind)handlerType.Value switch {
				OpCodeHandlerKind.Bitness => ("OpCodeHandler_Bitness", false),
				OpCodeHandlerKind.Bitness_DontReadModRM => ("OpCodeHandler_Bitness_DontReadModRM", true),
				OpCodeHandlerKind.Invalid => ("OpCodeHandler_Invalid", true),
				OpCodeHandlerKind.Invalid_NoModRM => ("OpCodeHandler_Invalid", false),
				OpCodeHandlerKind.RM => ("OpCodeHandler_RM", true),
				OpCodeHandlerKind.Options3 => ("OpCodeHandler_Options", false),
				OpCodeHandlerKind.Options5 => ("OpCodeHandler_Options", false),
				OpCodeHandlerKind.Options_DontReadModRM => ("OpCodeHandler_Options_DontReadModRM", true),
				OpCodeHandlerKind.AnotherTable => ("OpCodeHandler_AnotherTable", false),
				OpCodeHandlerKind.Group => ("OpCodeHandler_Group", true),
				OpCodeHandlerKind.Group8x64 => ("OpCodeHandler_Group8x64", true),
				OpCodeHandlerKind.Group8x8 => ("OpCodeHandler_Group8x8", true),
				OpCodeHandlerKind.LegacyMandatoryPrefix_F3_F2 => ("OpCodeHandler_MandatoryPrefix_F3_F2", false),
				OpCodeHandlerKind.D3NOW => ("OpCodeHandler_D3NOW", true),
				OpCodeHandlerKind.EVEX => ("OpCodeHandler_EVEX", true),
				OpCodeHandlerKind.VEX2 => ("OpCodeHandler_VEX2", true),
				OpCodeHandlerKind.VEX3 => ("OpCodeHandler_VEX3", true),
				OpCodeHandlerKind.XOP => ("OpCodeHandler_XOP", true),
				OpCodeHandlerKind.Mf_1 => ("OpCodeHandler_Mf", true),
				OpCodeHandlerKind.Mf_2a => ("OpCodeHandler_Mf", true),
				OpCodeHandlerKind.Mf_2b => ("OpCodeHandler_Mf", true),
				OpCodeHandlerKind.Simple => ("OpCodeHandler_Simple", false),
				OpCodeHandlerKind.Simple_ModRM => ("OpCodeHandler_Simple", true),
				OpCodeHandlerKind.ST_STi => ("OpCodeHandler_ST_STi", true),
				OpCodeHandlerKind.STi => ("OpCodeHandler_STi", true),
				OpCodeHandlerKind.STi_ST => ("OpCodeHandler_STi_ST", true),
				OpCodeHandlerKind.Reg => ("OpCodeHandler_Reg", false),
				OpCodeHandlerKind.RegIb => ("OpCodeHandler_RegIb", false),
				OpCodeHandlerKind.IbReg => ("OpCodeHandler_IbReg", false),
				OpCodeHandlerKind.AL_DX => ("OpCodeHandler_AL_DX", false),
				OpCodeHandlerKind.DX_AL => ("OpCodeHandler_DX_AL", false),
				OpCodeHandlerKind.Ib => ("OpCodeHandler_Ib", false),
				OpCodeHandlerKind.Ib3 => ("OpCodeHandler_Ib3", true),
				OpCodeHandlerKind.MandatoryPrefix => ("OpCodeHandler_MandatoryPrefix", true),
				OpCodeHandlerKind.MandatoryPrefix3 => ("OpCodeHandler_MandatoryPrefix3", true),
				OpCodeHandlerKind.MandatoryPrefix_F3_F2 => ("OpCodeHandler_MandatoryPrefix_F3_F2", false),
				OpCodeHandlerKind.MandatoryPrefix_NoModRM => ("OpCodeHandler_MandatoryPrefix", false),
				OpCodeHandlerKind.NIb => ("OpCodeHandler_NIb", true),
				OpCodeHandlerKind.ReservedNop => ("OpCodeHandler_ReservedNop", true),
				OpCodeHandlerKind.Ev_Iz_3 => ("OpCodeHandler_Ev_Iz", true),
				OpCodeHandlerKind.Ev_Iz_4 => ("OpCodeHandler_Ev_Iz", true),
				OpCodeHandlerKind.Ev_Ib_3 => ("OpCodeHandler_Ev_Ib", true),
				OpCodeHandlerKind.Ev_Ib_4 => ("OpCodeHandler_Ev_Ib", true),
				OpCodeHandlerKind.Ev_Ib2_3 => ("OpCodeHandler_Ev_Ib2", true),
				OpCodeHandlerKind.Ev_Ib2_4 => ("OpCodeHandler_Ev_Ib2", true),
				OpCodeHandlerKind.Ev1 => ("OpCodeHandler_Ev_1", true),
				OpCodeHandlerKind.Ev_CL => ("OpCodeHandler_Ev_CL", true),
				OpCodeHandlerKind.Ev_3a => ("OpCodeHandler_Ev", true),
				OpCodeHandlerKind.Ev_3b => ("OpCodeHandler_Ev", true),
				OpCodeHandlerKind.Ev_4 => ("OpCodeHandler_Ev", true),
				OpCodeHandlerKind.Rv => ("OpCodeHandler_Rv", true),
				OpCodeHandlerKind.Rv_32_64 => ("OpCodeHandler_Rv_32_64", true),
				OpCodeHandlerKind.Ev_REXW => ("OpCodeHandler_Ev_REXW", true),
				OpCodeHandlerKind.Evj => ("OpCodeHandler_Evj", true),
				OpCodeHandlerKind.Ep => ("OpCodeHandler_Ep", true),
				OpCodeHandlerKind.Evw => ("OpCodeHandler_Evw", true),
				OpCodeHandlerKind.Ew => ("OpCodeHandler_Ew", true),
				OpCodeHandlerKind.Ms => ("OpCodeHandler_Ms", true),
				OpCodeHandlerKind.Gv_Ev_3a => ("OpCodeHandler_Gv_Ev", true),
				OpCodeHandlerKind.Gv_Ev_3b => ("OpCodeHandler_Gv_Ev", true),
				OpCodeHandlerKind.Gv_M_as => ("OpCodeHandler_Gv_M_as", true),
				OpCodeHandlerKind.Gdq_Ev => ("OpCodeHandler_Gdq_Ev", true),
				OpCodeHandlerKind.Gv_Ev3 => ("OpCodeHandler_Gv_Ev3", true),
				OpCodeHandlerKind.Gv_Ev2 => ("OpCodeHandler_Gv_Ev2", true),
				OpCodeHandlerKind.R_C_3a => ("OpCodeHandler_R_C", true),
				OpCodeHandlerKind.R_C_3b => ("OpCodeHandler_R_C", true),
				OpCodeHandlerKind.C_R_3a => ("OpCodeHandler_C_R", true),
				OpCodeHandlerKind.C_R_3b => ("OpCodeHandler_C_R", true),
				OpCodeHandlerKind.Jb => ("OpCodeHandler_Jb", false),
				OpCodeHandlerKind.Jx => ("OpCodeHandler_Jx", false),
				OpCodeHandlerKind.Jz => ("OpCodeHandler_Jz", false),
				OpCodeHandlerKind.Jb2 => ("OpCodeHandler_Jb2", false),
				OpCodeHandlerKind.Jdisp => ("OpCodeHandler_Jdisp", false),
				OpCodeHandlerKind.PushOpSizeReg_4a => ("OpCodeHandler_PushOpSizeReg", false),
				OpCodeHandlerKind.PushOpSizeReg_4b => ("OpCodeHandler_PushOpSizeReg", false),
				OpCodeHandlerKind.PushEv => ("OpCodeHandler_PushEv", true),
				OpCodeHandlerKind.Ev_Gv_3a => ("OpCodeHandler_Ev_Gv", true),
				OpCodeHandlerKind.Ev_Gv_3b => ("OpCodeHandler_Ev_Gv", true),
				OpCodeHandlerKind.Ev_Gv_4 => ("OpCodeHandler_Ev_Gv", true),
				OpCodeHandlerKind.Ev_Gv_32_64 => ("OpCodeHandler_Ev_Gv_32_64", true),
				OpCodeHandlerKind.Ev_Gv_Ib => ("OpCodeHandler_Ev_Gv_Ib", true),
				OpCodeHandlerKind.Ev_Gv_CL => ("OpCodeHandler_Ev_Gv_CL", true),
				OpCodeHandlerKind.Gv_Mp_2 => ("OpCodeHandler_Gv_Mp", true),
				OpCodeHandlerKind.Gv_Mp_3 => ("OpCodeHandler_Gv_Mp", true),
				OpCodeHandlerKind.Gv_Eb => ("OpCodeHandler_Gv_Eb", true),
				OpCodeHandlerKind.Gv_Ew => ("OpCodeHandler_Gv_Ew", true),
				OpCodeHandlerKind.PushSimple2 => ("OpCodeHandler_PushSimple2", false),
				OpCodeHandlerKind.Simple2_3a => ("OpCodeHandler_Simple2", false),
				OpCodeHandlerKind.Simple2_3b => ("OpCodeHandler_Simple2", false),
				OpCodeHandlerKind.Simple2Iw => ("OpCodeHandler_Simple2Iw", false),
				OpCodeHandlerKind.Simple3 => ("OpCodeHandler_Simple3", false),
				OpCodeHandlerKind.Simple5 => ("OpCodeHandler_Simple5", false),
				OpCodeHandlerKind.Simple5_ModRM_as => ("OpCodeHandler_Simple5_ModRM_as", true),
				OpCodeHandlerKind.Simple4 => ("OpCodeHandler_Simple4", false),
				OpCodeHandlerKind.PushSimpleReg => ("OpCodeHandler_PushSimpleReg", false),
				OpCodeHandlerKind.SimpleReg => ("OpCodeHandler_SimpleReg", false),
				OpCodeHandlerKind.Xchg_Reg_rAX => ("OpCodeHandler_Xchg_Reg_rAX", false),
				OpCodeHandlerKind.Reg_Iz => ("OpCodeHandler_Reg_Iz", false),
				OpCodeHandlerKind.RegIb3 => ("OpCodeHandler_RegIb3", false),
				OpCodeHandlerKind.RegIz2 => ("OpCodeHandler_RegIz2", false),
				OpCodeHandlerKind.PushIb2 => ("OpCodeHandler_PushIb2", false),
				OpCodeHandlerKind.PushIz => ("OpCodeHandler_PushIz", false),
				OpCodeHandlerKind.Gv_Ma => ("OpCodeHandler_Gv_Ma", true),
				OpCodeHandlerKind.RvMw_Gw => ("OpCodeHandler_RvMw_Gw", true),
				OpCodeHandlerKind.Gv_Ev_Ib => ("OpCodeHandler_Gv_Ev_Ib", true),
				OpCodeHandlerKind.Gv_Ev_Ib_REX => ("OpCodeHandler_Gv_Ev_Ib_REX", true),
				OpCodeHandlerKind.Gv_Ev_32_64 => ("OpCodeHandler_Gv_Ev_32_64", true),
				OpCodeHandlerKind.Gv_Ev_Iz => ("OpCodeHandler_Gv_Ev_Iz", true),
				OpCodeHandlerKind.Yb_Reg => ("OpCodeHandler_Yb_Reg", false),
				OpCodeHandlerKind.Yv_Reg => ("OpCodeHandler_Yv_Reg", false),
				OpCodeHandlerKind.Yv_Reg2 => ("OpCodeHandler_Yv_Reg2", false),
				OpCodeHandlerKind.Reg_Xb => ("OpCodeHandler_Reg_Xb", false),
				OpCodeHandlerKind.Reg_Xv => ("OpCodeHandler_Reg_Xv", false),
				OpCodeHandlerKind.Reg_Xv2 => ("OpCodeHandler_Reg_Xv2", false),
				OpCodeHandlerKind.Reg_Yb => ("OpCodeHandler_Reg_Yb", false),
				OpCodeHandlerKind.Reg_Yv => ("OpCodeHandler_Reg_Yv", false),
				OpCodeHandlerKind.Yb_Xb => ("OpCodeHandler_Yb_Xb", false),
				OpCodeHandlerKind.Yv_Xv => ("OpCodeHandler_Yv_Xv", false),
				OpCodeHandlerKind.Xb_Yb => ("OpCodeHandler_Xb_Yb", false),
				OpCodeHandlerKind.Xv_Yv => ("OpCodeHandler_Xv_Yv", false),
				OpCodeHandlerKind.Ev_Sw => ("OpCodeHandler_Ev_Sw", true),
				OpCodeHandlerKind.Gv_M => ("OpCodeHandler_Gv_M", true),
				OpCodeHandlerKind.Sw_Ev => ("OpCodeHandler_Sw_Ev", true),
				OpCodeHandlerKind.Ap => ("OpCodeHandler_Ap", false),
				OpCodeHandlerKind.Reg_Ob => ("OpCodeHandler_Reg_Ob", false),
				OpCodeHandlerKind.Ob_Reg => ("OpCodeHandler_Ob_Reg", false),
				OpCodeHandlerKind.Reg_Ov => ("OpCodeHandler_Reg_Ov", false),
				OpCodeHandlerKind.Ov_Reg => ("OpCodeHandler_Ov_Reg", false),
				OpCodeHandlerKind.BranchIw => ("OpCodeHandler_BranchIw", false),
				OpCodeHandlerKind.BranchSimple => ("OpCodeHandler_BranchSimple", false),
				OpCodeHandlerKind.Iw_Ib => ("OpCodeHandler_Iw_Ib", false),
				OpCodeHandlerKind.Reg_Ib2 => ("OpCodeHandler_Reg_Ib2", false),
				OpCodeHandlerKind.IbReg2 => ("OpCodeHandler_IbReg2", false),
				OpCodeHandlerKind.eAX_DX => ("OpCodeHandler_eAX_DX", false),
				OpCodeHandlerKind.DX_eAX => ("OpCodeHandler_DX_eAX", false),
				OpCodeHandlerKind.Eb_Ib_1 => ("OpCodeHandler_Eb_Ib", true),
				OpCodeHandlerKind.Eb_Ib_2 => ("OpCodeHandler_Eb_Ib", true),
				OpCodeHandlerKind.Eb1 => ("OpCodeHandler_Eb_1", true),
				OpCodeHandlerKind.Eb_CL => ("OpCodeHandler_Eb_CL", true),
				OpCodeHandlerKind.Eb_1 => ("OpCodeHandler_Eb", true),
				OpCodeHandlerKind.Eb_2 => ("OpCodeHandler_Eb", true),
				OpCodeHandlerKind.Eb_Gb_1 => ("OpCodeHandler_Eb_Gb", true),
				OpCodeHandlerKind.Eb_Gb_2 => ("OpCodeHandler_Eb_Gb", true),
				OpCodeHandlerKind.Gb_Eb => ("OpCodeHandler_Gb_Eb", true),
				OpCodeHandlerKind.M_1 => ("OpCodeHandler_M", true),
				OpCodeHandlerKind.M_2 => ("OpCodeHandler_M", true),
				OpCodeHandlerKind.M_REXW_2 => ("OpCodeHandler_M_REXW", true),
				OpCodeHandlerKind.M_REXW_4 => ("OpCodeHandler_M_REXW", true),
				OpCodeHandlerKind.MemBx => ("OpCodeHandler_MemBx", false),
				OpCodeHandlerKind.VW_2 => ("OpCodeHandler_VW", true),
				OpCodeHandlerKind.VW_3 => ("OpCodeHandler_VW", true),
				OpCodeHandlerKind.WV => ("OpCodeHandler_WV", true),
				OpCodeHandlerKind.rDI_VX_RX => ("OpCodeHandler_rDI_VX_RX", true),
				OpCodeHandlerKind.rDI_P_N => ("OpCodeHandler_rDI_P_N", true),
				OpCodeHandlerKind.VM => ("OpCodeHandler_VM", true),
				OpCodeHandlerKind.MV => ("OpCodeHandler_MV", true),
				OpCodeHandlerKind.VQ => ("OpCodeHandler_VQ", true),
				OpCodeHandlerKind.P_Q => ("OpCodeHandler_P_Q", true),
				OpCodeHandlerKind.Q_P => ("OpCodeHandler_Q_P", true),
				OpCodeHandlerKind.MP => ("OpCodeHandler_MP", true),
				OpCodeHandlerKind.P_Q_Ib => ("OpCodeHandler_P_Q_Ib", true),
				OpCodeHandlerKind.P_W => ("OpCodeHandler_P_W", true),
				OpCodeHandlerKind.P_R => ("OpCodeHandler_P_R", true),
				OpCodeHandlerKind.P_Ev => ("OpCodeHandler_P_Ev", true),
				OpCodeHandlerKind.P_Ev_Ib => ("OpCodeHandler_P_Ev_Ib", true),
				OpCodeHandlerKind.Ev_P => ("OpCodeHandler_Ev_P", true),
				OpCodeHandlerKind.Gv_W => ("OpCodeHandler_Gv_W", true),
				OpCodeHandlerKind.V_Ev => ("OpCodeHandler_V_Ev", true),
				OpCodeHandlerKind.VWIb_2 => ("OpCodeHandler_VWIb", true),
				OpCodeHandlerKind.VWIb_3 => ("OpCodeHandler_VWIb", true),
				OpCodeHandlerKind.VRIbIb => ("OpCodeHandler_VRIbIb", true),
				OpCodeHandlerKind.RIbIb => ("OpCodeHandler_RIbIb", true),
				OpCodeHandlerKind.RIb => ("OpCodeHandler_RIb", true),
				OpCodeHandlerKind.Ed_V_Ib => ("OpCodeHandler_Ed_V_Ib", true),
				OpCodeHandlerKind.VX_Ev => ("OpCodeHandler_VX_Ev", true),
				OpCodeHandlerKind.Ev_VX => ("OpCodeHandler_Ev_VX", true),
				OpCodeHandlerKind.VX_E_Ib => ("OpCodeHandler_VX_E_Ib", true),
				OpCodeHandlerKind.Gv_RX => ("OpCodeHandler_Gv_RX", true),
				OpCodeHandlerKind.B_MIB => ("OpCodeHandler_B_MIB", true),
				OpCodeHandlerKind.MIB_B => ("OpCodeHandler_MIB_B", true),
				OpCodeHandlerKind.B_BM => ("OpCodeHandler_B_BM", true),
				OpCodeHandlerKind.BM_B => ("OpCodeHandler_BM_B", true),
				OpCodeHandlerKind.B_Ev => ("OpCodeHandler_B_Ev", true),
				OpCodeHandlerKind.Mv_Gv_REXW => ("OpCodeHandler_Mv_Gv_REXW", true),
				OpCodeHandlerKind.Gv_N_Ib_REX => ("OpCodeHandler_Gv_N_Ib_REX", true),
				OpCodeHandlerKind.Gv_N => ("OpCodeHandler_Gv_N", true),
				OpCodeHandlerKind.VN => ("OpCodeHandler_VN", true),
				OpCodeHandlerKind.Gv_Mv => ("OpCodeHandler_Gv_Mv", true),
				OpCodeHandlerKind.Mv_Gv => ("OpCodeHandler_Mv_Gv", true),
				OpCodeHandlerKind.Gv_Eb_REX => ("OpCodeHandler_Gv_Eb_REX", true),
				OpCodeHandlerKind.Gv_Ev_REX => ("OpCodeHandler_Gv_Ev_REX", true),
				OpCodeHandlerKind.Ev_Gv_REX => ("OpCodeHandler_Ev_Gv_REX", true),
				OpCodeHandlerKind.GvM_VX_Ib => ("OpCodeHandler_GvM_VX_Ib", true),
				OpCodeHandlerKind.Wbinvd => ("OpCodeHandler_Wbinvd", false),
				_ => throw new InvalidOperationException(),
			};
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
