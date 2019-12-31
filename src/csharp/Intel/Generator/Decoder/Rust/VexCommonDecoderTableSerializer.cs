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
using Generator.Enums;
using Generator.Enums.Decoder;
using Generator.IO;

namespace Generator.Decoder.Rust {
	abstract class VexCommonDecoderTableSerializer : DecoderTableSerializer {
		protected override string GetHandlerTypeName(EnumValue handlerType) => GetHandlerTypeInfo(handlerType).name;

		(string name, bool hasModRM) GetHandlerTypeInfo(EnumValue handlerType) {
			if (handlerType.DeclaringType.TypeId != TypeIds.VexOpCodeHandlerKind)
				throw new InvalidOperationException();
			switch ((VexOpCodeHandlerKind)handlerType.Value) {
			case VexOpCodeHandlerKind.Invalid: return ("OpCodeHandler_Invalid", true);
			case VexOpCodeHandlerKind.Invalid_NoModRM: return ("OpCodeHandler_Invalid", false);
			case VexOpCodeHandlerKind.Bitness_DontReadModRM: return ("OpCodeHandler_Bitness_DontReadModRM", true);
			case VexOpCodeHandlerKind.RM: return ("OpCodeHandler_RM", true);
			case VexOpCodeHandlerKind.Group: return ("OpCodeHandler_Group", true);
			case VexOpCodeHandlerKind.W: return ("OpCodeHandler_W", true);
			case VexOpCodeHandlerKind.MandatoryPrefix2_1: return ("OpCodeHandler_MandatoryPrefix2", true);
			case VexOpCodeHandlerKind.MandatoryPrefix2_4: return ("OpCodeHandler_MandatoryPrefix2", true);
			case VexOpCodeHandlerKind.MandatoryPrefix2_NoModRM: return ("OpCodeHandler_MandatoryPrefix2", false);
			case VexOpCodeHandlerKind.VectorLength_NoModRM: return ("OpCodeHandler_VectorLength_VEX", false);
			case VexOpCodeHandlerKind.VectorLength: return ("OpCodeHandler_VectorLength_VEX", true);
			case VexOpCodeHandlerKind.Simple: return ("OpCodeHandler_VEX_Simple", false);
			case VexOpCodeHandlerKind.VHEv: return ("OpCodeHandler_VEX_VHEv", true);
			case VexOpCodeHandlerKind.VHEvIb: return ("OpCodeHandler_VEX_VHEvIb", true);
			case VexOpCodeHandlerKind.VW_2: return ("OpCodeHandler_VEX_VW", true);
			case VexOpCodeHandlerKind.VW_3: return ("OpCodeHandler_VEX_VW", true);
			case VexOpCodeHandlerKind.VX_Ev: return ("OpCodeHandler_VEX_VX_Ev", true);
			case VexOpCodeHandlerKind.Ev_VX: return ("OpCodeHandler_VEX_Ev_VX", true);
			case VexOpCodeHandlerKind.WV: return ("OpCodeHandler_VEX_WV", true);
			case VexOpCodeHandlerKind.VM: return ("OpCodeHandler_VEX_VM", true);
			case VexOpCodeHandlerKind.MV: return ("OpCodeHandler_VEX_MV", true);
			case VexOpCodeHandlerKind.M: return ("OpCodeHandler_VEX_M", true);
			case VexOpCodeHandlerKind.RdRq: return ("OpCodeHandler_VEX_RdRq", true);
			case VexOpCodeHandlerKind.rDI_VX_RX: return ("OpCodeHandler_VEX_rDI_VX_RX", true);
			case VexOpCodeHandlerKind.VWIb_2: return ("OpCodeHandler_VEX_VWIb", true);
			case VexOpCodeHandlerKind.VWIb_3: return ("OpCodeHandler_VEX_VWIb", true);
			case VexOpCodeHandlerKind.WVIb: return ("OpCodeHandler_VEX_WVIb", true);
			case VexOpCodeHandlerKind.Ed_V_Ib: return ("OpCodeHandler_VEX_Ed_V_Ib", true);
			case VexOpCodeHandlerKind.VHW_2: return ("OpCodeHandler_VEX_VHW", true);
			case VexOpCodeHandlerKind.VHW_3: return ("OpCodeHandler_VEX_VHW", true);
			case VexOpCodeHandlerKind.VHW_4: return ("OpCodeHandler_VEX_VHW", true);
			case VexOpCodeHandlerKind.VWH: return ("OpCodeHandler_VEX_VWH", true);
			case VexOpCodeHandlerKind.WHV: return ("OpCodeHandler_VEX_WHV", true);
			case VexOpCodeHandlerKind.VHM: return ("OpCodeHandler_VEX_VHM", true);
			case VexOpCodeHandlerKind.MHV: return ("OpCodeHandler_VEX_MHV", true);
			case VexOpCodeHandlerKind.VHWIb_2: return ("OpCodeHandler_VEX_VHWIb", true);
			case VexOpCodeHandlerKind.VHWIb_4: return ("OpCodeHandler_VEX_VHWIb", true);
			case VexOpCodeHandlerKind.HRIb: return ("OpCodeHandler_VEX_HRIb", true);
			case VexOpCodeHandlerKind.VHWIs4: return ("OpCodeHandler_VEX_VHWIs4", true);
			case VexOpCodeHandlerKind.VHIs4W: return ("OpCodeHandler_VEX_VHIs4W", true);
			case VexOpCodeHandlerKind.VHWIs5: return ("OpCodeHandler_VEX_VHWIs5", true);
			case VexOpCodeHandlerKind.VHIs5W: return ("OpCodeHandler_VEX_VHIs5W", true);
			case VexOpCodeHandlerKind.VK_HK_RK: return ("OpCodeHandler_VEX_VK_HK_RK", true);
			case VexOpCodeHandlerKind.VK_RK: return ("OpCodeHandler_VEX_VK_RK", true);
			case VexOpCodeHandlerKind.VK_RK_Ib: return ("OpCodeHandler_VEX_VK_RK_Ib", true);
			case VexOpCodeHandlerKind.VK_WK: return ("OpCodeHandler_VEX_VK_WK", true);
			case VexOpCodeHandlerKind.M_VK: return ("OpCodeHandler_VEX_M_VK", true);
			case VexOpCodeHandlerKind.VK_R: return ("OpCodeHandler_VEX_VK_R", true);
			case VexOpCodeHandlerKind.G_VK: return ("OpCodeHandler_VEX_G_VK", true);
			case VexOpCodeHandlerKind.Gv_W: return ("OpCodeHandler_VEX_Gv_W", true);
			case VexOpCodeHandlerKind.Gv_RX: return ("OpCodeHandler_VEX_Gv_RX", true);
			case VexOpCodeHandlerKind.Gv_GPR_Ib: return ("OpCodeHandler_VEX_Gv_GPR_Ib", true);
			case VexOpCodeHandlerKind.VX_VSIB_HX: return ("OpCodeHandler_VEX_VX_VSIB_HX", true);
			case VexOpCodeHandlerKind.Gv_Gv_Ev: return ("OpCodeHandler_VEX_Gv_Gv_Ev", true);
			case VexOpCodeHandlerKind.Gv_Ev_Gv: return ("OpCodeHandler_VEX_Gv_Ev_Gv", true);
			case VexOpCodeHandlerKind.Hv_Ev: return ("OpCodeHandler_VEX_Hv_Ev", true);
			case VexOpCodeHandlerKind.Hv_Ed_Id: return ("OpCodeHandler_VEX_Hv_Ed_Id", true);
			case VexOpCodeHandlerKind.GvM_VX_Ib: return ("OpCodeHandler_VEX_GvM_VX_Ib", true);
			case VexOpCodeHandlerKind.Gv_Ev_Ib: return ("OpCodeHandler_VEX_Gv_Ev_Ib", true);
			case VexOpCodeHandlerKind.Gv_Ev_Id: return ("OpCodeHandler_VEX_Gv_Ev_Id", true);

			case VexOpCodeHandlerKind.Invalid2:
			case VexOpCodeHandlerKind.Dup:
			case VexOpCodeHandlerKind.HandlerReference:
			case VexOpCodeHandlerKind.ArrayReference:
			default: throw new InvalidOperationException();
			}
		}

