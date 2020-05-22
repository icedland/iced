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

#if DECODER && !NO_EVEX
using System;

namespace Iced.Intel.DecoderInternal {
	sealed class EvexOpCodeHandlerReader : OpCodeHandlerReader {
		public override int ReadHandlers(ref TableDeserializer deserializer, OpCodeHandler?[] result, int resultIndex) {
			ref var elem = ref result[resultIndex];
			Code code;
			switch (deserializer.ReadEvexOpCodeHandlerKind()) {
			case EvexOpCodeHandlerKind.Invalid:
				elem = OpCodeHandler_Invalid.Instance;
				return 1;

			case EvexOpCodeHandlerKind.Invalid2:
				result[resultIndex] = OpCodeHandler_Invalid.Instance;
				result[resultIndex + 1] = OpCodeHandler_Invalid.Instance;
				return 2;

			case EvexOpCodeHandlerKind.Dup:
				int count = deserializer.ReadInt32();
				var handler = deserializer.ReadHandler();
				for (int i = 0; i < count; i++)
					result[resultIndex + i] = handler;
				return count;

			case EvexOpCodeHandlerKind.HandlerReference:
				elem = deserializer.ReadHandlerReference();
				return 1;

			case EvexOpCodeHandlerKind.ArrayReference:
				throw new InvalidOperationException();

			case EvexOpCodeHandlerKind.RM:
				elem = new OpCodeHandler_RM(deserializer.ReadHandler(), deserializer.ReadHandler());
				return 1;

			case EvexOpCodeHandlerKind.Group:
				elem = new OpCodeHandler_Group(deserializer.ReadArrayReference((uint)EvexOpCodeHandlerKind.ArrayReference));
				return 1;

			case EvexOpCodeHandlerKind.W:
				elem = new OpCodeHandler_W(deserializer.ReadHandler(), deserializer.ReadHandler());
				return 1;

			case EvexOpCodeHandlerKind.MandatoryPrefix2:
				elem = new OpCodeHandler_MandatoryPrefix2(deserializer.ReadHandler(), deserializer.ReadHandler(), deserializer.ReadHandler(), deserializer.ReadHandler());
				return 1;

			case EvexOpCodeHandlerKind.VectorLength:
				elem = new OpCodeHandler_VectorLength_EVEX(deserializer.ReadHandler(), deserializer.ReadHandler(), deserializer.ReadHandler());
				return 1;

			case EvexOpCodeHandlerKind.VectorLength_er:
				elem = new OpCodeHandler_VectorLength_EVEX_er(deserializer.ReadHandler(), deserializer.ReadHandler(), deserializer.ReadHandler());
				return 1;

			case EvexOpCodeHandlerKind.Ed_V_Ib:
				elem = new OpCodeHandler_EVEX_Ed_V_Ib(deserializer.ReadRegister(), code = deserializer.ReadCode(), code + 1, deserializer.ReadTupleType(), deserializer.ReadTupleType());
				return 1;

			case EvexOpCodeHandlerKind.Ev_VX:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_EVEX_Ev_VX(code, code + 1, deserializer.ReadTupleType(), deserializer.ReadTupleType());
				return 1;

			case EvexOpCodeHandlerKind.Ev_VX_Ib:
				elem = new OpCodeHandler_EVEX_Ev_VX_Ib(deserializer.ReadRegister(), code = deserializer.ReadCode(), code + 1);
				return 1;

			case EvexOpCodeHandlerKind.Gv_W_er:
				elem = new OpCodeHandler_EVEX_Gv_W_er(deserializer.ReadRegister(), code = deserializer.ReadCode(), code + 1, deserializer.ReadTupleType(), deserializer.ReadBoolean());
				return 1;

			case EvexOpCodeHandlerKind.GvM_VX_Ib:
				elem = new OpCodeHandler_EVEX_GvM_VX_Ib(deserializer.ReadRegister(), code = deserializer.ReadCode(), code + 1, deserializer.ReadTupleType(), deserializer.ReadTupleType());
				return 1;

			case EvexOpCodeHandlerKind.HkWIb_3:
				elem = new OpCodeHandler_EVEX_HkWIb(deserializer.ReadRegister(), deserializer.ReadCode(), deserializer.ReadTupleType(), false);
				return 1;

			case EvexOpCodeHandlerKind.HkWIb_3b:
				elem = new OpCodeHandler_EVEX_HkWIb(deserializer.ReadRegister(), deserializer.ReadCode(), deserializer.ReadTupleType(), true);
				return 1;

			case EvexOpCodeHandlerKind.HWIb:
				elem = new OpCodeHandler_EVEX_HWIb(deserializer.ReadRegister(), deserializer.ReadCode(), deserializer.ReadTupleType());
				return 1;

			case EvexOpCodeHandlerKind.KkHW_3:
				elem = new OpCodeHandler_EVEX_KkHW(deserializer.ReadRegister(), deserializer.ReadCode(), deserializer.ReadTupleType(), false);
				return 1;

			case EvexOpCodeHandlerKind.KkHW_3b:
				elem = new OpCodeHandler_EVEX_KkHW(deserializer.ReadRegister(), deserializer.ReadCode(), deserializer.ReadTupleType(), true);
				return 1;

			case EvexOpCodeHandlerKind.KkHWIb_sae_3:
				elem = new OpCodeHandler_EVEX_KkHWIb_sae(deserializer.ReadRegister(), deserializer.ReadCode(), deserializer.ReadTupleType(), false);
				return 1;

			case EvexOpCodeHandlerKind.KkHWIb_sae_3b:
				elem = new OpCodeHandler_EVEX_KkHWIb_sae(deserializer.ReadRegister(), deserializer.ReadCode(), deserializer.ReadTupleType(), true);
				return 1;

			case EvexOpCodeHandlerKind.KkHWIb_3:
				elem = new OpCodeHandler_EVEX_KkHWIb(deserializer.ReadRegister(), deserializer.ReadCode(), deserializer.ReadTupleType(), false);
				return 1;

			case EvexOpCodeHandlerKind.KkHWIb_3b:
				elem = new OpCodeHandler_EVEX_KkHWIb(deserializer.ReadRegister(), deserializer.ReadCode(), deserializer.ReadTupleType(), true);
				return 1;

			case EvexOpCodeHandlerKind.KkWIb_3:
				elem = new OpCodeHandler_EVEX_KkWIb(deserializer.ReadRegister(), deserializer.ReadCode(), deserializer.ReadTupleType(), false);
				return 1;

			case EvexOpCodeHandlerKind.KkWIb_3b:
				elem = new OpCodeHandler_EVEX_KkWIb(deserializer.ReadRegister(), deserializer.ReadCode(), deserializer.ReadTupleType(), true);
				return 1;

			case EvexOpCodeHandlerKind.KP1HW:
				elem = new OpCodeHandler_EVEX_KP1HW(deserializer.ReadRegister(), deserializer.ReadCode(), deserializer.ReadTupleType());
				return 1;

			case EvexOpCodeHandlerKind.KR:
				elem = new OpCodeHandler_EVEX_KR(deserializer.ReadRegister(), deserializer.ReadCode());
				return 1;

			case EvexOpCodeHandlerKind.MV:
				elem = new OpCodeHandler_EVEX_MV(deserializer.ReadRegister(), deserializer.ReadCode(), deserializer.ReadTupleType());
				return 1;

			case EvexOpCodeHandlerKind.V_H_Ev_er:
				elem = new OpCodeHandler_EVEX_V_H_Ev_er(deserializer.ReadRegister(), code = deserializer.ReadCode(), code + 1, deserializer.ReadTupleType(), deserializer.ReadTupleType());
				return 1;

			case EvexOpCodeHandlerKind.V_H_Ev_Ib:
				elem = new OpCodeHandler_EVEX_V_H_Ev_Ib(deserializer.ReadRegister(), code = deserializer.ReadCode(), code + 1, deserializer.ReadTupleType(), deserializer.ReadTupleType());
				return 1;

			case EvexOpCodeHandlerKind.VHM:
				elem = new OpCodeHandler_EVEX_VHM(deserializer.ReadRegister(), deserializer.ReadCode(), deserializer.ReadTupleType());
				return 1;

			case EvexOpCodeHandlerKind.VHW_3:
				elem = new OpCodeHandler_EVEX_VHW(deserializer.ReadRegister(), deserializer.ReadCode(), deserializer.ReadTupleType());
				return 1;

			case EvexOpCodeHandlerKind.VHW_4:
				elem = new OpCodeHandler_EVEX_VHW(deserializer.ReadRegister(), deserializer.ReadCode(), deserializer.ReadCode(), deserializer.ReadTupleType());
				return 1;

			case EvexOpCodeHandlerKind.VHWIb:
				elem = new OpCodeHandler_EVEX_VHWIb(deserializer.ReadRegister(), deserializer.ReadCode(), deserializer.ReadTupleType());
				return 1;

			case EvexOpCodeHandlerKind.VK:
				elem = new OpCodeHandler_EVEX_VK(deserializer.ReadRegister(), deserializer.ReadCode());
				return 1;

			case EvexOpCodeHandlerKind.Vk_VSIB:
				elem = new OpCodeHandler_EVEX_Vk_VSIB(deserializer.ReadRegister(), deserializer.ReadRegister(), deserializer.ReadCode(), deserializer.ReadTupleType());
				return 1;

			case EvexOpCodeHandlerKind.VkEv_REXW_2:
				elem = new OpCodeHandler_EVEX_VkEv_REXW(deserializer.ReadRegister(), deserializer.ReadCode());
				return 1;

			case EvexOpCodeHandlerKind.VkEv_REXW_3:
				elem = new OpCodeHandler_EVEX_VkEv_REXW(deserializer.ReadRegister(), deserializer.ReadCode(), deserializer.ReadCode());
				return 1;

			case EvexOpCodeHandlerKind.VkHM:
				elem = new OpCodeHandler_EVEX_VkHM(deserializer.ReadRegister(), deserializer.ReadCode(), deserializer.ReadTupleType());
				return 1;

			case EvexOpCodeHandlerKind.VkHW_3:
				elem = new OpCodeHandler_EVEX_VkHW(deserializer.ReadRegister(), deserializer.ReadCode(), deserializer.ReadTupleType(), false);
				return 1;

			case EvexOpCodeHandlerKind.VkHW_3b:
				elem = new OpCodeHandler_EVEX_VkHW(deserializer.ReadRegister(), deserializer.ReadCode(), deserializer.ReadTupleType(), true);
				return 1;

			case EvexOpCodeHandlerKind.VkHW_5:
				elem = new OpCodeHandler_EVEX_VkHW(deserializer.ReadRegister(), deserializer.ReadRegister(), deserializer.ReadRegister(), deserializer.ReadCode(), deserializer.ReadTupleType(), false);
				return 1;

			case EvexOpCodeHandlerKind.VkHW_er_4:
				elem = new OpCodeHandler_EVEX_VkHW_er(deserializer.ReadRegister(), deserializer.ReadCode(), deserializer.ReadTupleType(), deserializer.ReadBoolean(), false);
				return 1;

			case EvexOpCodeHandlerKind.VkHW_er_4b:
				elem = new OpCodeHandler_EVEX_VkHW_er(deserializer.ReadRegister(), deserializer.ReadCode(), deserializer.ReadTupleType(), deserializer.ReadBoolean(), true);
				return 1;

			case EvexOpCodeHandlerKind.VkHWIb_3:
				elem = new OpCodeHandler_EVEX_VkHWIb(deserializer.ReadRegister(), deserializer.ReadCode(), deserializer.ReadTupleType(), false);
				return 1;

			case EvexOpCodeHandlerKind.VkHWIb_3b:
				elem = new OpCodeHandler_EVEX_VkHWIb(deserializer.ReadRegister(), deserializer.ReadCode(), deserializer.ReadTupleType(), true);
				return 1;

			case EvexOpCodeHandlerKind.VkHWIb_5:
				elem = new OpCodeHandler_EVEX_VkHWIb(deserializer.ReadRegister(), deserializer.ReadRegister(), deserializer.ReadRegister(), deserializer.ReadCode(), deserializer.ReadTupleType(), false);
				return 1;

			case EvexOpCodeHandlerKind.VkHWIb_er_4:
				elem = new OpCodeHandler_EVEX_VkHWIb_er(deserializer.ReadRegister(), deserializer.ReadCode(), deserializer.ReadTupleType(), false);
				return 1;

			case EvexOpCodeHandlerKind.VkHWIb_er_4b:
				elem = new OpCodeHandler_EVEX_VkHWIb_er(deserializer.ReadRegister(), deserializer.ReadCode(), deserializer.ReadTupleType(), true);
				return 1;

			case EvexOpCodeHandlerKind.VkM:
				elem = new OpCodeHandler_EVEX_VkM(deserializer.ReadRegister(), deserializer.ReadCode(), deserializer.ReadTupleType());
				return 1;

			case EvexOpCodeHandlerKind.VkW_3:
				elem = new OpCodeHandler_EVEX_VkW(deserializer.ReadRegister(), deserializer.ReadCode(), deserializer.ReadTupleType(), false);
				return 1;

			case EvexOpCodeHandlerKind.VkW_3b:
				elem = new OpCodeHandler_EVEX_VkW(deserializer.ReadRegister(), deserializer.ReadCode(), deserializer.ReadTupleType(), true);
				return 1;

			case EvexOpCodeHandlerKind.VkW_4:
				elem = new OpCodeHandler_EVEX_VkW(deserializer.ReadRegister(), deserializer.ReadRegister(), deserializer.ReadCode(), deserializer.ReadTupleType(), false);
				return 1;

			case EvexOpCodeHandlerKind.VkW_4b:
				elem = new OpCodeHandler_EVEX_VkW(deserializer.ReadRegister(), deserializer.ReadRegister(), deserializer.ReadCode(), deserializer.ReadTupleType(), true);
				return 1;

			case EvexOpCodeHandlerKind.VkW_er_4:
				elem = new OpCodeHandler_EVEX_VkW_er(deserializer.ReadRegister(), deserializer.ReadCode(), deserializer.ReadTupleType(), deserializer.ReadBoolean());
				return 1;

			case EvexOpCodeHandlerKind.VkW_er_5:
				elem = new OpCodeHandler_EVEX_VkW_er(deserializer.ReadRegister(), deserializer.ReadRegister(), deserializer.ReadCode(), deserializer.ReadTupleType(), deserializer.ReadBoolean());
				return 1;

			case EvexOpCodeHandlerKind.VkW_er_6:
				elem = new OpCodeHandler_EVEX_VkW_er(deserializer.ReadRegister(), deserializer.ReadRegister(), deserializer.ReadCode(), deserializer.ReadTupleType(), deserializer.ReadBoolean(), deserializer.ReadBoolean());
				return 1;

			case EvexOpCodeHandlerKind.VkWIb_3:
				elem = new OpCodeHandler_EVEX_VkWIb(deserializer.ReadRegister(), deserializer.ReadCode(), deserializer.ReadTupleType(), false);
				return 1;

			case EvexOpCodeHandlerKind.VkWIb_3b:
				elem = new OpCodeHandler_EVEX_VkWIb(deserializer.ReadRegister(), deserializer.ReadCode(), deserializer.ReadTupleType(), true);
				return 1;

			case EvexOpCodeHandlerKind.VkWIb_er:
				elem = new OpCodeHandler_EVEX_VkWIb_er(deserializer.ReadRegister(), deserializer.ReadCode(), deserializer.ReadTupleType());
				return 1;

			case EvexOpCodeHandlerKind.VM:
				elem = new OpCodeHandler_EVEX_VM(deserializer.ReadRegister(), deserializer.ReadCode(), deserializer.ReadTupleType());
				return 1;

			case EvexOpCodeHandlerKind.VSIB_k1:
				elem = new OpCodeHandler_EVEX_VSIB_k1(deserializer.ReadRegister(), deserializer.ReadCode(), deserializer.ReadTupleType());
				return 1;

			case EvexOpCodeHandlerKind.VSIB_k1_VX:
				elem = new OpCodeHandler_EVEX_VSIB_k1_VX(deserializer.ReadRegister(), deserializer.ReadRegister(), deserializer.ReadCode(), deserializer.ReadTupleType());
				return 1;

			case EvexOpCodeHandlerKind.VW:
				elem = new OpCodeHandler_EVEX_VW(deserializer.ReadRegister(), deserializer.ReadCode(), deserializer.ReadTupleType());
				return 1;

			case EvexOpCodeHandlerKind.VW_er:
				elem = new OpCodeHandler_EVEX_VW_er(deserializer.ReadRegister(), deserializer.ReadCode(), deserializer.ReadTupleType());
				return 1;

			case EvexOpCodeHandlerKind.VX_Ev:
				code = deserializer.ReadCode();
				elem = new OpCodeHandler_EVEX_VX_Ev(code, code + 1, deserializer.ReadTupleType(), deserializer.ReadTupleType());
				return 1;

			case EvexOpCodeHandlerKind.WkHV:
				elem = new OpCodeHandler_EVEX_WkHV(deserializer.ReadRegister(), deserializer.ReadCode(), deserializer.ReadTupleType());
				return 1;

			case EvexOpCodeHandlerKind.WkV_3:
				elem = new OpCodeHandler_EVEX_WkV(deserializer.ReadRegister(), deserializer.ReadCode(), deserializer.ReadTupleType());
				return 1;

			case EvexOpCodeHandlerKind.WkV_4a:
				elem = new OpCodeHandler_EVEX_WkV(deserializer.ReadRegister(), deserializer.ReadRegister(), deserializer.ReadCode(), deserializer.ReadTupleType());
				return 1;

			case EvexOpCodeHandlerKind.WkV_4b:
				elem = new OpCodeHandler_EVEX_WkV(deserializer.ReadRegister(), deserializer.ReadCode(), deserializer.ReadTupleType(), deserializer.ReadBoolean());
				return 1;

			case EvexOpCodeHandlerKind.WkVIb:
				elem = new OpCodeHandler_EVEX_WkVIb(deserializer.ReadRegister(), deserializer.ReadRegister(), deserializer.ReadCode(), deserializer.ReadTupleType());
				return 1;

			case EvexOpCodeHandlerKind.WkVIb_er:
				elem = new OpCodeHandler_EVEX_WkVIb_er(deserializer.ReadRegister(), deserializer.ReadRegister(), deserializer.ReadCode(), deserializer.ReadTupleType());
				return 1;

			case EvexOpCodeHandlerKind.WV:
				elem = new OpCodeHandler_EVEX_WV(deserializer.ReadRegister(), deserializer.ReadCode(), deserializer.ReadTupleType());
				return 1;

			default:
				throw new InvalidOperationException();
			}
		}
	}
}
#endif
