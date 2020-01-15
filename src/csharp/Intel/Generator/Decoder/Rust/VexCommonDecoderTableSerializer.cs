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
			return (VexOpCodeHandlerKind)handlerType.Value switch {
				VexOpCodeHandlerKind.Invalid => ("OpCodeHandler_Invalid", true),
				VexOpCodeHandlerKind.Invalid_NoModRM => ("OpCodeHandler_Invalid", false),
				VexOpCodeHandlerKind.Bitness_DontReadModRM => ("OpCodeHandler_Bitness_DontReadModRM", true),
				VexOpCodeHandlerKind.RM => ("OpCodeHandler_RM", true),
				VexOpCodeHandlerKind.Group => ("OpCodeHandler_Group", true),
				VexOpCodeHandlerKind.W => ("OpCodeHandler_W", true),
				VexOpCodeHandlerKind.MandatoryPrefix2_1 => ("OpCodeHandler_MandatoryPrefix2", true),
				VexOpCodeHandlerKind.MandatoryPrefix2_4 => ("OpCodeHandler_MandatoryPrefix2", true),
				VexOpCodeHandlerKind.MandatoryPrefix2_NoModRM => ("OpCodeHandler_MandatoryPrefix2", false),
				VexOpCodeHandlerKind.VectorLength_NoModRM => ("OpCodeHandler_VectorLength_VEX", false),
				VexOpCodeHandlerKind.VectorLength => ("OpCodeHandler_VectorLength_VEX", true),
				VexOpCodeHandlerKind.Simple => ("OpCodeHandler_VEX_Simple", false),
				VexOpCodeHandlerKind.VHEv => ("OpCodeHandler_VEX_VHEv", true),
				VexOpCodeHandlerKind.VHEvIb => ("OpCodeHandler_VEX_VHEvIb", true),
				VexOpCodeHandlerKind.VW_2 => ("OpCodeHandler_VEX_VW", true),
				VexOpCodeHandlerKind.VW_3 => ("OpCodeHandler_VEX_VW", true),
				VexOpCodeHandlerKind.VX_Ev => ("OpCodeHandler_VEX_VX_Ev", true),
				VexOpCodeHandlerKind.Ev_VX => ("OpCodeHandler_VEX_Ev_VX", true),
				VexOpCodeHandlerKind.WV => ("OpCodeHandler_VEX_WV", true),
				VexOpCodeHandlerKind.VM => ("OpCodeHandler_VEX_VM", true),
				VexOpCodeHandlerKind.MV => ("OpCodeHandler_VEX_MV", true),
				VexOpCodeHandlerKind.M => ("OpCodeHandler_VEX_M", true),
				VexOpCodeHandlerKind.RdRq => ("OpCodeHandler_VEX_RdRq", true),
				VexOpCodeHandlerKind.rDI_VX_RX => ("OpCodeHandler_VEX_rDI_VX_RX", true),
				VexOpCodeHandlerKind.VWIb_2 => ("OpCodeHandler_VEX_VWIb", true),
				VexOpCodeHandlerKind.VWIb_3 => ("OpCodeHandler_VEX_VWIb", true),
				VexOpCodeHandlerKind.WVIb => ("OpCodeHandler_VEX_WVIb", true),
				VexOpCodeHandlerKind.Ed_V_Ib => ("OpCodeHandler_VEX_Ed_V_Ib", true),
				VexOpCodeHandlerKind.VHW_2 => ("OpCodeHandler_VEX_VHW", true),
				VexOpCodeHandlerKind.VHW_3 => ("OpCodeHandler_VEX_VHW", true),
				VexOpCodeHandlerKind.VHW_4 => ("OpCodeHandler_VEX_VHW", true),
				VexOpCodeHandlerKind.VWH => ("OpCodeHandler_VEX_VWH", true),
				VexOpCodeHandlerKind.WHV => ("OpCodeHandler_VEX_WHV", true),
				VexOpCodeHandlerKind.VHM => ("OpCodeHandler_VEX_VHM", true),
				VexOpCodeHandlerKind.MHV => ("OpCodeHandler_VEX_MHV", true),
				VexOpCodeHandlerKind.VHWIb_2 => ("OpCodeHandler_VEX_VHWIb", true),
				VexOpCodeHandlerKind.VHWIb_4 => ("OpCodeHandler_VEX_VHWIb", true),
				VexOpCodeHandlerKind.HRIb => ("OpCodeHandler_VEX_HRIb", true),
				VexOpCodeHandlerKind.VHWIs4 => ("OpCodeHandler_VEX_VHWIs4", true),
				VexOpCodeHandlerKind.VHIs4W => ("OpCodeHandler_VEX_VHIs4W", true),
				VexOpCodeHandlerKind.VHWIs5 => ("OpCodeHandler_VEX_VHWIs5", true),
				VexOpCodeHandlerKind.VHIs5W => ("OpCodeHandler_VEX_VHIs5W", true),
				VexOpCodeHandlerKind.VK_HK_RK => ("OpCodeHandler_VEX_VK_HK_RK", true),
				VexOpCodeHandlerKind.VK_RK => ("OpCodeHandler_VEX_VK_RK", true),
				VexOpCodeHandlerKind.VK_RK_Ib => ("OpCodeHandler_VEX_VK_RK_Ib", true),
				VexOpCodeHandlerKind.VK_WK => ("OpCodeHandler_VEX_VK_WK", true),
				VexOpCodeHandlerKind.M_VK => ("OpCodeHandler_VEX_M_VK", true),
				VexOpCodeHandlerKind.VK_R => ("OpCodeHandler_VEX_VK_R", true),
				VexOpCodeHandlerKind.G_VK => ("OpCodeHandler_VEX_G_VK", true),
				VexOpCodeHandlerKind.Gv_W => ("OpCodeHandler_VEX_Gv_W", true),
				VexOpCodeHandlerKind.Gv_RX => ("OpCodeHandler_VEX_Gv_RX", true),
				VexOpCodeHandlerKind.Gv_GPR_Ib => ("OpCodeHandler_VEX_Gv_GPR_Ib", true),
				VexOpCodeHandlerKind.VX_VSIB_HX => ("OpCodeHandler_VEX_VX_VSIB_HX", true),
				VexOpCodeHandlerKind.Gv_Gv_Ev => ("OpCodeHandler_VEX_Gv_Gv_Ev", true),
				VexOpCodeHandlerKind.Gv_Ev_Gv => ("OpCodeHandler_VEX_Gv_Ev_Gv", true),
				VexOpCodeHandlerKind.Hv_Ev => ("OpCodeHandler_VEX_Hv_Ev", true),
				VexOpCodeHandlerKind.Hv_Ed_Id => ("OpCodeHandler_VEX_Hv_Ed_Id", true),
				VexOpCodeHandlerKind.GvM_VX_Ib => ("OpCodeHandler_VEX_GvM_VX_Ib", true),
				VexOpCodeHandlerKind.Gv_Ev_Ib => ("OpCodeHandler_VEX_Gv_Ev_Ib", true),
				VexOpCodeHandlerKind.Gv_Ev_Id => ("OpCodeHandler_VEX_Gv_Ev_Id", true),
				_ => throw new InvalidOperationException(),
			};
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