		protected sealed override void WriteFieldsCore(FileWriter writer, object?[] handler) {
			if (!HandlerUtils.IsHandler(handler, out var enumValue))
				throw new InvalidOperationException();
			var typeInfo = GetHandlerTypeInfo(enumValue);
			WriteFirstFields(writer, typeInfo.name, typeInfo.hasModRM);

			switch ((VexOpCodeHandlerKind)enumValue.Value) {
			case VexOpCodeHandlerKind.Invalid:
			case VexOpCodeHandlerKind.Invalid_NoModRM:
				if (handler.Length != 1)
					throw new InvalidOperationException();
				break;
			case VexOpCodeHandlerKind.Bitness_DontReadModRM:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "handler1632", handler[1]);
				WriteField(writer, "handler64", handler[2]);
				break;
			case VexOpCodeHandlerKind.RM:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "reg", handler[1]);
				WriteField(writer, "mem", handler[2]);
				break;
			case VexOpCodeHandlerKind.Group:
				if (handler.Length != 2)
					throw new InvalidOperationException();
				WriteField(writer, "group_handlers", VerifyArray(8, handler[1]));
				break;
			case VexOpCodeHandlerKind.W:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "handlers", new object?[] { handler[1], handler[2] });
				break;
			case VexOpCodeHandlerKind.MandatoryPrefix2_1:
				if (handler.Length != 2)
					throw new InvalidOperationException();
				WriteField(writer, "handlers", new object?[] { handler[1], GetInvalid(), GetInvalid(), GetInvalid() });
				break;
			case VexOpCodeHandlerKind.MandatoryPrefix2_4:
			case VexOpCodeHandlerKind.MandatoryPrefix2_NoModRM:
				if (handler.Length != 5)
					throw new InvalidOperationException();
				WriteField(writer, "handlers", new object?[] { handler[1], handler[2], handler[3], handler[4] });
				break;
			case VexOpCodeHandlerKind.VectorLength_NoModRM:
			case VexOpCodeHandlerKind.VectorLength:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "handlers", new object?[] { handler[1], handler[2], GetInvalid(), GetInvalid() });
				break;
			case VexOpCodeHandlerKind.Simple:
				if (handler.Length != 2)
					throw new InvalidOperationException();
				WriteField(writer, "code", handler[1]);
				break;
			case VexOpCodeHandlerKind.VHEv:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code_w0", handler[2]);
				WriteField(writer, "code_w1", handler[3]);
				break;
			case VexOpCodeHandlerKind.VHEvIb:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code_w0", handler[2]);
				WriteField(writer, "code_w1", handler[3]);
				break;
			case VexOpCodeHandlerKind.VW_2:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg1", handler[1]);
				WriteField(writer, "base_reg2", handler[1]);
				WriteField(writer, "code", handler[2]);
				break;
			case VexOpCodeHandlerKind.VW_3:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg1", handler[1]);
				WriteField(writer, "base_reg2", handler[2]);
				WriteField(writer, "code", handler[3]);
				break;
			case VexOpCodeHandlerKind.VX_Ev:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code32", handler[1]);
				WriteField(writer, "code64", handler[2]);
				break;
			case VexOpCodeHandlerKind.Ev_VX:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code32", handler[1]);
				WriteField(writer, "code64", handler[2]);
				break;
			case VexOpCodeHandlerKind.WV:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg1", handler[1]);
				WriteField(writer, "base_reg2", handler[1]);
				WriteField(writer, "code", handler[2]);
				break;
			case VexOpCodeHandlerKind.VM:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code", handler[2]);
				break;
			case VexOpCodeHandlerKind.MV:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code", handler[2]);
				break;
			case VexOpCodeHandlerKind.M:
				if (handler.Length != 2)
					throw new InvalidOperationException();
				WriteField(writer, "code", handler[1]);
				break;
			case VexOpCodeHandlerKind.RdRq:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code32", handler[1]);
				WriteField(writer, "code64", handler[2]);
				break;
			case VexOpCodeHandlerKind.rDI_VX_RX:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code", handler[2]);
				break;
			case VexOpCodeHandlerKind.VWIb_2:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg1", handler[1]);
				WriteField(writer, "base_reg2", handler[1]);
				WriteField(writer, "code_w0", handler[2]);
				WriteField(writer, "code_w1", handler[2]);
				break;
			case VexOpCodeHandlerKind.VWIb_3:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg1", handler[1]);
				WriteField(writer, "base_reg2", handler[1]);
				WriteField(writer, "code_w0", handler[2]);
				WriteField(writer, "code_w1", handler[3]);
				break;
			case VexOpCodeHandlerKind.WVIb:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg1", handler[1]);
				WriteField(writer, "base_reg2", handler[2]);
				WriteField(writer, "code", handler[3]);
				break;
			case VexOpCodeHandlerKind.Ed_V_Ib:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case VexOpCodeHandlerKind.VHW_2:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg1", handler[1]);
				WriteField(writer, "base_reg2", handler[1]);
				WriteField(writer, "base_reg3", handler[1]);
				WriteField(writer, "code_r", handler[2]);
				WriteField(writer, "code_m", handler[2]);
				break;
			case VexOpCodeHandlerKind.VHW_3:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg1", handler[1]);
				WriteField(writer, "base_reg2", handler[1]);
				WriteField(writer, "base_reg3", handler[1]);
				WriteField(writer, "code_r", handler[2]);
				WriteField(writer, "code_m", handler[3]);
				break;
			case VexOpCodeHandlerKind.VHW_4:
				if (handler.Length != 5)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg1", handler[1]);
				WriteField(writer, "base_reg2", handler[2]);
				WriteField(writer, "base_reg3", handler[3]);
				WriteField(writer, "code_r", handler[4]);
				WriteField(writer, "code_m", handler[4]);
				break;
			case VexOpCodeHandlerKind.VWH:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code", handler[2]);
				break;
			case VexOpCodeHandlerKind.WHV:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code_r", handler[2]);
				WriteField(writer, "code_m", handler[2]);
				break;
			case VexOpCodeHandlerKind.VHM:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code", handler[2]);
				break;
			case VexOpCodeHandlerKind.MHV:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code", handler[2]);
				break;
			case VexOpCodeHandlerKind.VHWIb_2:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg1", handler[1]);
				WriteField(writer, "base_reg2", handler[1]);
				WriteField(writer, "base_reg3", handler[1]);
				WriteField(writer, "code", handler[2]);
				break;
			case VexOpCodeHandlerKind.VHWIb_4:
				if (handler.Length != 5)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg1", handler[1]);
				WriteField(writer, "base_reg2", handler[2]);
				WriteField(writer, "base_reg3", handler[3]);
				WriteField(writer, "code", handler[4]);
				break;
			case VexOpCodeHandlerKind.HRIb:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code", handler[2]);
				break;
			case VexOpCodeHandlerKind.VHWIs4:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code", handler[2]);
				break;
			case VexOpCodeHandlerKind.VHIs4W:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code", handler[2]);
				break;
			case VexOpCodeHandlerKind.VHWIs5:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code", handler[2]);
				break;
			case VexOpCodeHandlerKind.VHIs5W:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code", handler[2]);
				break;
			case VexOpCodeHandlerKind.VK_HK_RK:
				if (handler.Length != 2)
					throw new InvalidOperationException();
				WriteField(writer, "code", handler[1]);
				break;
			case VexOpCodeHandlerKind.VK_RK:
				if (handler.Length != 2)
					throw new InvalidOperationException();
				WriteField(writer, "code", handler[1]);
				break;
			case VexOpCodeHandlerKind.VK_RK_Ib:
				if (handler.Length != 2)
					throw new InvalidOperationException();
				WriteField(writer, "code", handler[1]);
				break;
			case VexOpCodeHandlerKind.VK_WK:
				if (handler.Length != 2)
					throw new InvalidOperationException();
				WriteField(writer, "code", handler[1]);
				break;
			case VexOpCodeHandlerKind.M_VK:
				if (handler.Length != 2)
					throw new InvalidOperationException();
				WriteField(writer, "code", handler[1]);
				break;
			case VexOpCodeHandlerKind.VK_R:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code", handler[1]);
				WriteField(writer, "gpr", handler[2]);
				break;
			case VexOpCodeHandlerKind.G_VK:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code", handler[1]);
				WriteField(writer, "gpr", handler[2]);
				break;
			case VexOpCodeHandlerKind.Gv_W:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code_w0", handler[2]);
				WriteField(writer, "code_w1", handler[3]);
				break;
			case VexOpCodeHandlerKind.Gv_RX:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case VexOpCodeHandlerKind.Gv_GPR_Ib:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case VexOpCodeHandlerKind.VX_VSIB_HX:
				if (handler.Length != 5)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg1", handler[1]);
				WriteField(writer, "vsib_index", handler[2]);
				WriteField(writer, "base_reg3", handler[3]);
				WriteField(writer, "code", handler[4]);
				break;
			case VexOpCodeHandlerKind.Gv_Gv_Ev:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code32", handler[1]);
				WriteField(writer, "code64", handler[2]);
				break;
			case VexOpCodeHandlerKind.Gv_Ev_Gv:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code32", handler[1]);
				WriteField(writer, "code64", handler[2]);
				break;
			case VexOpCodeHandlerKind.Hv_Ev:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code32", handler[1]);
				WriteField(writer, "code64", handler[2]);
				break;
			case VexOpCodeHandlerKind.Hv_Ed_Id:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code32", handler[1]);
				WriteField(writer, "code64", handler[2]);
				break;
			case VexOpCodeHandlerKind.GvM_VX_Ib:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case VexOpCodeHandlerKind.Gv_Ev_Ib:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code32", handler[1]);
				WriteField(writer, "code64", handler[2]);
				break;
			case VexOpCodeHandlerKind.Gv_Ev_Id:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code32", handler[1]);
				WriteField(writer, "code64", handler[2]);
				break;
			case VexOpCodeHandlerKind.Invalid2:
			case VexOpCodeHandlerKind.Dup:
			case VexOpCodeHandlerKind.HandlerReference:
			case VexOpCodeHandlerKind.ArrayReference:
			default:
				throw new InvalidOperationException();
			}
		}
	}
}
