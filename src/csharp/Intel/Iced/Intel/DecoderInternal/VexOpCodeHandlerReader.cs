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

#if DECODER && (!NO_VEX || !NO_XOP)
using System;

namespace Iced.Intel.DecoderInternal {
	sealed class VexOpCodeHandlerReader : OpCodeHandlerReader {
		public override int ReadHandlers(ref TableDeserializer deserializer, OpCodeHandler?[] result, int resultIndex) {
			ref var elem = ref result[resultIndex];
			Code code;
			switch (deserializer.ReadVexOpCodeHandlerKind()) {
			case VexOpCodeHandlerKind.Invalid:
				elem = OpCodeHandler_Invalid.Instance;
				return 1;

			case VexOpCodeHandlerKind.Invalid2:
				result[resultIndex] = OpCodeHandler_Invalid.Instance;
				result[resultIndex + 1] = OpCodeHandler_Invalid.Instance;
				return 2;

			case VexOpCodeHandlerKind.Dup:
				int count = deserializer.ReadInt32();
				var handler = deserializer.ReadHandler();
				for (int i = 0; i < count; i++)
					result[resultIndex + i] = handler;
				return count;

			case VexOpCodeHandlerKind.Invalid_NoModRM:
				elem = OpCodeHandler_Invalid_NoModRM.Instance;
				return 1;

			case VexOpCodeHandlerKind.Bitness_DontReadModRM:
				elem = new OpCodeHandler_Bitness_DontReadModRM(deserializer.ReadHandler(), deserializer.ReadHandler());
				return 1;

			case VexOpCodeHandlerKind.HandlerReference:
				elem = deserializer.ReadHandlerReference();
				return 1;

			case VexOpCodeHandlerKind.ArrayReference:
				throw new InvalidOperationException();

			case VexOpCodeHandlerKind.RM:
				elem = new OpCodeHandler_RM(deserializer.ReadHandler(), deserializer.ReadHandler());
				return 1;

			case VexOpCodeHandlerKind.Group:
				elem = new OpCodeHandler_Group(deserializer.ReadArrayReference((uint)VexOpCodeHandlerKind.ArrayReference));
				return 1;

			case VexOpCodeHandlerKind.W:
				elem = new OpCodeHandler_W(deserializer.ReadHandler(), deserializer.ReadHandler());
				return 1;

			case VexOpCodeHandlerKind.MandatoryPrefix2_1:
				elem = new OpCodeHandler_MandatoryPrefix2(deserializer.ReadHandler());
				return 1;

			case VexOpCodeHandlerKind.MandatoryPrefix2_4:
				elem = new OpCodeHandler_MandatoryPrefix2(deserializer.ReadHandler(), deserializer.ReadHandler(), deserializer.ReadHandler(), deserializer.ReadHandler());
				return 1;

			case VexOpCodeHandlerKind.MandatoryPrefix2_NoModRM:
				elem = new OpCodeHandler_MandatoryPrefix2_NoModRM(deserializer.ReadHandler(), deserializer.ReadHandler(), deserializer.ReadHandler(), deserializer.ReadHandler());
				return 1;

			case VexOpCodeHandlerKind.VectorLength_NoModRM:
				elem = new OpCodeHandler_VectorLength_NoModRM_VEX(deserializer.ReadHandler(), deserializer.ReadHandler());
				return 1;

			case VexOpCodeHandlerKind.VectorLength:
				elem = new OpCodeHandler_VectorLength_VEX(deserializer.ReadHandler(), deserializer.ReadHandler());
				return 1;

			case VexOpCodeHandlerKind.Ed_V_Ib:
				elem = new OpCodeHandler_VEX_Ed_V_Ib(deserializer.ReadRegister(), code = deserializer.ReadCode(), code + 1);
				return 1;

			case VexOpCodeHandlerKind.Ev_VX:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_VEX_Ev_VX(code, code + 1);
				return 1;

			case VexOpCodeHandlerKind.G_VK:
				elem = new OpCodeHandler_VEX_G_VK(deserializer.ReadCode(), deserializer.ReadRegister());
				return 1;

			case VexOpCodeHandlerKind.Gv_Ev_Gv:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_VEX_Gv_Ev_Gv(code, code + 1);
				return 1;

			case VexOpCodeHandlerKind.Gv_Ev_Ib:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_VEX_Gv_Ev_Ib(code, code + 1);
				return 1;

			case VexOpCodeHandlerKind.Gv_Ev_Id:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_VEX_Gv_Ev_Id(code, code + 1);
				return 1;

			case VexOpCodeHandlerKind.Gv_GPR_Ib:
				elem = new OpCodeHandler_VEX_Gv_GPR_Ib(deserializer.ReadRegister(), code = deserializer.ReadCode(), code + 1);
				return 1;

			case VexOpCodeHandlerKind.Gv_Gv_Ev:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_VEX_Gv_Gv_Ev(code, code + 1);
				return 1;

			case VexOpCodeHandlerKind.Gv_RX:
				elem = new OpCodeHandler_VEX_Gv_RX(deserializer.ReadRegister(), code = deserializer.ReadCode(), code + 1);
				return 1;

			case VexOpCodeHandlerKind.Gv_W:
				elem = new OpCodeHandler_VEX_Gv_W(deserializer.ReadRegister(), code = deserializer.ReadCode(), code + 1);
				return 1;

			case VexOpCodeHandlerKind.GvM_VX_Ib:
				elem = new OpCodeHandler_VEX_GvM_VX_Ib(deserializer.ReadRegister(), code = deserializer.ReadCode(), code + 1);
				return 1;

			case VexOpCodeHandlerKind.HRIb:
				elem = new OpCodeHandler_VEX_HRIb(deserializer.ReadRegister(), deserializer.ReadCode());
				return 1;

			case VexOpCodeHandlerKind.Hv_Ed_Id:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_VEX_Hv_Ed_Id(code, code + 1);
				return 1;

			case VexOpCodeHandlerKind.Hv_Ev:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_VEX_Hv_Ev(code, code + 1);
				return 1;

			case VexOpCodeHandlerKind.M:
				elem = new OpCodeHandler_VEX_M(deserializer.ReadCode());
				return 1;

			case VexOpCodeHandlerKind.MHV:
				elem = new OpCodeHandler_VEX_MHV(deserializer.ReadRegister(), deserializer.ReadCode());
				return 1;

			case VexOpCodeHandlerKind.M_VK:
				elem = new OpCodeHandler_VEX_M_VK(deserializer.ReadCode());
				return 1;

			case VexOpCodeHandlerKind.MV:
				elem = new OpCodeHandler_VEX_MV(deserializer.ReadRegister(), deserializer.ReadCode());
				return 1;

			case VexOpCodeHandlerKind.rDI_VX_RX:
				elem = new OpCodeHandler_VEX_rDI_VX_RX(deserializer.ReadRegister(), deserializer.ReadCode());
				return 1;

			case VexOpCodeHandlerKind.RdRq:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_VEX_RdRq(code, code + 1);
				return 1;

			case VexOpCodeHandlerKind.Simple:
				elem = new OpCodeHandler_VEX_Simple(deserializer.ReadCode());
				return 1;

			case VexOpCodeHandlerKind.VHEv:
				elem = new OpCodeHandler_VEX_VHEv(deserializer.ReadRegister(), code = deserializer.ReadCode(), code + 1);
				return 1;

			case VexOpCodeHandlerKind.VHEvIb:
				elem = new OpCodeHandler_VEX_VHEvIb(deserializer.ReadRegister(), code = deserializer.ReadCode(), code + 1);
				return 1;

			case VexOpCodeHandlerKind.VHIs4W:
				elem = new OpCodeHandler_VEX_VHIs4W(deserializer.ReadRegister(), deserializer.ReadCode());
				return 1;

			case VexOpCodeHandlerKind.VHIs5W:
				elem = new OpCodeHandler_VEX_VHIs5W(deserializer.ReadRegister(), deserializer.ReadCode());
				return 1;

			case VexOpCodeHandlerKind.VHM:
				elem = new OpCodeHandler_VEX_VHM(deserializer.ReadRegister(), deserializer.ReadCode());
				return 1;

			case VexOpCodeHandlerKind.VHW_2:
				elem = new OpCodeHandler_VEX_VHW(deserializer.ReadRegister(), deserializer.ReadCode());
				return 1;

			case VexOpCodeHandlerKind.VHW_3:
				elem = new OpCodeHandler_VEX_VHW(deserializer.ReadRegister(), deserializer.ReadCode(), deserializer.ReadCode());
				return 1;

			case VexOpCodeHandlerKind.VHW_4:
				elem = new OpCodeHandler_VEX_VHW(deserializer.ReadRegister(), deserializer.ReadRegister(), deserializer.ReadRegister(), deserializer.ReadCode());
				return 1;

			case VexOpCodeHandlerKind.VHWIb_2:
				elem = new OpCodeHandler_VEX_VHWIb(deserializer.ReadRegister(), deserializer.ReadCode());
				return 1;

			case VexOpCodeHandlerKind.VHWIb_4:
				elem = new OpCodeHandler_VEX_VHWIb(deserializer.ReadRegister(), deserializer.ReadRegister(), deserializer.ReadRegister(), deserializer.ReadCode());
				return 1;

			case VexOpCodeHandlerKind.VHWIs4:
				elem = new OpCodeHandler_VEX_VHWIs4(deserializer.ReadRegister(), deserializer.ReadCode());
				return 1;

			case VexOpCodeHandlerKind.VHWIs5:
				elem = new OpCodeHandler_VEX_VHWIs5(deserializer.ReadRegister(), deserializer.ReadCode());
				return 1;

			case VexOpCodeHandlerKind.VK_HK_RK:
				elem = new OpCodeHandler_VEX_VK_HK_RK(deserializer.ReadCode());
				return 1;

			case VexOpCodeHandlerKind.VK_R:
				elem = new OpCodeHandler_VEX_VK_R(deserializer.ReadCode(), deserializer.ReadRegister());
				return 1;

			case VexOpCodeHandlerKind.VK_RK:
				elem = new OpCodeHandler_VEX_VK_RK(deserializer.ReadCode());
				return 1;

			case VexOpCodeHandlerKind.VK_RK_Ib:
				elem = new OpCodeHandler_VEX_VK_RK_Ib(deserializer.ReadCode());
				return 1;

			case VexOpCodeHandlerKind.VK_WK:
				elem = new OpCodeHandler_VEX_VK_WK(deserializer.ReadCode());
				return 1;

			case VexOpCodeHandlerKind.VM:
				elem = new OpCodeHandler_VEX_VM(deserializer.ReadRegister(), deserializer.ReadCode());
				return 1;

			case VexOpCodeHandlerKind.VW_2:
				elem = new OpCodeHandler_VEX_VW(deserializer.ReadRegister(), deserializer.ReadCode());
				return 1;

			case VexOpCodeHandlerKind.VW_3:
				elem = new OpCodeHandler_VEX_VW(deserializer.ReadRegister(), deserializer.ReadRegister(), deserializer.ReadCode());
				return 1;

			case VexOpCodeHandlerKind.VWH:
				elem = new OpCodeHandler_VEX_VWH(deserializer.ReadRegister(), deserializer.ReadCode());
				return 1;

			case VexOpCodeHandlerKind.VWIb_2:
				elem = new OpCodeHandler_VEX_VWIb(deserializer.ReadRegister(), deserializer.ReadCode());
				return 1;

			case VexOpCodeHandlerKind.VWIb_3:
				elem = new OpCodeHandler_VEX_VWIb(deserializer.ReadRegister(), code = deserializer.ReadCode(), code + 1);
				return 1;

			case VexOpCodeHandlerKind.VX_Ev:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_VEX_VX_Ev(code, code + 1);
				return 1;

			case VexOpCodeHandlerKind.VX_VSIB_HX:
				elem = new OpCodeHandler_VEX_VX_VSIB_HX(deserializer.ReadRegister(), deserializer.ReadRegister(), deserializer.ReadRegister(), deserializer.ReadCode());
				return 1;

			case VexOpCodeHandlerKind.WHV:
				elem = new OpCodeHandler_VEX_WHV(deserializer.ReadRegister(), deserializer.ReadCode());
				return 1;

			case VexOpCodeHandlerKind.WV:
				elem = new OpCodeHandler_VEX_WV(deserializer.ReadRegister(), deserializer.ReadCode());
				return 1;

			case VexOpCodeHandlerKind.WVIb:
				elem = new OpCodeHandler_VEX_WVIb(deserializer.ReadRegister(), deserializer.ReadRegister(), deserializer.ReadCode());
				return 1;

			default:
				throw new InvalidOperationException();
			}
		}
	}
}
#endif
