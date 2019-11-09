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

namespace Generator.Enums {
	static class EvexOpCodeHandlerKindEnum {
		const string? documentation = null;

		static EnumValue[] GetValues() =>
			new EnumValue[] {
				new EnumValue("Invalid"),
				new EnumValue("Invalid2"),
				new EnumValue("Dup"),
				new EnumValue("HandlerReference"),
				new EnumValue("ArrayReference"),
				new EnumValue("RM"),
				new EnumValue("Group"),
				new EnumValue("W"),
				new EnumValue("MandatoryPrefix2"),
				new EnumValue("VectorLength"),
				new EnumValue("VectorLength_er"),
				new EnumValue("Ed_V_Ib"),
				new EnumValue("Ev_VX"),
				new EnumValue("Ev_VX_Ib"),
				new EnumValue("Gv_W_er"),
				new EnumValue("GvM_VX_Ib"),
				new EnumValue("HkWIb_3"),
				new EnumValue("HkWIb_3b"),
				new EnumValue("HWIb"),
				new EnumValue("KkHW_3"),
				new EnumValue("KkHW_3b"),
				new EnumValue("KkHWIb_sae_3"),
				new EnumValue("KkHWIb_sae_3b"),
				new EnumValue("KkHWIb_3"),
				new EnumValue("KkHWIb_3b"),
				new EnumValue("KkWIb_3"),
				new EnumValue("KkWIb_3b"),
				new EnumValue("KP1HW"),
				new EnumValue("KR"),
				new EnumValue("MV"),
				new EnumValue("V_H_Ev_er"),
				new EnumValue("V_H_Ev_Ib"),
				new EnumValue("VHM"),
				new EnumValue("VHW_3"),
				new EnumValue("VHW_4"),
				new EnumValue("VHWIb"),
				new EnumValue("VK"),
				new EnumValue("Vk_VSIB"),
				new EnumValue("VkEv_REXW_2"),
				new EnumValue("VkEv_REXW_3"),
				new EnumValue("VkHM"),
				new EnumValue("VkHW_3"),
				new EnumValue("VkHW_3b"),
				new EnumValue("VkHW_5"),
				new EnumValue("VkHW_er_4"),
				new EnumValue("VkHW_er_4b"),
				new EnumValue("VkHWIb_3"),
				new EnumValue("VkHWIb_3b"),
				new EnumValue("VkHWIb_5"),
				new EnumValue("VkHWIb_er_4"),
				new EnumValue("VkHWIb_er_4b"),
				new EnumValue("VkM"),
				new EnumValue("VkW_3"),
				new EnumValue("VkW_3b"),
				new EnumValue("VkW_4"),
				new EnumValue("VkW_4b"),
				new EnumValue("VkW_er_4"),
				new EnumValue("VkW_er_5"),
				new EnumValue("VkW_er_6"),
				new EnumValue("VkWIb_3"),
				new EnumValue("VkWIb_3b"),
				new EnumValue("VkWIb_er"),
				new EnumValue("VM"),
				new EnumValue("VSIB_k1"),
				new EnumValue("VSIB_k1_VX"),
				new EnumValue("VW"),
				new EnumValue("VW_er"),
				new EnumValue("VX_Ev"),
				new EnumValue("WkHV"),
				new EnumValue("WkV_3"),
				new EnumValue("WkV_4a"),
				new EnumValue("WkV_4b"),
				new EnumValue("WkVIb"),
				new EnumValue("WkVIb_er"),
				new EnumValue("WV"),
			};

		public static readonly EnumType Instance = new EnumType(TypeIds.EvexOpCodeHandlerKind, documentation, GetValues(), EnumTypeFlags.None);
	}
}
