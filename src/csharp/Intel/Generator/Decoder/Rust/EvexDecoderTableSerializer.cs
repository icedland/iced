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
			switch ((EvexOpCodeHandlerKind)handlerType.Value) {
			case EvexOpCodeHandlerKind.Invalid: return ("OpCodeHandler_Invalid", true);
			case EvexOpCodeHandlerKind.RM: return ("OpCodeHandler_RM", true);
			case EvexOpCodeHandlerKind.Group: return ("OpCodeHandler_Group", true);
			case EvexOpCodeHandlerKind.W: return ("OpCodeHandler_W", true);
			case EvexOpCodeHandlerKind.MandatoryPrefix2: return ("OpCodeHandler_MandatoryPrefix2", true);
			case EvexOpCodeHandlerKind.VectorLength: return ("OpCodeHandler_VectorLength_EVEX", true);
			case EvexOpCodeHandlerKind.VectorLength_er: return ("OpCodeHandler_VectorLength_EVEX_er", true);
			case EvexOpCodeHandlerKind.V_H_Ev_er: return ("OpCodeHandler_EVEX_V_H_Ev_er", true);
			case EvexOpCodeHandlerKind.V_H_Ev_Ib: return ("OpCodeHandler_EVEX_V_H_Ev_Ib", true);
			case EvexOpCodeHandlerKind.Ed_V_Ib: return ("OpCodeHandler_EVEX_Ed_V_Ib", true);
			case EvexOpCodeHandlerKind.VkHW_er_4: return ("OpCodeHandler_EVEX_VkHW_er", true);
			case EvexOpCodeHandlerKind.VkHW_er_4b: return ("OpCodeHandler_EVEX_VkHW_er", true);
			case EvexOpCodeHandlerKind.VkW_er_4: return ("OpCodeHandler_EVEX_VkW_er", true);
			case EvexOpCodeHandlerKind.VkW_er_5: return ("OpCodeHandler_EVEX_VkW_er", true);
			case EvexOpCodeHandlerKind.VkW_er_6: return ("OpCodeHandler_EVEX_VkW_er", true);
			case EvexOpCodeHandlerKind.VkWIb_er: return ("OpCodeHandler_EVEX_VkWIb_er", true);
			case EvexOpCodeHandlerKind.VkW_3: return ("OpCodeHandler_EVEX_VkW", true);
			case EvexOpCodeHandlerKind.VkW_3b: return ("OpCodeHandler_EVEX_VkW", true);
			case EvexOpCodeHandlerKind.VkW_4: return ("OpCodeHandler_EVEX_VkW", true);
			case EvexOpCodeHandlerKind.VkW_4b: return ("OpCodeHandler_EVEX_VkW", true);
			case EvexOpCodeHandlerKind.WkV_3: return ("OpCodeHandler_EVEX_WkV", true);
			case EvexOpCodeHandlerKind.WkV_4a: return ("OpCodeHandler_EVEX_WkV", true);
			case EvexOpCodeHandlerKind.WkV_4b: return ("OpCodeHandler_EVEX_WkV", true);
			case EvexOpCodeHandlerKind.VkM: return ("OpCodeHandler_EVEX_VkM", true);
			case EvexOpCodeHandlerKind.VkWIb_3: return ("OpCodeHandler_EVEX_VkWIb", true);
			case EvexOpCodeHandlerKind.VkWIb_3b: return ("OpCodeHandler_EVEX_VkWIb", true);
			case EvexOpCodeHandlerKind.WkVIb: return ("OpCodeHandler_EVEX_WkVIb", true);
			case EvexOpCodeHandlerKind.HkWIb_3: return ("OpCodeHandler_EVEX_HkWIb", true);
			case EvexOpCodeHandlerKind.HkWIb_3b: return ("OpCodeHandler_EVEX_HkWIb", true);
			case EvexOpCodeHandlerKind.HWIb: return ("OpCodeHandler_EVEX_HWIb", true);
			case EvexOpCodeHandlerKind.WkVIb_er: return ("OpCodeHandler_EVEX_WkVIb_er", true);
			case EvexOpCodeHandlerKind.VW_er: return ("OpCodeHandler_EVEX_VW_er", true);
			case EvexOpCodeHandlerKind.VW: return ("OpCodeHandler_EVEX_VW", true);
			case EvexOpCodeHandlerKind.WV: return ("OpCodeHandler_EVEX_WV", true);
			case EvexOpCodeHandlerKind.VM: return ("OpCodeHandler_EVEX_VM", true);
			case EvexOpCodeHandlerKind.VK: return ("OpCodeHandler_EVEX_VK", true);
			case EvexOpCodeHandlerKind.KR: return ("OpCodeHandler_EVEX_KR", true);
			case EvexOpCodeHandlerKind.KkHWIb_sae_3: return ("OpCodeHandler_EVEX_KkHWIb_sae", true);
			case EvexOpCodeHandlerKind.KkHWIb_sae_3b: return ("OpCodeHandler_EVEX_KkHWIb_sae", true);
			case EvexOpCodeHandlerKind.VkHW_3: return ("OpCodeHandler_EVEX_VkHW", true);
			case EvexOpCodeHandlerKind.VkHW_3b: return ("OpCodeHandler_EVEX_VkHW", true);
			case EvexOpCodeHandlerKind.VkHW_5: return ("OpCodeHandler_EVEX_VkHW", true);
			case EvexOpCodeHandlerKind.VkHM: return ("OpCodeHandler_EVEX_VkHM", true);
			case EvexOpCodeHandlerKind.VkHWIb_3: return ("OpCodeHandler_EVEX_VkHWIb", true);
			case EvexOpCodeHandlerKind.VkHWIb_3b: return ("OpCodeHandler_EVEX_VkHWIb", true);
			case EvexOpCodeHandlerKind.VkHWIb_5: return ("OpCodeHandler_EVEX_VkHWIb", true);
			case EvexOpCodeHandlerKind.VkHWIb_er_4: return ("OpCodeHandler_EVEX_VkHWIb_er", true);
			case EvexOpCodeHandlerKind.VkHWIb_er_4b: return ("OpCodeHandler_EVEX_VkHWIb_er", true);
			case EvexOpCodeHandlerKind.KkHW_3: return ("OpCodeHandler_EVEX_KkHW", true);
			case EvexOpCodeHandlerKind.KkHW_3b: return ("OpCodeHandler_EVEX_KkHW", true);
			case EvexOpCodeHandlerKind.KP1HW: return ("OpCodeHandler_EVEX_KP1HW", true);
			case EvexOpCodeHandlerKind.KkHWIb_3: return ("OpCodeHandler_EVEX_KkHWIb", true);
			case EvexOpCodeHandlerKind.KkHWIb_3b: return ("OpCodeHandler_EVEX_KkHWIb", true);
			case EvexOpCodeHandlerKind.WkHV: return ("OpCodeHandler_EVEX_WkHV", true);
			case EvexOpCodeHandlerKind.VHWIb: return ("OpCodeHandler_EVEX_VHWIb", true);
			case EvexOpCodeHandlerKind.VHW_3: return ("OpCodeHandler_EVEX_VHW", true);
			case EvexOpCodeHandlerKind.VHW_4: return ("OpCodeHandler_EVEX_VHW", true);
			case EvexOpCodeHandlerKind.VHM: return ("OpCodeHandler_EVEX_VHM", true);
			case EvexOpCodeHandlerKind.Gv_W_er: return ("OpCodeHandler_EVEX_Gv_W_er", true);
			case EvexOpCodeHandlerKind.VX_Ev: return ("OpCodeHandler_EVEX_VX_Ev", true);
			case EvexOpCodeHandlerKind.Ev_VX: return ("OpCodeHandler_EVEX_Ev_VX", true);
			case EvexOpCodeHandlerKind.Ev_VX_Ib: return ("OpCodeHandler_EVEX_Ev_VX_Ib", true);
			case EvexOpCodeHandlerKind.MV: return ("OpCodeHandler_EVEX_MV", true);
			case EvexOpCodeHandlerKind.VkEv_REXW_2: return ("OpCodeHandler_EVEX_VkEv_REXW", true);
			case EvexOpCodeHandlerKind.VkEv_REXW_3: return ("OpCodeHandler_EVEX_VkEv_REXW", true);
			case EvexOpCodeHandlerKind.Vk_VSIB: return ("OpCodeHandler_EVEX_Vk_VSIB", true);
			case EvexOpCodeHandlerKind.VSIB_k1_VX: return ("OpCodeHandler_EVEX_VSIB_k1_VX", true);
			case EvexOpCodeHandlerKind.VSIB_k1: return ("OpCodeHandler_EVEX_VSIB_k1", true);
			case EvexOpCodeHandlerKind.GvM_VX_Ib: return ("OpCodeHandler_EVEX_GvM_VX_Ib", true);
			case EvexOpCodeHandlerKind.KkWIb_3: return ("OpCodeHandler_EVEX_KkWIb", true);
			case EvexOpCodeHandlerKind.KkWIb_3b: return ("OpCodeHandler_EVEX_KkWIb", true);
			case EvexOpCodeHandlerKind.Invalid2:
			case EvexOpCodeHandlerKind.Dup:
			case EvexOpCodeHandlerKind.HandlerReference:
			case EvexOpCodeHandlerKind.ArrayReference:
			default: throw new InvalidOperationException();
			}
		}

		protected sealed override void WriteFieldsCore(FileWriter writer, object?[] handler) {
			if (!HandlerUtils.IsHandler(handler, out var enumValue))
				throw new InvalidOperationException();
			var typeInfo = GetHandlerTypeInfo(enumValue);
			WriteFirstFields(writer, typeInfo.name, typeInfo.hasModRM);

			var e = (EvexOpCodeHandlerKind)enumValue.Value;
			switch (e) {
			case EvexOpCodeHandlerKind.Invalid:
				if (handler.Length != 1)
					throw new InvalidOperationException();
				break;
			case EvexOpCodeHandlerKind.RM:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "reg", handler[1]);
				WriteField(writer, "mem", handler[2]);
				break;
			case EvexOpCodeHandlerKind.Group:
				if (handler.Length != 2)
					throw new InvalidOperationException();
				WriteField(writer, "group_handlers", VerifyArray(8, handler[1]));
				break;
			case EvexOpCodeHandlerKind.W:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "handlers", new object?[] { handler[1], handler[2] });
				break;
			case EvexOpCodeHandlerKind.MandatoryPrefix2:
				if (handler.Length != 5)
					throw new InvalidOperationException();
				WriteField(writer, "handlers", new object?[] { handler[1], handler[2], handler[3], handler[4] });
				break;
			case EvexOpCodeHandlerKind.VectorLength:
			case EvexOpCodeHandlerKind.VectorLength_er:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "handlers", new object?[] { handler[1], handler[2], handler[3], GetInvalid() });
				break;
			case EvexOpCodeHandlerKind.V_H_Ev_er:
				if (handler.Length != 6)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code_w0", handler[2]);
				WriteField(writer, "code_w1", handler[3]);
				WriteField(writer, "tuple_type_w0", handler[4]);
				WriteField(writer, "tuple_type_w1", handler[5]);
				break;
			case EvexOpCodeHandlerKind.V_H_Ev_Ib:
				if (handler.Length != 6)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code_w0", handler[2]);
				WriteField(writer, "code_w1", handler[3]);
				WriteField(writer, "tuple_type_w0", handler[4]);
				WriteField(writer, "tuple_type_w1", handler[5]);
				break;
			case EvexOpCodeHandlerKind.Ed_V_Ib:
				if (handler.Length != 6)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				WriteField(writer, "tuple_type32", handler[4]);
				WriteField(writer, "tuple_type64", handler[5]);
				break;
			case EvexOpCodeHandlerKind.VkHW_er_4:
			case EvexOpCodeHandlerKind.VkHW_er_4b:
				if (handler.Length != 5)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code", handler[2]);
				WriteField(writer, "tuple_type", handler[3]);
				WriteField(writer, "only_sae", handler[4]);
				WriteField(writer, "can_broadcast", e == EvexOpCodeHandlerKind.VkHW_er_4b);
				break;
			case EvexOpCodeHandlerKind.VkW_er_4:
				if (handler.Length != 5)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg1", handler[1]);
				WriteField(writer, "base_reg2", handler[1]);
				WriteField(writer, "code", handler[2]);
				WriteField(writer, "tuple_type", handler[3]);
				WriteField(writer, "only_sae", handler[4]);
				WriteField(writer, "can_broadcast", true);
				break;
			case EvexOpCodeHandlerKind.VkW_er_5:
				if (handler.Length != 6)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg1", handler[1]);
				WriteField(writer, "base_reg2", handler[2]);
				WriteField(writer, "code", handler[3]);
				WriteField(writer, "tuple_type", handler[4]);
				WriteField(writer, "only_sae", handler[5]);
				WriteField(writer, "can_broadcast", true);
				break;
			case EvexOpCodeHandlerKind.VkW_er_6:
				if (handler.Length != 7)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg1", handler[1]);
				WriteField(writer, "base_reg2", handler[2]);
				WriteField(writer, "code", handler[3]);
				WriteField(writer, "tuple_type", handler[4]);
				WriteField(writer, "only_sae", handler[5]);
				WriteField(writer, "can_broadcast", handler[6]);
				break;
			case EvexOpCodeHandlerKind.VkWIb_er:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg1", handler[1]);
				WriteField(writer, "base_reg2", handler[1]);
				WriteField(writer, "code", handler[2]);
				WriteField(writer, "tuple_type", handler[3]);
				break;
			case EvexOpCodeHandlerKind.VkW_3:
			case EvexOpCodeHandlerKind.VkW_3b:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg1", handler[1]);
				WriteField(writer, "base_reg2", handler[1]);
				WriteField(writer, "code", handler[2]);
				WriteField(writer, "tuple_type", handler[3]);
				WriteField(writer, "can_broadcast", e == EvexOpCodeHandlerKind.VkW_3b);
				break;
			case EvexOpCodeHandlerKind.VkW_4:
			case EvexOpCodeHandlerKind.VkW_4b:
				if (handler.Length != 5)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg1", handler[1]);
				WriteField(writer, "base_reg2", handler[2]);
				WriteField(writer, "code", handler[3]);
				WriteField(writer, "tuple_type", handler[4]);
				WriteField(writer, "can_broadcast", e == EvexOpCodeHandlerKind.VkW_4b);
				break;
			case EvexOpCodeHandlerKind.WkV_3:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg1", handler[1]);
				WriteField(writer, "base_reg2", handler[1]);
				WriteField(writer, "code", handler[2]);
				WriteField(writer, "tuple_type", handler[3]);
				WriteField(writer, "disallow_zeroing_masking", 0);
				break;
			case EvexOpCodeHandlerKind.WkV_4a:
				if (handler.Length != 5)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg1", handler[1]);
				WriteField(writer, "base_reg2", handler[2]);
				WriteField(writer, "code", handler[3]);
				WriteField(writer, "tuple_type", handler[4]);
				WriteField(writer, "disallow_zeroing_masking", 0);
				break;
			case EvexOpCodeHandlerKind.WkV_4b:
				if (handler.Length != 5)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg1", handler[1]);
				WriteField(writer, "base_reg2", handler[1]);
				WriteField(writer, "code", handler[2]);
				WriteField(writer, "tuple_type", handler[3]);
				WriteField(writer, "disallow_zeroing_masking", (bool)handler[4]! ? 0 : uint.MaxValue);
				break;
			case EvexOpCodeHandlerKind.VkM:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code", handler[2]);
				WriteField(writer, "tuple_type", handler[3]);
				break;
			case EvexOpCodeHandlerKind.VkWIb_3:
			case EvexOpCodeHandlerKind.VkWIb_3b:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg1", handler[1]);
				WriteField(writer, "base_reg2", handler[1]);
				WriteField(writer, "code", handler[2]);
				WriteField(writer, "tuple_type", handler[3]);
				WriteField(writer, "can_broadcast", e == EvexOpCodeHandlerKind.VkWIb_3b);
				break;
			case EvexOpCodeHandlerKind.WkVIb:
				if (handler.Length != 5)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg1", handler[1]);
				WriteField(writer, "base_reg2", handler[2]);
				WriteField(writer, "code", handler[3]);
				WriteField(writer, "tuple_type", handler[4]);
				break;
			case EvexOpCodeHandlerKind.HkWIb_3:
			case EvexOpCodeHandlerKind.HkWIb_3b:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg1", handler[1]);
				WriteField(writer, "base_reg2", handler[1]);
				WriteField(writer, "code", handler[2]);
				WriteField(writer, "tuple_type", handler[3]);
				WriteField(writer, "can_broadcast", e == EvexOpCodeHandlerKind.HkWIb_3b);
				break;
			case EvexOpCodeHandlerKind.HWIb:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg1", handler[1]);
				WriteField(writer, "base_reg2", handler[1]);
				WriteField(writer, "code", handler[2]);
				WriteField(writer, "tuple_type", handler[3]);
				break;
			case EvexOpCodeHandlerKind.WkVIb_er:
				if (handler.Length != 5)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg1", handler[1]);
				WriteField(writer, "base_reg2", handler[2]);
				WriteField(writer, "code", handler[3]);
				WriteField(writer, "tuple_type", handler[4]);
				break;
			case EvexOpCodeHandlerKind.VW_er:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg1", handler[1]);
				WriteField(writer, "base_reg2", handler[1]);
				WriteField(writer, "code", handler[2]);
				WriteField(writer, "tuple_type", handler[3]);
				break;
			case EvexOpCodeHandlerKind.VW:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg1", handler[1]);
				WriteField(writer, "base_reg2", handler[1]);
				WriteField(writer, "code", handler[2]);
				WriteField(writer, "tuple_type", handler[3]);
				break;
			case EvexOpCodeHandlerKind.WV:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg1", handler[1]);
				WriteField(writer, "base_reg2", handler[1]);
				WriteField(writer, "code", handler[2]);
				WriteField(writer, "tuple_type", handler[3]);
				break;
			case EvexOpCodeHandlerKind.VM:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code", handler[2]);
				WriteField(writer, "tuple_type", handler[3]);
				break;
			case EvexOpCodeHandlerKind.VK:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code", handler[2]);
				break;
			case EvexOpCodeHandlerKind.KR:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code", handler[2]);
				break;
			case EvexOpCodeHandlerKind.KkHWIb_sae_3:
			case EvexOpCodeHandlerKind.KkHWIb_sae_3b:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code", handler[2]);
				WriteField(writer, "tuple_type", handler[3]);
				WriteField(writer, "can_broadcast", e == EvexOpCodeHandlerKind.KkHWIb_sae_3b);
				break;
			case EvexOpCodeHandlerKind.VkHW_3:
			case EvexOpCodeHandlerKind.VkHW_3b:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg1", handler[1]);
				WriteField(writer, "base_reg2", handler[1]);
				WriteField(writer, "base_reg3", handler[1]);
				WriteField(writer, "code", handler[2]);
				WriteField(writer, "tuple_type", handler[3]);
				WriteField(writer, "can_broadcast", e == EvexOpCodeHandlerKind.VkHW_3b);
				break;
			case EvexOpCodeHandlerKind.VkHW_5:
				if (handler.Length != 6)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg1", handler[1]);
				WriteField(writer, "base_reg2", handler[2]);
				WriteField(writer, "base_reg3", handler[3]);
				WriteField(writer, "code", handler[4]);
				WriteField(writer, "tuple_type", handler[5]);
				WriteField(writer, "can_broadcast", false);
				break;
			case EvexOpCodeHandlerKind.VkHM:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg1", handler[1]);
				WriteField(writer, "base_reg2", handler[1]);
				WriteField(writer, "code", handler[2]);
				WriteField(writer, "tuple_type", handler[3]);
				break;
			case EvexOpCodeHandlerKind.VkHWIb_3:
			case EvexOpCodeHandlerKind.VkHWIb_3b:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg1", handler[1]);
				WriteField(writer, "base_reg2", handler[1]);
				WriteField(writer, "base_reg3", handler[1]);
				WriteField(writer, "code", handler[2]);
				WriteField(writer, "tuple_type", handler[3]);
				WriteField(writer, "can_broadcast", e == EvexOpCodeHandlerKind.VkHWIb_3b);
				break;
			case EvexOpCodeHandlerKind.VkHWIb_5:
				if (handler.Length != 6)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg1", handler[1]);
				WriteField(writer, "base_reg2", handler[2]);
				WriteField(writer, "base_reg3", handler[3]);
				WriteField(writer, "code", handler[4]);
				WriteField(writer, "tuple_type", handler[5]);
				WriteField(writer, "can_broadcast", false);
				break;
			case EvexOpCodeHandlerKind.VkHWIb_er_4:
			case EvexOpCodeHandlerKind.VkHWIb_er_4b:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg1", handler[1]);
				WriteField(writer, "base_reg2", handler[1]);
				WriteField(writer, "base_reg3", handler[1]);
				WriteField(writer, "code", handler[2]);
				WriteField(writer, "tuple_type", handler[3]);
				WriteField(writer, "can_broadcast", e == EvexOpCodeHandlerKind.VkHWIb_er_4b);
				break;
			case EvexOpCodeHandlerKind.KkHW_3:
			case EvexOpCodeHandlerKind.KkHW_3b:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code", handler[2]);
				WriteField(writer, "tuple_type", handler[3]);
				WriteField(writer, "can_broadcast", e == EvexOpCodeHandlerKind.KkHW_3b);
				break;
			case EvexOpCodeHandlerKind.KP1HW:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code", handler[2]);
				WriteField(writer, "tuple_type", handler[3]);
				break;
			case EvexOpCodeHandlerKind.KkHWIb_3:
			case EvexOpCodeHandlerKind.KkHWIb_3b:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code", handler[2]);
				WriteField(writer, "tuple_type", handler[3]);
				WriteField(writer, "can_broadcast", e == EvexOpCodeHandlerKind.KkHWIb_3b);
				break;
			case EvexOpCodeHandlerKind.WkHV:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code", handler[2]);
				WriteField(writer, "tuple_type", handler[3]);
				break;
			case EvexOpCodeHandlerKind.VHWIb:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code", handler[2]);
				WriteField(writer, "tuple_type", handler[3]);
				break;
			case EvexOpCodeHandlerKind.VHW_3:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg1", handler[1]);
				WriteField(writer, "base_reg2", handler[1]);
				WriteField(writer, "base_reg3", handler[1]);
				WriteField(writer, "code_r", handler[2]);
				WriteField(writer, "code_m", handler[2]);
				WriteField(writer, "tuple_type", handler[3]);
				break;
			case EvexOpCodeHandlerKind.VHW_4:
				if (handler.Length != 5)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg1", handler[1]);
				WriteField(writer, "base_reg2", handler[1]);
				WriteField(writer, "base_reg3", handler[1]);
				WriteField(writer, "code_r", handler[2]);
				WriteField(writer, "code_m", handler[3]);
				WriteField(writer, "tuple_type", handler[4]);
				break;
			case EvexOpCodeHandlerKind.VHM:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code", handler[2]);
				WriteField(writer, "tuple_type", handler[3]);
				break;
			case EvexOpCodeHandlerKind.Gv_W_er:
				if (handler.Length != 6)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code_w0", handler[2]);
				WriteField(writer, "code_w1", handler[3]);
				WriteField(writer, "tuple_type", handler[4]);
				WriteField(writer, "only_sae", handler[5]);
				break;
			case EvexOpCodeHandlerKind.VX_Ev:
				if (handler.Length != 5)
					throw new InvalidOperationException();
				WriteField(writer, "code32", handler[1]);
				WriteField(writer, "code64", handler[2]);
				WriteField(writer, "tuple_type_w0", handler[3]);
				WriteField(writer, "tuple_type_w1", handler[4]);
				break;
			case EvexOpCodeHandlerKind.Ev_VX:
				if (handler.Length != 5)
					throw new InvalidOperationException();
				WriteField(writer, "code32", handler[1]);
				WriteField(writer, "code64", handler[2]);
				WriteField(writer, "tuple_type_w0", handler[3]);
				WriteField(writer, "tuple_type_w1", handler[4]);
				break;
			case EvexOpCodeHandlerKind.Ev_VX_Ib:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case EvexOpCodeHandlerKind.MV:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code", handler[2]);
				WriteField(writer, "tuple_type", handler[3]);
				break;
			case EvexOpCodeHandlerKind.VkEv_REXW_2:
				if (handler.Length != 3)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", CodeEnum.Instance[nameof(Code.INVALID)]);
				break;
			case EvexOpCodeHandlerKind.VkEv_REXW_3:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				break;
			case EvexOpCodeHandlerKind.Vk_VSIB:
				if (handler.Length != 5)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "vsib_base", handler[2]);
				WriteField(writer, "code", handler[3]);
				WriteField(writer, "tuple_type", handler[4]);
				break;
			case EvexOpCodeHandlerKind.VSIB_k1_VX:
				if (handler.Length != 5)
					throw new InvalidOperationException();
				WriteField(writer, "vsib_index", handler[1]);
				WriteField(writer, "base_reg", handler[2]);
				WriteField(writer, "code", handler[3]);
				WriteField(writer, "tuple_type", handler[4]);
				break;
			case EvexOpCodeHandlerKind.VSIB_k1:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "vsib_index", handler[1]);
				WriteField(writer, "code", handler[2]);
				WriteField(writer, "tuple_type", handler[3]);
				break;
			case EvexOpCodeHandlerKind.GvM_VX_Ib:
				if (handler.Length != 6)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code32", handler[2]);
				WriteField(writer, "code64", handler[3]);
				WriteField(writer, "tuple_type32", handler[4]);
				WriteField(writer, "tuple_type64", handler[5]);
				break;
			case EvexOpCodeHandlerKind.KkWIb_3:
			case EvexOpCodeHandlerKind.KkWIb_3b:
				if (handler.Length != 4)
					throw new InvalidOperationException();
				WriteField(writer, "base_reg", handler[1]);
				WriteField(writer, "code", handler[2]);
				WriteField(writer, "tuple_type", handler[3]);
				WriteField(writer, "can_broadcast", e == EvexOpCodeHandlerKind.KkWIb_3b);
				break;
			case EvexOpCodeHandlerKind.Invalid2:
			case EvexOpCodeHandlerKind.Dup:
			case EvexOpCodeHandlerKind.HandlerReference:
			case EvexOpCodeHandlerKind.ArrayReference:
			default:
				throw new InvalidOperationException();
			}
		}
	}
}
