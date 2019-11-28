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
			switch ((VexOpCodeHandlerKindEnum.Enum)handlerType.Value) {
			case VexOpCodeHandlerKindEnum.Enum.Invalid: return ("OpCodeHandler_Invalid", true);
			case VexOpCodeHandlerKindEnum.Enum.Invalid_NoModRM: return ("OpCodeHandler_Invalid", false);
			case VexOpCodeHandlerKindEnum.Enum.Bitness_DontReadModRM: return ("OpCodeHandler_Bitness_DontReadModRM", true);
			case VexOpCodeHandlerKindEnum.Enum.RM: return ("OpCodeHandler_RM", true);
			case VexOpCodeHandlerKindEnum.Enum.Group: return ("OpCodeHandler_Group", true);
			case VexOpCodeHandlerKindEnum.Enum.W: return ("OpCodeHandler_W", true);
			case VexOpCodeHandlerKindEnum.Enum.MandatoryPrefix2_1: return ("OpCodeHandler_MandatoryPrefix2", true);
			case VexOpCodeHandlerKindEnum.Enum.MandatoryPrefix2_4: return ("OpCodeHandler_MandatoryPrefix2", true);
			case VexOpCodeHandlerKindEnum.Enum.MandatoryPrefix2_NoModRM: return ("OpCodeHandler_MandatoryPrefix2", false);
			case VexOpCodeHandlerKindEnum.Enum.VectorLength_NoModRM: return ("OpCodeHandler_VectorLength_VEX", false);
			case VexOpCodeHandlerKindEnum.Enum.VectorLength: return ("OpCodeHandler_VectorLength_VEX", true);
			case VexOpCodeHandlerKindEnum.Enum.Simple: return ("OpCodeHandler_VEX_Simple", false);
			case VexOpCodeHandlerKindEnum.Enum.VHEv: return ("OpCodeHandler_VEX_VHEv", true);
			case VexOpCodeHandlerKindEnum.Enum.VHEvIb: return ("OpCodeHandler_VEX_VHEvIb", true);
			case VexOpCodeHandlerKindEnum.Enum.VW_2: return ("OpCodeHandler_VEX_VW", true);
			case VexOpCodeHandlerKindEnum.Enum.VW_3: return ("OpCodeHandler_VEX_VW", true);
			case VexOpCodeHandlerKindEnum.Enum.VX_Ev: return ("OpCodeHandler_VEX_VX_Ev", true);
			case VexOpCodeHandlerKindEnum.Enum.Ev_VX: return ("OpCodeHandler_VEX_Ev_VX", true);
			case VexOpCodeHandlerKindEnum.Enum.WV: return ("OpCodeHandler_VEX_WV", true);
			case VexOpCodeHandlerKindEnum.Enum.VM: return ("OpCodeHandler_VEX_VM", true);
			case VexOpCodeHandlerKindEnum.Enum.MV: return ("OpCodeHandler_VEX_MV", true);
			case VexOpCodeHandlerKindEnum.Enum.M: return ("OpCodeHandler_VEX_M", true);
			case VexOpCodeHandlerKindEnum.Enum.RdRq: return ("OpCodeHandler_VEX_RdRq", true);
			case VexOpCodeHandlerKindEnum.Enum.rDI_VX_RX: return ("OpCodeHandler_VEX_rDI_VX_RX", true);
			case VexOpCodeHandlerKindEnum.Enum.VWIb_2: return ("OpCodeHandler_VEX_VWIb", true);
			case VexOpCodeHandlerKindEnum.Enum.VWIb_3: return ("OpCodeHandler_VEX_VWIb", true);
			case VexOpCodeHandlerKindEnum.Enum.WVIb: return ("OpCodeHandler_VEX_WVIb", true);
			case VexOpCodeHandlerKindEnum.Enum.Ed_V_Ib: return ("OpCodeHandler_VEX_Ed_V_Ib", true);
			case VexOpCodeHandlerKindEnum.Enum.VHW_2: return ("OpCodeHandler_VEX_VHW", true);
			case VexOpCodeHandlerKindEnum.Enum.VHW_3: return ("OpCodeHandler_VEX_VHW", true);
			case VexOpCodeHandlerKindEnum.Enum.VHW_4: return ("OpCodeHandler_VEX_VHW", true);
			case VexOpCodeHandlerKindEnum.Enum.VWH: return ("OpCodeHandler_VEX_VWH", true);
			case VexOpCodeHandlerKindEnum.Enum.WHV: return ("OpCodeHandler_VEX_WHV", true);
			case VexOpCodeHandlerKindEnum.Enum.VHM: return ("OpCodeHandler_VEX_VHM", true);
			case VexOpCodeHandlerKindEnum.Enum.MHV: return ("OpCodeHandler_VEX_MHV", true);
			case VexOpCodeHandlerKindEnum.Enum.VHWIb_2: return ("OpCodeHandler_VEX_VHWIb", true);
			case VexOpCodeHandlerKindEnum.Enum.VHWIb_4: return ("OpCodeHandler_VEX_VHWIb", true);
			case VexOpCodeHandlerKindEnum.Enum.HRIb: return ("OpCodeHandler_VEX_HRIb", true);
			case VexOpCodeHandlerKindEnum.Enum.VHWIs4: return ("OpCodeHandler_VEX_VHWIs4", true);
			case VexOpCodeHandlerKindEnum.Enum.VHIs4W: return ("OpCodeHandler_VEX_VHIs4W", true);
			case VexOpCodeHandlerKindEnum.Enum.VHWIs5: return ("OpCodeHandler_VEX_VHWIs5", true);
			case VexOpCodeHandlerKindEnum.Enum.VHIs5W: return ("OpCodeHandler_VEX_VHIs5W", true);
			case VexOpCodeHandlerKindEnum.Enum.VK_HK_RK: return ("OpCodeHandler_VEX_VK_HK_RK", true);
			case VexOpCodeHandlerKindEnum.Enum.VK_RK: return ("OpCodeHandler_VEX_VK_RK", true);
			case VexOpCodeHandlerKindEnum.Enum.VK_RK_Ib: return ("OpCodeHandler_VEX_VK_RK_Ib", true);
			case VexOpCodeHandlerKindEnum.Enum.VK_WK: return ("OpCodeHandler_VEX_VK_WK", true);
			case VexOpCodeHandlerKindEnum.Enum.M_VK: return ("OpCodeHandler_VEX_M_VK", true);
			case VexOpCodeHandlerKindEnum.Enum.VK_R: return ("OpCodeHandler_VEX_VK_R", true);
			case VexOpCodeHandlerKindEnum.Enum.G_VK: return ("OpCodeHandler_VEX_G_VK", true);
			case VexOpCodeHandlerKindEnum.Enum.Gv_W: return ("OpCodeHandler_VEX_Gv_W", true);
			case VexOpCodeHandlerKindEnum.Enum.Gv_RX: return ("OpCodeHandler_VEX_Gv_RX", true);
			case VexOpCodeHandlerKindEnum.Enum.Gv_GPR_Ib: return ("OpCodeHandler_VEX_Gv_GPR_Ib", true);
			case VexOpCodeHandlerKindEnum.Enum.VX_VSIB_HX: return ("OpCodeHandler_VEX_VX_VSIB_HX", true);
			case VexOpCodeHandlerKindEnum.Enum.Gv_Gv_Ev: return ("OpCodeHandler_VEX_Gv_Gv_Ev", true);
			case VexOpCodeHandlerKindEnum.Enum.Gv_Ev_Gv: return ("OpCodeHandler_VEX_Gv_Ev_Gv", true);
			case VexOpCodeHandlerKindEnum.Enum.Hv_Ev: return ("OpCodeHandler_VEX_Hv_Ev", true);
			case VexOpCodeHandlerKindEnum.Enum.Hv_Ed_Id: return ("OpCodeHandler_VEX_Hv_Ed_Id", true);
			case VexOpCodeHandlerKindEnum.Enum.GvM_VX_Ib: return ("OpCodeHandler_VEX_GvM_VX_Ib", true);
			case VexOpCodeHandlerKindEnum.Enum.Gv_Ev_Ib: return ("OpCodeHandler_VEX_Gv_Ev_Ib", true);
			case VexOpCodeHandlerKindEnum.Enum.Gv_Ev_Id: return ("OpCodeHandler_VEX_Gv_Ev_Id", true);

			case VexOpCodeHandlerKindEnum.Enum.Invalid2:
			case VexOpCodeHandlerKindEnum.Enum.Dup:
			case VexOpCodeHandlerKindEnum.Enum.HandlerReference:
			case VexOpCodeHandlerKindEnum.Enum.ArrayReference:
			default: throw new InvalidOperationException();
			}
		}

		protected sealed override void WriteFieldsCore(FileWriter writer, object?[] handler) {
			if (!HandlerUtils.IsHandler(handler, out var enumValue))
				throw new InvalidOperationException();
			var typeInfo = GetHandlerTypeInfo(enumValue);
			WriteFirstFields(writer, typeInfo.name, typeInfo.hasModRM);

			switch ((VexOpCodeHandlerKindEnum.Enum)enumValue.Value) {
			case VexOpCodeHandlerKindEnum.Enum.Invalid:
			case VexOpCodeHandlerKindEnum.Enum.Invalid_NoModRM:
				if (handler.Length != 1)
					throw new InvalidOperationException();
				break;
			case VexOpCodeHandlerKindEnum.Enum.Bitness_DontReadModRM:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "handler1632", handler[1]);
				WriteField(writer, "handler64", handler[2]);
				break;
			case VexOpCodeHandlerKindEnum.Enum.RM:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "reg", handler[1]);
				WriteField(writer, "mem", handler[2]);
				break;
			case VexOpCodeHandlerKindEnum.Enum.Group:
				if (handler.Length != 2)
					throw new InvalidOperationException();
				WriteField(writer, "group_handlers", VerifyArray(8, handler[1]));
				break;
			case VexOpCodeHandlerKindEnum.Enum.W:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "handlers", new object?[] { handler[1], handler[2] });
				break;
			case VexOpCodeHandlerKindEnum.Enum.MandatoryPrefix2_1:
				if (handler.Length != 2)
					throw new InvalidOperationException();
				WriteField(writer, "handlers", new object?[] { handler[1], GetInvalid(), GetInvalid(), GetInvalid() });
				break;
			case VexOpCodeHandlerKindEnum.Enum.MandatoryPrefix2_4:
			case VexOpCodeHandlerKindEnum.Enum.MandatoryPrefix2_NoModRM:
				if (handler.Length != 5)
					throw new InvalidOperationException();
				WriteField(writer, "handlers", new object?[] { handler[1], handler[2], handler[3], handler[4] });
				break;
			case VexOpCodeHandlerKindEnum.Enum.VectorLength_NoModRM:
			case VexOpCodeHandlerKindEnum.Enum.VectorLength:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "handlers", new object?[] { handler[1], handler[2], GetInvalid(), GetInvalid() });
				break;
			case VexOpCodeHandlerKindEnum.Enum.Simple:
				if (handler.Length != 2)
					throw new InvalidOperationException();
				WriteField(writer, "code", handler[1]);
				break;
			case VexOpCodeHandlerKindEnum.Enum.VHEv:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code_w0", handler[2]);
				WriteField(writer, "code_w1", handler[3]);
				break;
			case VexOpCodeHandlerKindEnum.Enum.VHEvIb:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code_w0", handler[2]);
				WriteField(writer, "code_w1", handler[3]);
				break;
			case VexOpCodeHandlerKindEnum.Enum.VW_2:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg1", handler[1]);
				WriteField(writer, "base_reg2", handler[1]);
				WriteField(writer, "code", handler[2]);
				break;
			case VexOpCodeHandlerKindEnum.Enum.VW_3:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg1", handler[1]);
				WriteField(writer, "base_reg2", handler[2]);
				WriteField(writer, "code", handler[3]);
				break;
			case VexOpCodeHandlerKindEnum.Enum.VX_Ev:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code32", handler[1]);
				WriteField(writer, "code64", handler[2]);
				break;
			case VexOpCodeHandlerKindEnum.Enum.Ev_VX:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code32", handler[1]);
				WriteField(writer, "code64", handler[2]);
				break;
			case VexOpCodeHandlerKindEnum.Enum.WV:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg1", handler[1]);
				WriteField(writer, "base_reg2", handler[1]);
				WriteField(writer, "code", handler[2]);
				break;
			case VexOpCodeHandlerKindEnum.Enum.VM:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code", handler[2]);
				break;
			case VexOpCodeHandlerKindEnum.Enum.MV:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code", handler[2]);
				break;
			case VexOpCodeHandlerKindEnum.Enum.M:
				if (handler.Length != 2)
					throw new InvalidOperationException();
				WriteField(writer, "code", handler[1]);
				break;
			case VexOpCodeHandlerKindEnum.Enum.RdRq:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code32", handler[1]);
				WriteField(writer, "code64", handler[2]);
				break;
			case VexOpCodeHandlerKindEnum.Enum.rDI_VX_RX:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code", handler[2]);
				break;
			case VexOpCodeHandlerKindEnum.Enum.VWIb_2:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg1", handler[1]);
				WriteField(writer, "base_reg2", handler[1]);
				WriteField(writer, "code_w0", handler[2]);
				WriteField(writer, "code_w1", handler[2]);
				break;
			case VexOpCodeHandlerKindEnum.Enum.VWIb_3:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg1", handler[1]);
				WriteField(writer, "base_reg2", handler[1]);
				WriteField(writer, "code_w0", handler[2]);
				WriteField(writer, "code_w1", handler[3]);
				break;
			case VexOpCodeHandlerKindEnum.Enum.WVIb:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg1", handler[1]);
				WriteField(writer, "base_reg2", handler[2]);
				WriteField(writer, "code", handler[3]);
				break;
			case VexOpCodeHandlerKindEnum.Enum.Ed_V_Ib:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case VexOpCodeHandlerKindEnum.Enum.VHW_2:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg1", handler[1]);
				WriteField(writer, "base_reg2", handler[1]);
				WriteField(writer, "base_reg3", handler[1]);
				WriteField(writer, "code_r", handler[2]);
				WriteField(writer, "code_m", handler[2]);
				break;
			case VexOpCodeHandlerKindEnum.Enum.VHW_3:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg1", handler[1]);
				WriteField(writer, "base_reg2", handler[1]);
				WriteField(writer, "base_reg3", handler[1]);
				WriteField(writer, "code_r", handler[2]);
				WriteField(writer, "code_m", handler[3]);
				break;
			case VexOpCodeHandlerKindEnum.Enum.VHW_4:
				if (handler.Length != 5)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg1", handler[1]);
				WriteField(writer, "base_reg2", handler[2]);
				WriteField(writer, "base_reg3", handler[3]);
				WriteField(writer, "code_r", handler[4]);
				WriteField(writer, "code_m", handler[4]);
				break;
			case VexOpCodeHandlerKindEnum.Enum.VWH:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code", handler[2]);
				break;
			case VexOpCodeHandlerKindEnum.Enum.WHV:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code_r", handler[2]);
				WriteField(writer, "code_m", handler[2]);
				break;
			case VexOpCodeHandlerKindEnum.Enum.VHM:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code", handler[2]);
				break;
			case VexOpCodeHandlerKindEnum.Enum.MHV:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code", handler[2]);
				break;
			case VexOpCodeHandlerKindEnum.Enum.VHWIb_2:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg1", handler[1]);
				WriteField(writer, "base_reg2", handler[1]);
				WriteField(writer, "base_reg3", handler[1]);
				WriteField(writer, "code", handler[2]);
				break;
			case VexOpCodeHandlerKindEnum.Enum.VHWIb_4:
				if (handler.Length != 5)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg1", handler[1]);
				WriteField(writer, "base_reg2", handler[2]);
				WriteField(writer, "base_reg3", handler[3]);
				WriteField(writer, "code", handler[4]);
				break;
			case VexOpCodeHandlerKindEnum.Enum.HRIb:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code", handler[2]);
				break;
			case VexOpCodeHandlerKindEnum.Enum.VHWIs4:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code", handler[2]);
				break;
			case VexOpCodeHandlerKindEnum.Enum.VHIs4W:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code", handler[2]);
				break;
			case VexOpCodeHandlerKindEnum.Enum.VHWIs5:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code", handler[2]);
				break;
			case VexOpCodeHandlerKindEnum.Enum.VHIs5W:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code", handler[2]);
				break;
			case VexOpCodeHandlerKindEnum.Enum.VK_HK_RK:
				if (handler.Length != 2)
					throw new InvalidOperationException();
				WriteField(writer, "code", handler[1]);
				break;
			case VexOpCodeHandlerKindEnum.Enum.VK_RK:
				if (handler.Length != 2)
					throw new InvalidOperationException();
				WriteField(writer, "code", handler[1]);
				break;
			case VexOpCodeHandlerKindEnum.Enum.VK_RK_Ib:
				if (handler.Length != 2)
					throw new InvalidOperationException();
				WriteField(writer, "code", handler[1]);
				break;
			case VexOpCodeHandlerKindEnum.Enum.VK_WK:
				if (handler.Length != 2)
					throw new InvalidOperationException();
				WriteField(writer, "code", handler[1]);
				break;
			case VexOpCodeHandlerKindEnum.Enum.M_VK:
				if (handler.Length != 2)
					throw new InvalidOperationException();
				WriteField(writer, "code", handler[1]);
				break;
			case VexOpCodeHandlerKindEnum.Enum.VK_R:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code", handler[1]);
				WriteField(writer, "gpr", handler[2]);
				break;
			case VexOpCodeHandlerKindEnum.Enum.G_VK:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code", handler[1]);
				WriteField(writer, "gpr", handler[2]);
				break;
			case VexOpCodeHandlerKindEnum.Enum.Gv_W:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code_w0", handler[2]);
				WriteField(writer, "code_w1", handler[3]);
				break;
			case VexOpCodeHandlerKindEnum.Enum.Gv_RX:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case VexOpCodeHandlerKindEnum.Enum.Gv_GPR_Ib:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case VexOpCodeHandlerKindEnum.Enum.VX_VSIB_HX:
				if (handler.Length != 5)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg1", handler[1]);
				WriteField(writer, "vsib_index", handler[2]);
				WriteField(writer, "base_reg3", handler[3]);
				WriteField(writer, "code", handler[4]);
				break;
			case VexOpCodeHandlerKindEnum.Enum.Gv_Gv_Ev:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code32", handler[1]);
				WriteField(writer, "code64", handler[2]);
				break;
			case VexOpCodeHandlerKindEnum.Enum.Gv_Ev_Gv:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code32", handler[1]);
				WriteField(writer, "code64", handler[2]);
				break;
			case VexOpCodeHandlerKindEnum.Enum.Hv_Ev:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code32", handler[1]);
				WriteField(writer, "code64", handler[2]);
				break;
			case VexOpCodeHandlerKindEnum.Enum.Hv_Ed_Id:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code32", handler[1]);
				WriteField(writer, "code64", handler[2]);
				break;
			case VexOpCodeHandlerKindEnum.Enum.GvM_VX_Ib:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case VexOpCodeHandlerKindEnum.Enum.Gv_Ev_Ib:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code32", handler[1]);
				WriteField(writer, "code64", handler[2]);
				break;
			case VexOpCodeHandlerKindEnum.Enum.Gv_Ev_Id:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "code32", handler[1]);
				WriteField(writer, "code64", handler[2]);
				break;
			case VexOpCodeHandlerKindEnum.Enum.Invalid2:
			case VexOpCodeHandlerKindEnum.Enum.Dup:
			case VexOpCodeHandlerKindEnum.Enum.HandlerReference:
			case VexOpCodeHandlerKindEnum.Enum.ArrayReference:
			default:
				throw new InvalidOperationException();
			}
		}
	}
}
