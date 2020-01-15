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
			return (EvexOpCodeHandlerKind)handlerType.Value switch {
				EvexOpCodeHandlerKind.Invalid => ("OpCodeHandler_Invalid", true),
				EvexOpCodeHandlerKind.RM => ("OpCodeHandler_RM", true),
				EvexOpCodeHandlerKind.Group => ("OpCodeHandler_Group", true),
				EvexOpCodeHandlerKind.W => ("OpCodeHandler_W", true),
				EvexOpCodeHandlerKind.MandatoryPrefix2 => ("OpCodeHandler_MandatoryPrefix2", true),
				EvexOpCodeHandlerKind.VectorLength => ("OpCodeHandler_VectorLength_EVEX", true),
				EvexOpCodeHandlerKind.VectorLength_er => ("OpCodeHandler_VectorLength_EVEX_er", true),
				EvexOpCodeHandlerKind.V_H_Ev_er => ("OpCodeHandler_EVEX_V_H_Ev_er", true),
				EvexOpCodeHandlerKind.V_H_Ev_Ib => ("OpCodeHandler_EVEX_V_H_Ev_Ib", true),
				EvexOpCodeHandlerKind.Ed_V_Ib => ("OpCodeHandler_EVEX_Ed_V_Ib", true),
				EvexOpCodeHandlerKind.VkHW_er_4 => ("OpCodeHandler_EVEX_VkHW_er", true),
				EvexOpCodeHandlerKind.VkHW_er_4b => ("OpCodeHandler_EVEX_VkHW_er", true),
				EvexOpCodeHandlerKind.VkW_er_4 => ("OpCodeHandler_EVEX_VkW_er", true),
				EvexOpCodeHandlerKind.VkW_er_5 => ("OpCodeHandler_EVEX_VkW_er", true),
				EvexOpCodeHandlerKind.VkW_er_6 => ("OpCodeHandler_EVEX_VkW_er", true),
				EvexOpCodeHandlerKind.VkWIb_er => ("OpCodeHandler_EVEX_VkWIb_er", true),
				EvexOpCodeHandlerKind.VkW_3 => ("OpCodeHandler_EVEX_VkW", true),
				EvexOpCodeHandlerKind.VkW_3b => ("OpCodeHandler_EVEX_VkW", true),
				EvexOpCodeHandlerKind.VkW_4 => ("OpCodeHandler_EVEX_VkW", true),
				EvexOpCodeHandlerKind.VkW_4b => ("OpCodeHandler_EVEX_VkW", true),
				EvexOpCodeHandlerKind.WkV_3 => ("OpCodeHandler_EVEX_WkV", true),
				EvexOpCodeHandlerKind.WkV_4a => ("OpCodeHandler_EVEX_WkV", true),
				EvexOpCodeHandlerKind.WkV_4b => ("OpCodeHandler_EVEX_WkV", true),
				EvexOpCodeHandlerKind.VkM => ("OpCodeHandler_EVEX_VkM", true),
				EvexOpCodeHandlerKind.VkWIb_3 => ("OpCodeHandler_EVEX_VkWIb", true),
				EvexOpCodeHandlerKind.VkWIb_3b => ("OpCodeHandler_EVEX_VkWIb", true),
				EvexOpCodeHandlerKind.WkVIb => ("OpCodeHandler_EVEX_WkVIb", true),
				EvexOpCodeHandlerKind.HkWIb_3 => ("OpCodeHandler_EVEX_HkWIb", true),
				EvexOpCodeHandlerKind.HkWIb_3b => ("OpCodeHandler_EVEX_HkWIb", true),
				EvexOpCodeHandlerKind.HWIb => ("OpCodeHandler_EVEX_HWIb", true),
				EvexOpCodeHandlerKind.WkVIb_er => ("OpCodeHandler_EVEX_WkVIb_er", true),
				EvexOpCodeHandlerKind.VW_er => ("OpCodeHandler_EVEX_VW_er", true),
				EvexOpCodeHandlerKind.VW => ("OpCodeHandler_EVEX_VW", true),
				EvexOpCodeHandlerKind.WV => ("OpCodeHandler_EVEX_WV", true),
				EvexOpCodeHandlerKind.VM => ("OpCodeHandler_EVEX_VM", true),
				EvexOpCodeHandlerKind.VK => ("OpCodeHandler_EVEX_VK", true),
				EvexOpCodeHandlerKind.KR => ("OpCodeHandler_EVEX_KR", true),
				EvexOpCodeHandlerKind.KkHWIb_sae_3 => ("OpCodeHandler_EVEX_KkHWIb_sae", true),
				EvexOpCodeHandlerKind.KkHWIb_sae_3b => ("OpCodeHandler_EVEX_KkHWIb_sae", true),
				EvexOpCodeHandlerKind.VkHW_3 => ("OpCodeHandler_EVEX_VkHW", true),
				EvexOpCodeHandlerKind.VkHW_3b => ("OpCodeHandler_EVEX_VkHW", true),
				EvexOpCodeHandlerKind.VkHW_5 => ("OpCodeHandler_EVEX_VkHW", true),
				EvexOpCodeHandlerKind.VkHM => ("OpCodeHandler_EVEX_VkHM", true),
				EvexOpCodeHandlerKind.VkHWIb_3 => ("OpCodeHandler_EVEX_VkHWIb", true),
				EvexOpCodeHandlerKind.VkHWIb_3b => ("OpCodeHandler_EVEX_VkHWIb", true),
				EvexOpCodeHandlerKind.VkHWIb_5 => ("OpCodeHandler_EVEX_VkHWIb", true),
				EvexOpCodeHandlerKind.VkHWIb_er_4 => ("OpCodeHandler_EVEX_VkHWIb_er", true),
				EvexOpCodeHandlerKind.VkHWIb_er_4b => ("OpCodeHandler_EVEX_VkHWIb_er", true),
				EvexOpCodeHandlerKind.KkHW_3 => ("OpCodeHandler_EVEX_KkHW", true),
				EvexOpCodeHandlerKind.KkHW_3b => ("OpCodeHandler_EVEX_KkHW", true),
				EvexOpCodeHandlerKind.KP1HW => ("OpCodeHandler_EVEX_KP1HW", true),
				EvexOpCodeHandlerKind.KkHWIb_3 => ("OpCodeHandler_EVEX_KkHWIb", true),
				EvexOpCodeHandlerKind.KkHWIb_3b => ("OpCodeHandler_EVEX_KkHWIb", true),
				EvexOpCodeHandlerKind.WkHV => ("OpCodeHandler_EVEX_WkHV", true),
				EvexOpCodeHandlerKind.VHWIb => ("OpCodeHandler_EVEX_VHWIb", true),
				EvexOpCodeHandlerKind.VHW_3 => ("OpCodeHandler_EVEX_VHW", true),
				EvexOpCodeHandlerKind.VHW_4 => ("OpCodeHandler_EVEX_VHW", true),
				EvexOpCodeHandlerKind.VHM => ("OpCodeHandler_EVEX_VHM", true),
				EvexOpCodeHandlerKind.Gv_W_er => ("OpCodeHandler_EVEX_Gv_W_er", true),
				EvexOpCodeHandlerKind.VX_Ev => ("OpCodeHandler_EVEX_VX_Ev", true),
				EvexOpCodeHandlerKind.Ev_VX => ("OpCodeHandler_EVEX_Ev_VX", true),
				EvexOpCodeHandlerKind.Ev_VX_Ib => ("OpCodeHandler_EVEX_Ev_VX_Ib", true),
				EvexOpCodeHandlerKind.MV => ("OpCodeHandler_EVEX_MV", true),
				EvexOpCodeHandlerKind.VkEv_REXW_2 => ("OpCodeHandler_EVEX_VkEv_REXW", true),
				EvexOpCodeHandlerKind.VkEv_REXW_3 => ("OpCodeHandler_EVEX_VkEv_REXW", true),
				EvexOpCodeHandlerKind.Vk_VSIB => ("OpCodeHandler_EVEX_Vk_VSIB", true),
				EvexOpCodeHandlerKind.VSIB_k1_VX => ("OpCodeHandler_EVEX_VSIB_k1_VX", true),
				EvexOpCodeHandlerKind.VSIB_k1 => ("OpCodeHandler_EVEX_VSIB_k1", true),
				EvexOpCodeHandlerKind.GvM_VX_Ib => ("OpCodeHandler_EVEX_GvM_VX_Ib", true),
				EvexOpCodeHandlerKind.KkWIb_3 => ("OpCodeHandler_EVEX_KkWIb", true),
				EvexOpCodeHandlerKind.KkWIb_3b => ("OpCodeHandler_EVEX_KkWIb", true),
				_ => throw new InvalidOperationException(),
			};
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
