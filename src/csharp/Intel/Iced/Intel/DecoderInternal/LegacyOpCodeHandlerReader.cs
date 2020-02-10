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

#if DECODER
using System;

namespace Iced.Intel.DecoderInternal {
	sealed class LegacyOpCodeHandlerReader : OpCodeHandlerReader {
		public override int ReadHandlers(ref TableDeserializer deserializer, OpCodeHandler?[] result, int resultIndex) {
			ref var elem = ref result[resultIndex];
			Code code;
			switch (deserializer.ReadOpCodeHandlerKind()) {
			case OpCodeHandlerKind.Bitness:
				elem = new OpCodeHandler_Bitness(deserializer.ReadHandler(), deserializer.ReadHandler());
				return 1;

			case OpCodeHandlerKind.Bitness_DontReadModRM:
				elem = new OpCodeHandler_Bitness_DontReadModRM(deserializer.ReadHandler(), deserializer.ReadHandler());
				return 1;

			case OpCodeHandlerKind.Invalid:
				elem = OpCodeHandler_Invalid.Instance;
				return 1;

			case OpCodeHandlerKind.Invalid_NoModRM:
				elem = OpCodeHandler_Invalid_NoModRM.Instance;
				return 1;

			case OpCodeHandlerKind.Invalid2:
				result[resultIndex] = OpCodeHandler_Invalid.Instance;
				result[resultIndex + 1] = OpCodeHandler_Invalid.Instance;
				return 2;

			case OpCodeHandlerKind.Dup:
				int count = deserializer.ReadInt32();
				var handler = deserializer.ReadHandlerOrNull();
				for (int i = 0; i < count; i++)
					result[resultIndex + i] = handler;
				return count;

			case OpCodeHandlerKind.Null:
				elem = null;
				return 1;

			case OpCodeHandlerKind.HandlerReference:
				elem = deserializer.ReadHandlerReference();
				return 1;

			case OpCodeHandlerKind.ArrayReference:
				throw new InvalidOperationException();

			case OpCodeHandlerKind.RM:
				elem = new OpCodeHandler_RM(deserializer.ReadHandler(), deserializer.ReadHandler());
				return 1;

			case OpCodeHandlerKind.Options3:
				elem = new OpCodeHandler_Options(deserializer.ReadHandler(), deserializer.ReadHandler(), deserializer.ReadDecoderOptions());
				return 1;

			case OpCodeHandlerKind.Options5:
				elem = new OpCodeHandler_Options(deserializer.ReadHandler(), deserializer.ReadHandler(), deserializer.ReadDecoderOptions(), deserializer.ReadHandler(), deserializer.ReadDecoderOptions());
				return 1;

			case OpCodeHandlerKind.Options_DontReadModRM:
				elem = new OpCodeHandler_Options_DontReadModRM(deserializer.ReadHandler(), deserializer.ReadHandler(), deserializer.ReadDecoderOptions());
				return 1;

			case OpCodeHandlerKind.AnotherTable:
				elem = new OpCodeHandler_AnotherTable(deserializer.ReadArrayReference((uint)OpCodeHandlerKind.ArrayReference));
				return 1;

			case OpCodeHandlerKind.Group:
				elem = new OpCodeHandler_Group(deserializer.ReadArrayReference((uint)OpCodeHandlerKind.ArrayReference));
				return 1;

			case OpCodeHandlerKind.Group8x64:
				elem = new OpCodeHandler_Group8x64(deserializer.ReadArrayReference((uint)OpCodeHandlerKind.ArrayReference), deserializer.ReadArrayReference((uint)OpCodeHandlerKind.ArrayReference));
				return 1;

			case OpCodeHandlerKind.Group8x8:
				elem = new OpCodeHandler_Group8x8(deserializer.ReadArrayReference((uint)OpCodeHandlerKind.ArrayReference), deserializer.ReadArrayReference((uint)OpCodeHandlerKind.ArrayReference));
				return 1;

			case OpCodeHandlerKind.MandatoryPrefix:
				elem = new OpCodeHandler_MandatoryPrefix(deserializer.ReadHandler(), deserializer.ReadHandler(), deserializer.ReadHandler(), deserializer.ReadHandler());
				return 1;

			case OpCodeHandlerKind.MandatoryPrefix_F3_F2:
				elem = new OpCodeHandler_MandatoryPrefix_F3_F2(deserializer.ReadHandler(), deserializer.ReadHandler(), true, deserializer.ReadHandler(), true);
				return 1;

			case OpCodeHandlerKind.LegacyMandatoryPrefix_F3_F2:
				elem = new OpCodeHandler_MandatoryPrefix_F3_F2(deserializer.ReadHandler(), deserializer.ReadHandler(), deserializer.ReadBoolean(), deserializer.ReadHandler(), deserializer.ReadBoolean());
				return 1;

			case OpCodeHandlerKind.MandatoryPrefix_NoModRM:
				elem = new OpCodeHandler_MandatoryPrefix_NoModRM(deserializer.ReadHandler(), deserializer.ReadHandler(), deserializer.ReadHandler(), deserializer.ReadHandler());
				return 1;

			case OpCodeHandlerKind.MandatoryPrefix3:
				elem = new OpCodeHandler_MandatoryPrefix3(deserializer.ReadHandler(), deserializer.ReadHandler(), deserializer.ReadHandler(), deserializer.ReadHandler(), deserializer.ReadHandler(), deserializer.ReadHandler(), deserializer.ReadHandler(), deserializer.ReadHandler(), deserializer.ReadLegacyHandlerFlags());
				return 1;

			case OpCodeHandlerKind.D3NOW:
				elem = new OpCodeHandler_D3NOW();
				return 1;

			case OpCodeHandlerKind.EVEX:
				elem = new OpCodeHandler_EVEX(deserializer.ReadHandler());
				return 1;

			case OpCodeHandlerKind.VEX2:
				elem = new OpCodeHandler_VEX2(deserializer.ReadHandler());
				return 1;

			case OpCodeHandlerKind.VEX3:
				elem = new OpCodeHandler_VEX3(deserializer.ReadHandler());
				return 1;

			case OpCodeHandlerKind.XOP:
				elem = new OpCodeHandler_XOP(deserializer.ReadHandler());
				return 1;

			case OpCodeHandlerKind.AL_DX:
				elem = new OpCodeHandler_AL_DX(deserializer.ReadCode());
				return 1;

			case OpCodeHandlerKind.Ap:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Ap(code, code + 1);
				return 1;

			case OpCodeHandlerKind.B_BM:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_B_BM(code, code + 1);
				return 1;

			case OpCodeHandlerKind.B_Ev:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_B_Ev(code, code + 1);
				return 1;

			case OpCodeHandlerKind.B_MIB:
				elem = new OpCodeHandler_B_MIB(deserializer.ReadCode());
				return 1;

			case OpCodeHandlerKind.BM_B:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_BM_B(code, code + 1);
				return 1;

			case OpCodeHandlerKind.BranchIw:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_BranchIw(code, code + 1, code + 2);
				return 1;

			case OpCodeHandlerKind.BranchSimple:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_BranchSimple(code, code + 1, code + 2);
				return 1;

			case OpCodeHandlerKind.C_R_3a:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_C_R(code, code + 1, deserializer.ReadRegister());
				return 1;

			case OpCodeHandlerKind.C_R_3b:
				elem = new OpCodeHandler_C_R(deserializer.ReadCode(), Code.INVALID, deserializer.ReadRegister());
				return 1;

			case OpCodeHandlerKind.DX_AL:
				elem = new OpCodeHandler_DX_AL(deserializer.ReadCode());
				return 1;

			case OpCodeHandlerKind.DX_eAX:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_DX_eAX(code, code + 1);
				return 1;

			case OpCodeHandlerKind.eAX_DX:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_eAX_DX(code, code + 1);
				return 1;

			case OpCodeHandlerKind.Eb_1:
				elem = new OpCodeHandler_Eb(deserializer.ReadCode());
				return 1;

			case OpCodeHandlerKind.Eb_2:
				elem = new OpCodeHandler_Eb(deserializer.ReadCode(), deserializer.ReadHandlerFlags());
				return 1;

			case OpCodeHandlerKind.Eb_CL:
				elem = new OpCodeHandler_Eb_CL(deserializer.ReadCode());
				return 1;

			case OpCodeHandlerKind.Eb_Gb_1:
				elem = new OpCodeHandler_Eb_Gb(deserializer.ReadCode());
				return 1;

			case OpCodeHandlerKind.Eb_Gb_2:
				elem = new OpCodeHandler_Eb_Gb(deserializer.ReadCode(), deserializer.ReadHandlerFlags());
				return 1;

			case OpCodeHandlerKind.Eb_Ib_1:
				elem = new OpCodeHandler_Eb_Ib(deserializer.ReadCode());
				return 1;

			case OpCodeHandlerKind.Eb_Ib_2:
				elem = new OpCodeHandler_Eb_Ib(deserializer.ReadCode(), deserializer.ReadHandlerFlags());
				return 1;

			case OpCodeHandlerKind.Eb1:
				elem = new OpCodeHandler_Eb_1(deserializer.ReadCode());
				return 1;

			case OpCodeHandlerKind.Ed_V_Ib:
				elem = new OpCodeHandler_Ed_V_Ib(deserializer.ReadRegister(), code = deserializer.ReadCode(), code + 1);
				return 1;

			case OpCodeHandlerKind.Ep:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Ep(code, code + 1, code + 2);
				return 1;

			case OpCodeHandlerKind.Ev_3a:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Ev(code, code + 1, code + 2);
				return 1;

			case OpCodeHandlerKind.Ev_3b:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Ev(code, code + 1, Code.INVALID);
				return 1;

			case OpCodeHandlerKind.Ev_4:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Ev(code, code + 1, code + 2, deserializer.ReadHandlerFlags());
				return 1;

			case OpCodeHandlerKind.Ev_CL:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Ev_CL(code, code + 1, code + 2);
				return 1;

			case OpCodeHandlerKind.Ev_Gv_32_64:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Ev_Gv_32_64(code, code + 1);
				return 1;

			case OpCodeHandlerKind.Ev_Gv_3a:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Ev_Gv(code, code + 1, code + 2);
				return 1;

			case OpCodeHandlerKind.Ev_Gv_3b:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Ev_Gv(code, code + 1, Code.INVALID);
				return 1;

			case OpCodeHandlerKind.Ev_Gv_4:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Ev_Gv(code, code + 1, code + 2, deserializer.ReadHandlerFlags());
				return 1;

			case OpCodeHandlerKind.Ev_Gv_CL:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Ev_Gv_CL(code, code + 1, code + 2);
				return 1;

			case OpCodeHandlerKind.Ev_Gv_Ib:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Ev_Gv_Ib(code, code + 1, code + 2);
				return 1;

			case OpCodeHandlerKind.Ev_Gv_REX:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Ev_Gv_REX(code, code + 1);
				return 1;

			case OpCodeHandlerKind.Ev_Ib_3:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Ev_Ib(code, code + 1, code + 2);
				return 1;

			case OpCodeHandlerKind.Ev_Ib_4:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Ev_Ib(code, code + 1, code + 2, deserializer.ReadHandlerFlags());
				return 1;

			case OpCodeHandlerKind.Ev_Ib2_3:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Ev_Ib2(code, code + 1, code + 2);
				return 1;

			case OpCodeHandlerKind.Ev_Ib2_4:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Ev_Ib2(code, code + 1, code + 2, deserializer.ReadHandlerFlags());
				return 1;

			case OpCodeHandlerKind.Ev_Iz_3:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Ev_Iz(code, code + 1, code + 2);
				return 1;

			case OpCodeHandlerKind.Ev_Iz_4:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Ev_Iz(code, code + 1, code + 2, deserializer.ReadHandlerFlags());
				return 1;

			case OpCodeHandlerKind.Ev_P:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Ev_P(code, code + 1);
				return 1;

			case OpCodeHandlerKind.Ev_REXW:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Ev_REXW(code, code + 1, deserializer.ReadBoolean(), deserializer.ReadBoolean());
				return 1;

			case OpCodeHandlerKind.Ev_Sw:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Ev_Sw(code, code + 1, code + 2);
				return 1;

			case OpCodeHandlerKind.Ev_VX:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Ev_VX(code, code + 1);
				return 1;

			case OpCodeHandlerKind.Ev1:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Ev_1(code, code + 1, code + 2);
				return 1;

			case OpCodeHandlerKind.Evj:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Evj(code, code + 1, code + 2);
				return 1;

			case OpCodeHandlerKind.Evw:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Evw(code, code + 1, code + 2);
				return 1;

			case OpCodeHandlerKind.Ew:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Ew(code, code + 1, code + 2);
				return 1;

			case OpCodeHandlerKind.Gb_Eb:
				elem = new OpCodeHandler_Gb_Eb(deserializer.ReadCode());
				return 1;

			case OpCodeHandlerKind.Gdq_Ev:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Gdq_Ev(code, code + 1, code + 2);
				return 1;

			case OpCodeHandlerKind.Gv_Eb:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Gv_Eb(code, code + 1, code + 2);
				return 1;

			case OpCodeHandlerKind.Gv_Eb_REX:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Gv_Eb_REX(code, code + 1);
				return 1;

			case OpCodeHandlerKind.Gv_Ev_32_64:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Gv_Ev_32_64(code, code + 1, deserializer.ReadBoolean(), deserializer.ReadBoolean());
				return 1;

			case OpCodeHandlerKind.Gv_Ev_3a:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Gv_Ev(code, code + 1, code + 2);
				return 1;

			case OpCodeHandlerKind.Gv_Ev_3b:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Gv_Ev(code, code + 1, Code.INVALID);
				return 1;

			case OpCodeHandlerKind.Gv_Ev_Ib:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Gv_Ev_Ib(code, code + 1, code + 2);
				return 1;

			case OpCodeHandlerKind.Gv_Ev_Ib_REX:
				elem = new OpCodeHandler_Gv_Ev_Ib_REX(deserializer.ReadRegister(), code = deserializer.ReadCode(), code + 1);
				return 1;

			case OpCodeHandlerKind.Gv_Ev_Iz:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Gv_Ev_Iz(code, code + 1, code + 2);
				return 1;

			case OpCodeHandlerKind.Gv_Ev_REX:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Gv_Ev_REX(code, code + 1);
				return 1;

			case OpCodeHandlerKind.Gv_Ev2:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Gv_Ev2(code, code + 1, code + 2);
				return 1;

			case OpCodeHandlerKind.Gv_Ev3:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Gv_Ev3(code, code + 1, code + 2);
				return 1;

			case OpCodeHandlerKind.Gv_Ew:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Gv_Ew(code, code + 1, code + 2);
				return 1;

			case OpCodeHandlerKind.Gv_M:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Gv_M(code, code + 1, code + 2);
				return 1;

			case OpCodeHandlerKind.Gv_M_as:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Gv_M_as(code, code + 1, code + 2);
				return 1;

			case OpCodeHandlerKind.Gv_Ma:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Gv_Ma(code, code + 1);
				return 1;

			case OpCodeHandlerKind.Gv_Mp_2:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Gv_Mp(code, code + 1, Code.INVALID);
				return 1;

			case OpCodeHandlerKind.Gv_Mp_3:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Gv_Mp(code, code + 1, code + 2);
				return 1;

			case OpCodeHandlerKind.Gv_Mv:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Gv_Mv(code, code + 1, code + 2);
				return 1;

			case OpCodeHandlerKind.Gv_N:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Gv_N(code, code + 1);
				return 1;

			case OpCodeHandlerKind.Gv_N_Ib_REX:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Gv_N_Ib_REX(code, code + 1);
				return 1;

			case OpCodeHandlerKind.Gv_RX:
				elem = new OpCodeHandler_Gv_RX(deserializer.ReadRegister(), code = deserializer.ReadCode(), code + 1);
				return 1;

			case OpCodeHandlerKind.Gv_W:
				elem = new OpCodeHandler_Gv_W(deserializer.ReadRegister(), code = deserializer.ReadCode(), code + 1);
				return 1;

			case OpCodeHandlerKind.GvM_VX_Ib:
				elem = new OpCodeHandler_GvM_VX_Ib(deserializer.ReadRegister(), code = deserializer.ReadCode(), code + 1);
				return 1;

			case OpCodeHandlerKind.Ib:
				elem = new OpCodeHandler_Ib(deserializer.ReadCode());
				return 1;

			case OpCodeHandlerKind.Ib3:
				elem = new OpCodeHandler_Ib3(deserializer.ReadCode());
				return 1;

			case OpCodeHandlerKind.IbReg:
				elem = new OpCodeHandler_IbReg(deserializer.ReadCode(), deserializer.ReadRegister());
				return 1;

			case OpCodeHandlerKind.IbReg2:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_IbReg2(code, code + 1);
				return 1;

			case OpCodeHandlerKind.Iw_Ib:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Iw_Ib(code, code + 1, code + 2);
				return 1;

			case OpCodeHandlerKind.Jb:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Jb(code, code + 1, code + 2);
				return 1;

			case OpCodeHandlerKind.Jb2:
				elem = new OpCodeHandler_Jb2(deserializer.ReadCode(), deserializer.ReadCode(), deserializer.ReadCode(), deserializer.ReadCode(), deserializer.ReadCode(), deserializer.ReadCode(), deserializer.ReadCode());
				return 1;

			case OpCodeHandlerKind.Jdisp:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Jdisp(code, code + 1);
				return 1;

			case OpCodeHandlerKind.Jx:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Jx(code, code + 1, deserializer.ReadCode());
				return 1;

			case OpCodeHandlerKind.Jz:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Jz(code, code + 1, code + 2);
				return 1;

			case OpCodeHandlerKind.M_1:
				elem = new OpCodeHandler_M(deserializer.ReadCode());
				return 1;

			case OpCodeHandlerKind.M_2:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_M(code, code + 1);
				return 1;

			case OpCodeHandlerKind.M_REXW_2:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_M_REXW(code, code + 1);
				return 1;

			case OpCodeHandlerKind.M_REXW_4:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_M_REXW(code, code + 1, deserializer.ReadHandlerFlags(), deserializer.ReadHandlerFlags());
				return 1;

			case OpCodeHandlerKind.MemBx:
				elem = new OpCodeHandler_MemBx(deserializer.ReadCode());
				return 1;

			case OpCodeHandlerKind.Mf_1:
				elem = new OpCodeHandler_Mf(deserializer.ReadCode());
				return 1;

			case OpCodeHandlerKind.Mf_2a:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Mf(code, code + 1);
				return 1;

			case OpCodeHandlerKind.Mf_2b:
				elem = new OpCodeHandler_Mf(deserializer.ReadCode(), deserializer.ReadCode());
				return 1;

			case OpCodeHandlerKind.MIB_B:
				elem = new OpCodeHandler_MIB_B(deserializer.ReadCode());
				return 1;

			case OpCodeHandlerKind.MP:
				elem = new OpCodeHandler_MP(deserializer.ReadCode());
				return 1;

			case OpCodeHandlerKind.Ms:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Ms(code, code + 1, code + 2);
				return 1;

			case OpCodeHandlerKind.MV:
				elem = new OpCodeHandler_MV(deserializer.ReadRegister(), deserializer.ReadCode());
				return 1;

			case OpCodeHandlerKind.Mv_Gv:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Mv_Gv(code, code + 1, code + 2);
				return 1;

			case OpCodeHandlerKind.Mv_Gv_REXW:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Mv_Gv_REXW(code, code + 1);
				return 1;

			case OpCodeHandlerKind.NIb:
				elem = new OpCodeHandler_NIb(deserializer.ReadCode());
				return 1;

			case OpCodeHandlerKind.Ob_Reg:
				elem = new OpCodeHandler_Ob_Reg(deserializer.ReadCode(), deserializer.ReadRegister());
				return 1;

			case OpCodeHandlerKind.Ov_Reg:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Ov_Reg(code, code + 1, code + 2);
				return 1;

			case OpCodeHandlerKind.P_Ev:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_P_Ev(code, code + 1);
				return 1;

			case OpCodeHandlerKind.P_Ev_Ib:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_P_Ev_Ib(code, code + 1);
				return 1;

			case OpCodeHandlerKind.P_Q:
				elem = new OpCodeHandler_P_Q(deserializer.ReadCode());
				return 1;

			case OpCodeHandlerKind.P_Q_Ib:
				elem = new OpCodeHandler_P_Q_Ib(deserializer.ReadCode());
				return 1;

			case OpCodeHandlerKind.P_R:
				elem = new OpCodeHandler_P_R(deserializer.ReadRegister(), deserializer.ReadCode());
				return 1;

			case OpCodeHandlerKind.P_W:
				elem = new OpCodeHandler_P_W(deserializer.ReadRegister(), deserializer.ReadCode());
				return 1;

			case OpCodeHandlerKind.PushEv:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_PushEv(code, code + 1, code + 2);
				return 1;

			case OpCodeHandlerKind.PushIb2:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_PushIb2(code, code + 1, code + 2);
				return 1;

			case OpCodeHandlerKind.PushIz:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_PushIz(code, code + 1, code + 2);
				return 1;

			case OpCodeHandlerKind.PushOpSizeReg_4a:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_PushOpSizeReg(code, code + 1, code + 2, deserializer.ReadRegister());
				return 1;

			case OpCodeHandlerKind.PushOpSizeReg_4b:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_PushOpSizeReg(code, code + 1, Code.INVALID, deserializer.ReadRegister());
				return 1;

			case OpCodeHandlerKind.PushSimple2:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_PushSimple2(code, code + 1, code + 2);
				return 1;

			case OpCodeHandlerKind.PushSimpleReg:
				elem = new OpCodeHandler_PushSimpleReg(deserializer.ReadInt32(), code = deserializer.ReadCode(), code + 1, code + 2);
				return 1;

			case OpCodeHandlerKind.Q_P:
				elem = new OpCodeHandler_Q_P(deserializer.ReadCode());
				return 1;

			case OpCodeHandlerKind.R_C_3a:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_R_C(code, code + 1, deserializer.ReadRegister());
				return 1;

			case OpCodeHandlerKind.R_C_3b:
				elem = new OpCodeHandler_R_C(deserializer.ReadCode(), Code.INVALID, deserializer.ReadRegister());
				return 1;

			case OpCodeHandlerKind.rDI_P_N:
				elem = new OpCodeHandler_rDI_P_N(deserializer.ReadCode());
				return 1;

			case OpCodeHandlerKind.rDI_VX_RX:
				elem = new OpCodeHandler_rDI_VX_RX(deserializer.ReadRegister(), deserializer.ReadCode());
				return 1;

			case OpCodeHandlerKind.Reg:
				elem = new OpCodeHandler_Reg(deserializer.ReadCode(), deserializer.ReadRegister());
				return 1;

			case OpCodeHandlerKind.Reg_Ib2:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Reg_Ib2(code, code + 1);
				return 1;

			case OpCodeHandlerKind.Reg_Iz:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Reg_Iz(code, code + 1, code + 2);
				return 1;

			case OpCodeHandlerKind.Reg_Ob:
				elem = new OpCodeHandler_Reg_Ob(deserializer.ReadCode(), deserializer.ReadRegister());
				return 1;

			case OpCodeHandlerKind.Reg_Ov:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Reg_Ov(code, code + 1, code + 2);
				return 1;

			case OpCodeHandlerKind.Reg_Xb:
				elem = new OpCodeHandler_Reg_Xb(deserializer.ReadCode(), deserializer.ReadRegister());
				return 1;

			case OpCodeHandlerKind.Reg_Xv:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Reg_Xv(code, code + 1, code + 2);
				return 1;

			case OpCodeHandlerKind.Reg_Xv2:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Reg_Xv2(code, code + 1);
				return 1;

			case OpCodeHandlerKind.Reg_Yb:
				elem = new OpCodeHandler_Reg_Yb(deserializer.ReadCode(), deserializer.ReadRegister());
				return 1;

			case OpCodeHandlerKind.Reg_Yv:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Reg_Yv(code, code + 1, code + 2);
				return 1;

			case OpCodeHandlerKind.RegIb:
				elem = new OpCodeHandler_RegIb(deserializer.ReadCode(), deserializer.ReadRegister());
				return 1;

			case OpCodeHandlerKind.RegIb3:
				elem = new OpCodeHandler_RegIb3(deserializer.ReadInt32());
				return 1;

			case OpCodeHandlerKind.RegIz2:
				elem = new OpCodeHandler_RegIz2(deserializer.ReadInt32());
				return 1;

			case OpCodeHandlerKind.ReservedNop:
				elem = new OpCodeHandler_ReservedNop(deserializer.ReadHandler(), deserializer.ReadHandler());
				return 1;

			case OpCodeHandlerKind.RIb:
				elem = new OpCodeHandler_RIb(deserializer.ReadRegister(), deserializer.ReadCode());
				return 1;

			case OpCodeHandlerKind.RIbIb:
				elem = new OpCodeHandler_RIbIb(deserializer.ReadRegister(), deserializer.ReadCode());
				return 1;

			case OpCodeHandlerKind.Rv:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Rv(code, code + 1, code + 2);
				return 1;

			case OpCodeHandlerKind.Rv_32_64:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Rv_32_64(code, code + 1);
				return 1;

			case OpCodeHandlerKind.RvMw_Gw:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_RvMw_Gw(code, code + 1);
				return 1;

			case OpCodeHandlerKind.Simple:
				elem = new OpCodeHandler_Simple(deserializer.ReadCode());
				return 1;

			case OpCodeHandlerKind.Simple_ModRM:
				elem = new OpCodeHandler_Simple_ModRM(deserializer.ReadCode());
				return 1;

			case OpCodeHandlerKind.Simple2_3a:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Simple2(code, code + 1, code + 2);
				return 1;

			case OpCodeHandlerKind.Simple2_3b:
				elem = new OpCodeHandler_Simple2(deserializer.ReadCode(), deserializer.ReadCode(), deserializer.ReadCode());
				return 1;

			case OpCodeHandlerKind.Simple2Iw:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Simple2Iw(code, code + 1, code + 2);
				return 1;

			case OpCodeHandlerKind.Simple3:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Simple3(code, code + 1, code + 2);
				return 1;

			case OpCodeHandlerKind.Simple4:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Simple4(code, code + 1);
				return 1;

			case OpCodeHandlerKind.Simple5:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Simple5(code, code + 1, code + 2);
				return 1;

			case OpCodeHandlerKind.Simple5_ModRM_as:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Simple5_ModRM_as(code, code + 1, code + 2);
				return 1;

			case OpCodeHandlerKind.SimpleReg:
				elem = new OpCodeHandler_SimpleReg(deserializer.ReadCode(), deserializer.ReadInt32());
				return 1;

			case OpCodeHandlerKind.ST_STi:
				elem = new OpCodeHandler_ST_STi(deserializer.ReadCode());
				return 1;

			case OpCodeHandlerKind.STi:
				elem = new OpCodeHandler_STi(deserializer.ReadCode());
				return 1;

			case OpCodeHandlerKind.STi_ST:
				elem = new OpCodeHandler_STi_ST(deserializer.ReadCode());
				return 1;

			case OpCodeHandlerKind.Sw_Ev:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Sw_Ev(code, code + 1, code + 2);
				return 1;

			case OpCodeHandlerKind.V_Ev:
				elem = new OpCodeHandler_V_Ev(deserializer.ReadRegister(), code = deserializer.ReadCode(), code + 1);
				return 1;

			case OpCodeHandlerKind.VM:
				elem = new OpCodeHandler_VM(deserializer.ReadRegister(), deserializer.ReadCode());
				return 1;

			case OpCodeHandlerKind.VN:
				elem = new OpCodeHandler_VN(deserializer.ReadRegister(), deserializer.ReadCode());
				return 1;

			case OpCodeHandlerKind.VQ:
				elem = new OpCodeHandler_VQ(deserializer.ReadRegister(), deserializer.ReadCode());
				return 1;

			case OpCodeHandlerKind.VRIbIb:
				elem = new OpCodeHandler_VRIbIb(deserializer.ReadRegister(), deserializer.ReadCode());
				return 1;

			case OpCodeHandlerKind.VW_2:
				elem = new OpCodeHandler_VW(deserializer.ReadRegister(), deserializer.ReadCode());
				return 1;

			case OpCodeHandlerKind.VW_3:
				elem = new OpCodeHandler_VW(deserializer.ReadRegister(), deserializer.ReadCode(), deserializer.ReadCode());
				return 1;

			case OpCodeHandlerKind.VWIb_2:
				elem = new OpCodeHandler_VWIb(deserializer.ReadRegister(), deserializer.ReadCode());
				return 1;

			case OpCodeHandlerKind.VWIb_3:
				elem = new OpCodeHandler_VWIb(deserializer.ReadRegister(), code = deserializer.ReadCode(), code + 1);
				return 1;

			case OpCodeHandlerKind.VX_E_Ib:
				elem = new OpCodeHandler_VX_E_Ib(deserializer.ReadRegister(), code = deserializer.ReadCode(), code + 1);
				return 1;

			case OpCodeHandlerKind.VX_Ev:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_VX_Ev(code, code + 1);
				return 1;

			case OpCodeHandlerKind.Wbinvd:
				elem = new OpCodeHandler_Wbinvd();
				return 1;

			case OpCodeHandlerKind.WV:
				elem = new OpCodeHandler_WV(deserializer.ReadRegister(), deserializer.ReadCode());
				return 1;

			case OpCodeHandlerKind.Xb_Yb:
				elem = new OpCodeHandler_Xb_Yb(deserializer.ReadCode());
				return 1;

			case OpCodeHandlerKind.Xchg_Reg_rAX:
				elem = new OpCodeHandler_Xchg_Reg_rAX(deserializer.ReadInt32());
				return 1;

			case OpCodeHandlerKind.Xv_Yv:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Xv_Yv(code, code + 1, code + 2);
				return 1;

			case OpCodeHandlerKind.Yb_Reg:
				elem = new OpCodeHandler_Yb_Reg(deserializer.ReadCode(), deserializer.ReadRegister());
				return 1;

			case OpCodeHandlerKind.Yb_Xb:
				elem = new OpCodeHandler_Yb_Xb(deserializer.ReadCode());
				return 1;

			case OpCodeHandlerKind.Yv_Reg:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Yv_Reg(code, code + 1, code + 2);
				return 1;

			case OpCodeHandlerKind.Yv_Reg2:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Yv_Reg2(code, code + 1);
				return 1;

			case OpCodeHandlerKind.Yv_Xv:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_Yv_Xv(code, code + 1, code + 2);
				return 1;

			default:
				throw new InvalidOperationException();
			}
		}
	}
}
#endif
