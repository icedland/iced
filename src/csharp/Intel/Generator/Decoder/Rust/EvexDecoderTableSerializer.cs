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
using Generator.IO;

namespace Generator.Decoder.Rust {
	sealed class EvexDecoderTableSerializer : DecoderTableSerializer {
		public override string Name => "evex";
		protected override (string name, object?[] handlers)[] Handlers => OpCodeHandlersTables_EVEX.GetHandlers();
		protected override (string origName, string newName)[] RootNames => new[] {
			(OpCodeHandlersTables_EVEX.TwoByteHandlers_0FXX, "HANDLERS_EVEX_0FXX"),
			(OpCodeHandlersTables_EVEX.ThreeByteHandlers_0F38XX, "HANDLERS_EVEX_0F38XX"),
			(OpCodeHandlersTables_EVEX.ThreeByteHandlers_0F3AXX, "HANDLERS_EVEX_0F3AXX"),
		};

		protected override string GetHandlerTypeName(EnumValue handlerType) => GetHandlerTypeInfo(handlerType).name;

		(string name, bool hasModRM) GetHandlerTypeInfo(EnumValue handlerType) {
			if (handlerType.DeclaringType.TypeId != TypeIds.EvexOpCodeHandlerKind)
				throw new InvalidOperationException();
			switch ((EvexOpCodeHandlerKindEnum.Enum)handlerType.Value) {
			case EvexOpCodeHandlerKindEnum.Enum.Invalid: return ("OpCodeHandler_Invalid", true);
			case EvexOpCodeHandlerKindEnum.Enum.RM: return ("OpCodeHandler_RM", true);
			case EvexOpCodeHandlerKindEnum.Enum.Group: return ("OpCodeHandler_Group", true);
			case EvexOpCodeHandlerKindEnum.Enum.W: return ("OpCodeHandler_W", true);
			case EvexOpCodeHandlerKindEnum.Enum.MandatoryPrefix2: return ("OpCodeHandler_MandatoryPrefix2", true);
			case EvexOpCodeHandlerKindEnum.Enum.VectorLength: return ("OpCodeHandler_VectorLength_EVEX", true);
			case EvexOpCodeHandlerKindEnum.Enum.VectorLength_er: return ("OpCodeHandler_VectorLength_EVEX_er", true);
			case EvexOpCodeHandlerKindEnum.Enum.V_H_Ev_er: return ("OpCodeHandler_EVEX_V_H_Ev_er", true);
			case EvexOpCodeHandlerKindEnum.Enum.V_H_Ev_Ib: return ("OpCodeHandler_EVEX_V_H_Ev_Ib", true);
			case EvexOpCodeHandlerKindEnum.Enum.Ed_V_Ib: return ("OpCodeHandler_EVEX_Ed_V_Ib", true);
			case EvexOpCodeHandlerKindEnum.Enum.VkHW_er_4: return ("OpCodeHandler_EVEX_VkHW_er", true);
			case EvexOpCodeHandlerKindEnum.Enum.VkHW_er_4b: return ("OpCodeHandler_EVEX_VkHW_er", true);
			case EvexOpCodeHandlerKindEnum.Enum.VkW_er_4: return ("OpCodeHandler_EVEX_VkW_er", true);
			case EvexOpCodeHandlerKindEnum.Enum.VkW_er_5: return ("OpCodeHandler_EVEX_VkW_er", true);
			case EvexOpCodeHandlerKindEnum.Enum.VkW_er_6: return ("OpCodeHandler_EVEX_VkW_er", true);
			case EvexOpCodeHandlerKindEnum.Enum.VkWIb_er: return ("OpCodeHandler_EVEX_VkWIb_er", true);
			case EvexOpCodeHandlerKindEnum.Enum.VkW_3: return ("OpCodeHandler_EVEX_VkW", true);
			case EvexOpCodeHandlerKindEnum.Enum.VkW_3b: return ("OpCodeHandler_EVEX_VkW", true);
			case EvexOpCodeHandlerKindEnum.Enum.VkW_4: return ("OpCodeHandler_EVEX_VkW", true);
			case EvexOpCodeHandlerKindEnum.Enum.VkW_4b: return ("OpCodeHandler_EVEX_VkW", true);
			case EvexOpCodeHandlerKindEnum.Enum.WkV_3: return ("OpCodeHandler_EVEX_WkV", true);
			case EvexOpCodeHandlerKindEnum.Enum.WkV_4a: return ("OpCodeHandler_EVEX_WkV", true);
			case EvexOpCodeHandlerKindEnum.Enum.WkV_4b: return ("OpCodeHandler_EVEX_WkV", true);
			case EvexOpCodeHandlerKindEnum.Enum.VkM: return ("OpCodeHandler_EVEX_VkM", true);
			case EvexOpCodeHandlerKindEnum.Enum.VkWIb_3: return ("OpCodeHandler_EVEX_VkWIb", true);
			case EvexOpCodeHandlerKindEnum.Enum.VkWIb_3b: return ("OpCodeHandler_EVEX_VkWIb", true);
			case EvexOpCodeHandlerKindEnum.Enum.WkVIb: return ("OpCodeHandler_EVEX_WkVIb", true);
			case EvexOpCodeHandlerKindEnum.Enum.HkWIb_3: return ("OpCodeHandler_EVEX_HkWIb", true);
			case EvexOpCodeHandlerKindEnum.Enum.HkWIb_3b: return ("OpCodeHandler_EVEX_HkWIb", true);
			case EvexOpCodeHandlerKindEnum.Enum.HWIb: return ("OpCodeHandler_EVEX_HWIb", true);
			case EvexOpCodeHandlerKindEnum.Enum.WkVIb_er: return ("OpCodeHandler_EVEX_WkVIb_er", true);
			case EvexOpCodeHandlerKindEnum.Enum.VW_er: return ("OpCodeHandler_EVEX_VW_er", true);
			case EvexOpCodeHandlerKindEnum.Enum.VW: return ("OpCodeHandler_EVEX_VW", true);
			case EvexOpCodeHandlerKindEnum.Enum.WV: return ("OpCodeHandler_EVEX_WV", true);
			case EvexOpCodeHandlerKindEnum.Enum.VM: return ("OpCodeHandler_EVEX_VM", true);
			case EvexOpCodeHandlerKindEnum.Enum.VK: return ("OpCodeHandler_EVEX_VK", true);
			case EvexOpCodeHandlerKindEnum.Enum.KR: return ("OpCodeHandler_EVEX_KR", true);
			case EvexOpCodeHandlerKindEnum.Enum.KkHWIb_sae_3: return ("OpCodeHandler_EVEX_KkHWIb_sae", true);
			case EvexOpCodeHandlerKindEnum.Enum.KkHWIb_sae_3b: return ("OpCodeHandler_EVEX_KkHWIb_sae", true);
			case EvexOpCodeHandlerKindEnum.Enum.VkHW_3: return ("OpCodeHandler_EVEX_VkHW", true);
			case EvexOpCodeHandlerKindEnum.Enum.VkHW_3b: return ("OpCodeHandler_EVEX_VkHW", true);
			case EvexOpCodeHandlerKindEnum.Enum.VkHW_5: return ("OpCodeHandler_EVEX_VkHW", true);
			case EvexOpCodeHandlerKindEnum.Enum.VkHM: return ("OpCodeHandler_EVEX_VkHM", true);
			case EvexOpCodeHandlerKindEnum.Enum.VkHWIb_3: return ("OpCodeHandler_EVEX_VkHWIb", true);
			case EvexOpCodeHandlerKindEnum.Enum.VkHWIb_3b: return ("OpCodeHandler_EVEX_VkHWIb", true);
			case EvexOpCodeHandlerKindEnum.Enum.VkHWIb_5: return ("OpCodeHandler_EVEX_VkHWIb", true);
			case EvexOpCodeHandlerKindEnum.Enum.VkHWIb_er_4: return ("OpCodeHandler_EVEX_VkHWIb_er", true);
			case EvexOpCodeHandlerKindEnum.Enum.VkHWIb_er_4b: return ("OpCodeHandler_EVEX_VkHWIb_er", true);
			case EvexOpCodeHandlerKindEnum.Enum.KkHW_3: return ("OpCodeHandler_EVEX_KkHW", true);
			case EvexOpCodeHandlerKindEnum.Enum.KkHW_3b: return ("OpCodeHandler_EVEX_KkHW", true);
			case EvexOpCodeHandlerKindEnum.Enum.KP1HW: return ("OpCodeHandler_EVEX_KP1HW", true);
			case EvexOpCodeHandlerKindEnum.Enum.KkHWIb_3: return ("OpCodeHandler_EVEX_KkHWIb", true);
			case EvexOpCodeHandlerKindEnum.Enum.KkHWIb_3b: return ("OpCodeHandler_EVEX_KkHWIb", true);
			case EvexOpCodeHandlerKindEnum.Enum.WkHV: return ("OpCodeHandler_EVEX_WkHV", true);
			case EvexOpCodeHandlerKindEnum.Enum.VHWIb: return ("OpCodeHandler_EVEX_VHWIb", true);
			case EvexOpCodeHandlerKindEnum.Enum.VHW_3: return ("OpCodeHandler_EVEX_VHW", true);
			case EvexOpCodeHandlerKindEnum.Enum.VHW_4: return ("OpCodeHandler_EVEX_VHW", true);
			case EvexOpCodeHandlerKindEnum.Enum.VHM: return ("OpCodeHandler_EVEX_VHM", true);
			case EvexOpCodeHandlerKindEnum.Enum.Gv_W_er: return ("OpCodeHandler_EVEX_Gv_W_er", true);
			case EvexOpCodeHandlerKindEnum.Enum.VX_Ev: return ("OpCodeHandler_EVEX_VX_Ev", true);
			case EvexOpCodeHandlerKindEnum.Enum.Ev_VX: return ("OpCodeHandler_EVEX_Ev_VX", true);
			case EvexOpCodeHandlerKindEnum.Enum.Ev_VX_Ib: return ("OpCodeHandler_EVEX_Ev_VX_Ib", true);
			case EvexOpCodeHandlerKindEnum.Enum.MV: return ("OpCodeHandler_EVEX_MV", true);
			case EvexOpCodeHandlerKindEnum.Enum.VkEv_REXW_2: return ("OpCodeHandler_EVEX_VkEv_REXW", true);
			case EvexOpCodeHandlerKindEnum.Enum.VkEv_REXW_3: return ("OpCodeHandler_EVEX_VkEv_REXW", true);
			case EvexOpCodeHandlerKindEnum.Enum.Vk_VSIB: return ("OpCodeHandler_EVEX_Vk_VSIB", true);
			case EvexOpCodeHandlerKindEnum.Enum.VSIB_k1_VX: return ("OpCodeHandler_EVEX_VSIB_k1_VX", true);
			case EvexOpCodeHandlerKindEnum.Enum.VSIB_k1: return ("OpCodeHandler_EVEX_VSIB_k1", true);
			case EvexOpCodeHandlerKindEnum.Enum.GvM_VX_Ib: return ("OpCodeHandler_EVEX_GvM_VX_Ib", true);
			case EvexOpCodeHandlerKindEnum.Enum.KkWIb_3: return ("OpCodeHandler_EVEX_KkWIb", true);
			case EvexOpCodeHandlerKindEnum.Enum.KkWIb_3b: return ("OpCodeHandler_EVEX_KkWIb", true);
			case EvexOpCodeHandlerKindEnum.Enum.Invalid2:
			case EvexOpCodeHandlerKindEnum.Enum.Dup:
			case EvexOpCodeHandlerKindEnum.Enum.HandlerReference:
			case EvexOpCodeHandlerKindEnum.Enum.ArrayReference:
			default: throw new InvalidOperationException();
			}
		}

		protected sealed override void WriteFieldsCore(FileWriter writer, object?[] handler) {
			if (!HandlerUtils.IsHandler(handler, out var enumValue))
				throw new InvalidOperationException();
			var typeInfo = GetHandlerTypeInfo(enumValue);
			WriteFirstFields(writer, typeInfo.name, typeInfo.hasModRM);

			var e = (EvexOpCodeHandlerKindEnum.Enum)enumValue.Value;
			switch (e) {
			case EvexOpCodeHandlerKindEnum.Enum.Invalid:
				if (handler.Length != 1)
					throw new InvalidOperationException();
				break;
			case EvexOpCodeHandlerKindEnum.Enum.RM:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "reg", handler[1]);
				WriteField(writer, "mem", handler[2]);
				break;
			case EvexOpCodeHandlerKindEnum.Enum.Group:
				if (handler.Length != 2)
					throw new InvalidOperationException();
				WriteField(writer, "group_handlers", VerifyArray(8, handler[1]));
				break;
			case EvexOpCodeHandlerKindEnum.Enum.W:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "handlers", new object?[] { handler[1], handler[2] });
				break;
			case EvexOpCodeHandlerKindEnum.Enum.MandatoryPrefix2:
				if (handler.Length != 5)
					throw new InvalidOperationException();
				WriteField(writer, "handlers", new object?[] { handler[1], handler[2], handler[3], handler[4] });
				break;
			case EvexOpCodeHandlerKindEnum.Enum.VectorLength:
			case EvexOpCodeHandlerKindEnum.Enum.VectorLength_er:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "handlers", new object?[] { handler[1], handler[2], handler[3], GetInvalid() });
				break;
			case EvexOpCodeHandlerKindEnum.Enum.V_H_Ev_er:
				if (handler.Length != 6)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code_w0", handler[2]);
				WriteField(writer, "code_w1", handler[3]);
				WriteField(writer, "tuple_type_w0", handler[4]);
				WriteField(writer, "tuple_type_w1", handler[5]);
				break;
			case EvexOpCodeHandlerKindEnum.Enum.V_H_Ev_Ib:
				if (handler.Length != 6)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code_w0", handler[2]);
				WriteField(writer, "code_w1", handler[3]);
				WriteField(writer, "tuple_type_w0", handler[4]);
				WriteField(writer, "tuple_type_w1", handler[5]);
				break;
			case EvexOpCodeHandlerKindEnum.Enum.Ed_V_Ib:
				if (handler.Length != 6)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				WriteField(writer, "tuple_type32", handler[4]);
				WriteField(writer, "tuple_type64", handler[5]);
				break;
			case EvexOpCodeHandlerKindEnum.Enum.VkHW_er_4:
			case EvexOpCodeHandlerKindEnum.Enum.VkHW_er_4b:
				if (handler.Length != 5)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code", handler[2]);
				WriteField(writer, "tuple_type", handler[3]);
				WriteField(writer, "only_sae", handler[4]);
				WriteField(writer, "can_broadcast", e == EvexOpCodeHandlerKindEnum.Enum.VkHW_er_4b);
				break;
			case EvexOpCodeHandlerKindEnum.Enum.VkW_er_4:
				if (handler.Length != 5)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg1", handler[1]);
				WriteField(writer, "base_reg2", handler[1]);
				WriteField(writer, "code", handler[2]);
				WriteField(writer, "tuple_type", handler[3]);
				WriteField(writer, "only_sae", handler[4]);
				WriteField(writer, "can_broadcast", true);
				break;
			case EvexOpCodeHandlerKindEnum.Enum.VkW_er_5:
				if (handler.Length != 6)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg1", handler[1]);
				WriteField(writer, "base_reg2", handler[2]);
				WriteField(writer, "code", handler[3]);
				WriteField(writer, "tuple_type", handler[4]);
				WriteField(writer, "only_sae", handler[5]);
				WriteField(writer, "can_broadcast", true);
				break;
			case EvexOpCodeHandlerKindEnum.Enum.VkW_er_6:
				if (handler.Length != 7)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg1", handler[1]);
				WriteField(writer, "base_reg2", handler[2]);
				WriteField(writer, "code", handler[3]);
				WriteField(writer, "tuple_type", handler[4]);
				WriteField(writer, "only_sae", handler[5]);
				WriteField(writer, "can_broadcast", handler[6]);
				break;
			case EvexOpCodeHandlerKindEnum.Enum.VkWIb_er:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg1", handler[1]);
				WriteField(writer, "base_reg2", handler[1]);
				WriteField(writer, "code", handler[2]);
				WriteField(writer, "tuple_type", handler[3]);
				break;
			case EvexOpCodeHandlerKindEnum.Enum.VkW_3:
			case EvexOpCodeHandlerKindEnum.Enum.VkW_3b:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg1", handler[1]);
				WriteField(writer, "base_reg2", handler[1]);
				WriteField(writer, "code", handler[2]);
				WriteField(writer, "tuple_type", handler[3]);
				WriteField(writer, "can_broadcast", e == EvexOpCodeHandlerKindEnum.Enum.VkW_3b);
				break;
			case EvexOpCodeHandlerKindEnum.Enum.VkW_4:
			case EvexOpCodeHandlerKindEnum.Enum.VkW_4b:
				if (handler.Length != 5)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg1", handler[1]);
				WriteField(writer, "base_reg2", handler[2]);
				WriteField(writer, "code", handler[3]);
				WriteField(writer, "tuple_type", handler[4]);
				WriteField(writer, "can_broadcast", e == EvexOpCodeHandlerKindEnum.Enum.VkW_4b);
				break;
			case EvexOpCodeHandlerKindEnum.Enum.WkV_3:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg1", handler[1]);
				WriteField(writer, "base_reg2", handler[1]);
				WriteField(writer, "code", handler[2]);
				WriteField(writer, "tuple_type", handler[3]);
				WriteField(writer, "disallow_zeroing_masking", 0);
				break;
			case EvexOpCodeHandlerKindEnum.Enum.WkV_4a:
				if (handler.Length != 5)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg1", handler[1]);
				WriteField(writer, "base_reg2", handler[2]);
				WriteField(writer, "code", handler[3]);
				WriteField(writer, "tuple_type", handler[4]);
				WriteField(writer, "disallow_zeroing_masking", 0);
				break;
			case EvexOpCodeHandlerKindEnum.Enum.WkV_4b:
				if (handler.Length != 5)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg1", handler[1]);
				WriteField(writer, "base_reg2", handler[1]);
				WriteField(writer, "code", handler[2]);
				WriteField(writer, "tuple_type", handler[3]);
				WriteField(writer, "disallow_zeroing_masking", (bool)handler[4]! ? 0 : uint.MaxValue);
				break;
			case EvexOpCodeHandlerKindEnum.Enum.VkM:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code", handler[2]);
				WriteField(writer, "tuple_type", handler[3]);
				break;
			case EvexOpCodeHandlerKindEnum.Enum.VkWIb_3:
			case EvexOpCodeHandlerKindEnum.Enum.VkWIb_3b:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg1", handler[1]);
				WriteField(writer, "base_reg2", handler[1]);
				WriteField(writer, "code", handler[2]);
				WriteField(writer, "tuple_type", handler[3]);
				WriteField(writer, "can_broadcast", e == EvexOpCodeHandlerKindEnum.Enum.VkWIb_3b);
				break;
			case EvexOpCodeHandlerKindEnum.Enum.WkVIb:
				if (handler.Length != 5)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg1", handler[1]);
				WriteField(writer, "base_reg2", handler[2]);
				WriteField(writer, "code", handler[3]);
				WriteField(writer, "tuple_type", handler[4]);
				break;
			case EvexOpCodeHandlerKindEnum.Enum.HkWIb_3:
			case EvexOpCodeHandlerKindEnum.Enum.HkWIb_3b:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg1", handler[1]);
				WriteField(writer, "base_reg2", handler[1]);
				WriteField(writer, "code", handler[2]);
				WriteField(writer, "tuple_type", handler[3]);
				WriteField(writer, "can_broadcast", e == EvexOpCodeHandlerKindEnum.Enum.HkWIb_3b);
				break;
			case EvexOpCodeHandlerKindEnum.Enum.HWIb:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg1", handler[1]);
				WriteField(writer, "base_reg2", handler[1]);
				WriteField(writer, "code", handler[2]);
				WriteField(writer, "tuple_type", handler[3]);
				break;
			case EvexOpCodeHandlerKindEnum.Enum.WkVIb_er:
				if (handler.Length != 5)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg1", handler[1]);
				WriteField(writer, "base_reg2", handler[2]);
				WriteField(writer, "code", handler[3]);
				WriteField(writer, "tuple_type", handler[4]);
				break;
			case EvexOpCodeHandlerKindEnum.Enum.VW_er:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg1", handler[1]);
				WriteField(writer, "base_reg2", handler[1]);
				WriteField(writer, "code", handler[2]);
				WriteField(writer, "tuple_type", handler[3]);
				break;
			case EvexOpCodeHandlerKindEnum.Enum.VW:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg1", handler[1]);
				WriteField(writer, "base_reg2", handler[1]);
				WriteField(writer, "code", handler[2]);
				WriteField(writer, "tuple_type", handler[3]);
				break;
			case EvexOpCodeHandlerKindEnum.Enum.WV:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg1", handler[1]);
				WriteField(writer, "base_reg2", handler[1]);
				WriteField(writer, "code", handler[2]);
				WriteField(writer, "tuple_type", handler[3]);
				break;
			case EvexOpCodeHandlerKindEnum.Enum.VM:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code", handler[2]);
				WriteField(writer, "tuple_type", handler[3]);
				break;
			case EvexOpCodeHandlerKindEnum.Enum.VK:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code", handler[2]);
				break;
			case EvexOpCodeHandlerKindEnum.Enum.KR:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code", handler[2]);
				break;
			case EvexOpCodeHandlerKindEnum.Enum.KkHWIb_sae_3:
			case EvexOpCodeHandlerKindEnum.Enum.KkHWIb_sae_3b:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code", handler[2]);
				WriteField(writer, "tuple_type", handler[3]);
				WriteField(writer, "can_broadcast", e == EvexOpCodeHandlerKindEnum.Enum.KkHWIb_sae_3b);
				break;
			case EvexOpCodeHandlerKindEnum.Enum.VkHW_3:
			case EvexOpCodeHandlerKindEnum.Enum.VkHW_3b:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg1", handler[1]);
				WriteField(writer, "base_reg2", handler[1]);
				WriteField(writer, "base_reg3", handler[1]);
				WriteField(writer, "code", handler[2]);
				WriteField(writer, "tuple_type", handler[3]);
				WriteField(writer, "can_broadcast", e == EvexOpCodeHandlerKindEnum.Enum.VkHW_3b);
				break;
			case EvexOpCodeHandlerKindEnum.Enum.VkHW_5:
				if (handler.Length != 6)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg1", handler[1]);
				WriteField(writer, "base_reg2", handler[2]);
				WriteField(writer, "base_reg3", handler[3]);
				WriteField(writer, "code", handler[4]);
				WriteField(writer, "tuple_type", handler[5]);
				WriteField(writer, "can_broadcast", false);
				break;
			case EvexOpCodeHandlerKindEnum.Enum.VkHM:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg1", handler[1]);
				WriteField(writer, "base_reg2", handler[1]);
				WriteField(writer, "code", handler[2]);
				WriteField(writer, "tuple_type", handler[3]);
				break;
			case EvexOpCodeHandlerKindEnum.Enum.VkHWIb_3:
			case EvexOpCodeHandlerKindEnum.Enum.VkHWIb_3b:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg1", handler[1]);
				WriteField(writer, "base_reg2", handler[1]);
				WriteField(writer, "base_reg3", handler[1]);
				WriteField(writer, "code", handler[2]);
				WriteField(writer, "tuple_type", handler[3]);
				WriteField(writer, "can_broadcast", e == EvexOpCodeHandlerKindEnum.Enum.VkHWIb_3b);
				break;
			case EvexOpCodeHandlerKindEnum.Enum.VkHWIb_5:
				if (handler.Length != 6)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg1", handler[1]);
				WriteField(writer, "base_reg2", handler[2]);
				WriteField(writer, "base_reg3", handler[3]);
				WriteField(writer, "code", handler[4]);
				WriteField(writer, "tuple_type", handler[5]);
				WriteField(writer, "can_broadcast", false);
				break;
			case EvexOpCodeHandlerKindEnum.Enum.VkHWIb_er_4:
			case EvexOpCodeHandlerKindEnum.Enum.VkHWIb_er_4b:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg1", handler[1]);
				WriteField(writer, "base_reg2", handler[1]);
				WriteField(writer, "base_reg3", handler[1]);
				WriteField(writer, "code", handler[2]);
				WriteField(writer, "tuple_type", handler[3]);
				WriteField(writer, "can_broadcast", e == EvexOpCodeHandlerKindEnum.Enum.VkHWIb_er_4b);
				break;
			case EvexOpCodeHandlerKindEnum.Enum.KkHW_3:
			case EvexOpCodeHandlerKindEnum.Enum.KkHW_3b:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code", handler[2]);
				WriteField(writer, "tuple_type", handler[3]);
				WriteField(writer, "can_broadcast", e == EvexOpCodeHandlerKindEnum.Enum.KkHW_3b);
				break;
			case EvexOpCodeHandlerKindEnum.Enum.KP1HW:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code", handler[2]);
				WriteField(writer, "tuple_type", handler[3]);
				break;
			case EvexOpCodeHandlerKindEnum.Enum.KkHWIb_3:
			case EvexOpCodeHandlerKindEnum.Enum.KkHWIb_3b:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code", handler[2]);
				WriteField(writer, "tuple_type", handler[3]);
				WriteField(writer, "can_broadcast", e == EvexOpCodeHandlerKindEnum.Enum.KkHWIb_3b);
				break;
			case EvexOpCodeHandlerKindEnum.Enum.WkHV:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code", handler[2]);
				WriteField(writer, "tuple_type", handler[3]);
				break;
			case EvexOpCodeHandlerKindEnum.Enum.VHWIb:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code", handler[2]);
				WriteField(writer, "tuple_type", handler[3]);
				break;
			case EvexOpCodeHandlerKindEnum.Enum.VHW_3:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg1", handler[1]);
				WriteField(writer, "base_reg2", handler[1]);
				WriteField(writer, "base_reg3", handler[1]);
				WriteField(writer, "code_r", handler[2]);
				WriteField(writer, "code_m", handler[2]);
				WriteField(writer, "tuple_type", handler[3]);
				break;
			case EvexOpCodeHandlerKindEnum.Enum.VHW_4:
				if (handler.Length != 5)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg1", handler[1]);
				WriteField(writer, "base_reg2", handler[1]);
				WriteField(writer, "base_reg3", handler[1]);
				WriteField(writer, "code_r", handler[2]);
				WriteField(writer, "code_m", handler[3]);
				WriteField(writer, "tuple_type", handler[4]);
				break;
			case EvexOpCodeHandlerKindEnum.Enum.VHM:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code", handler[2]);
				WriteField(writer, "tuple_type", handler[3]);
				break;
			case EvexOpCodeHandlerKindEnum.Enum.Gv_W_er:
				if (handler.Length != 6)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code_w0", handler[2]);
				WriteField(writer, "code_w1", handler[3]);
				WriteField(writer, "tuple_type", handler[4]);
				WriteField(writer, "only_sae", handler[5]);
				break;
			case EvexOpCodeHandlerKindEnum.Enum.VX_Ev:
				if (handler.Length != 5)
					throw new InvalidOperationException();
				WriteField(writer, "code32", handler[1]);
				WriteField(writer, "code64", handler[2]);
				WriteField(writer, "tuple_type_w0", handler[3]);
				WriteField(writer, "tuple_type_w1", handler[4]);
				break;
			case EvexOpCodeHandlerKindEnum.Enum.Ev_VX:
				if (handler.Length != 5)
					throw new InvalidOperationException();
				WriteField(writer, "code32", handler[1]);
				WriteField(writer, "code64", handler[2]);
				WriteField(writer, "tuple_type_w0", handler[3]);
				WriteField(writer, "tuple_type_w1", handler[4]);
				break;
			case EvexOpCodeHandlerKindEnum.Enum.Ev_VX_Ib:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case EvexOpCodeHandlerKindEnum.Enum.MV:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code", handler[2]);
				WriteField(writer, "tuple_type", handler[3]);
				break;
			case EvexOpCodeHandlerKindEnum.Enum.VkEv_REXW_2:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", CodeEnum.Instance["INVALID"]);
				break;
			case EvexOpCodeHandlerKindEnum.Enum.VkEv_REXW_3:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case EvexOpCodeHandlerKindEnum.Enum.Vk_VSIB:
				if (handler.Length != 5)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "vsib_base", handler[2]);
				WriteField(writer, "code", handler[3]);
				WriteField(writer, "tuple_type", handler[4]);
				break;
			case EvexOpCodeHandlerKindEnum.Enum.VSIB_k1_VX:
				if (handler.Length != 5)
					throw new InvalidOperationException();
				WriteField(writer, "vsib_index", handler[1]);
				WriteField(writer, "base_reg", handler[2]);
				WriteField(writer, "code", handler[3]);
				WriteField(writer, "tuple_type", handler[4]);
				break;
			case EvexOpCodeHandlerKindEnum.Enum.VSIB_k1:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "vsib_index", handler[1]);
				WriteField(writer, "code", handler[2]);
				WriteField(writer, "tuple_type", handler[3]);
				break;
			case EvexOpCodeHandlerKindEnum.Enum.GvM_VX_Ib:
				if (handler.Length != 6)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				WriteField(writer, "tuple_type32", handler[4]);
				WriteField(writer, "tuple_type64", handler[5]);
				break;
			case EvexOpCodeHandlerKindEnum.Enum.KkWIb_3:
			case EvexOpCodeHandlerKindEnum.Enum.KkWIb_3b:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code", handler[2]);
				WriteField(writer, "tuple_type", handler[3]);
				WriteField(writer, "can_broadcast", e == EvexOpCodeHandlerKindEnum.Enum.KkWIb_3b);
				break;
			case EvexOpCodeHandlerKindEnum.Enum.Invalid2:
			case EvexOpCodeHandlerKindEnum.Enum.Dup:
			case EvexOpCodeHandlerKindEnum.Enum.HandlerReference:
			case EvexOpCodeHandlerKindEnum.Enum.ArrayReference:
			default:
				throw new InvalidOperationException();
			}
		}
	}
}
