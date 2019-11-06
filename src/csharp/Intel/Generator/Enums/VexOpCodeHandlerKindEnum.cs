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
	static class VexOpCodeHandlerKindEnum {
		const string? documentation = null;

		static EnumValue[] GetValues() =>
			new EnumValue[] {
				new EnumValue("Invalid"),
				new EnumValue("Invalid2"),
				new EnumValue("Dup"),
				new EnumValue("Invalid_NoModRM"),
				new EnumValue("Bitness_DontReadModRM"),
				new EnumValue("HandlerReference"),
				new EnumValue("ArrayReference"),
				new EnumValue("RM"),
				new EnumValue("Group"),
				new EnumValue("W"),
				new EnumValue("MandatoryPrefix2_1"),
				new EnumValue("MandatoryPrefix2_4"),
				new EnumValue("MandatoryPrefix2_NoModRM"),
				new EnumValue("VectorLength_NoModRM"),
				new EnumValue("VectorLength"),
				new EnumValue("Ed_V_Ib"),
				new EnumValue("Ev_VX"),
				new EnumValue("G_VK"),
				new EnumValue("Gv_Ev_Gv"),
				new EnumValue("Gv_Ev_Ib"),
				new EnumValue("Gv_Ev_Id"),
				new EnumValue("Gv_GPR_Ib"),
				new EnumValue("Gv_Gv_Ev"),
				new EnumValue("Gv_RX"),
				new EnumValue("Gv_W"),
				new EnumValue("GvM_VX_Ib"),
				new EnumValue("HRIb"),
				new EnumValue("Hv_Ed_Id"),
				new EnumValue("Hv_Ev"),
				new EnumValue("M"),
				new EnumValue("MHV"),
				new EnumValue("M_VK"),
				new EnumValue("MV"),
				new EnumValue("rDI_VX_RX"),
				new EnumValue("RdRq"),
				new EnumValue("Simple"),
				new EnumValue("VHEv"),
				new EnumValue("VHEvIb"),
				new EnumValue("VHIs4W"),
				new EnumValue("VHIs5W"),
				new EnumValue("VHM"),
				new EnumValue("VHW_2"),
				new EnumValue("VHW_3"),
				new EnumValue("VHW_4"),
				new EnumValue("VHWIb_2"),
				new EnumValue("VHWIb_4"),
				new EnumValue("VHWIs4"),
				new EnumValue("VHWIs5"),
				new EnumValue("VK_HK_RK"),
				new EnumValue("VK_R"),
				new EnumValue("VK_RK"),
				new EnumValue("VK_RK_Ib"),
				new EnumValue("VK_WK"),
				new EnumValue("VM"),
				new EnumValue("VW_2"),
				new EnumValue("VW_3"),
				new EnumValue("VWH"),
				new EnumValue("VWIb_2"),
				new EnumValue("VWIb_3"),
				new EnumValue("VX_Ev"),
				new EnumValue("VX_VSIB_HX"),
				new EnumValue("WHV"),
				new EnumValue("WV"),
				new EnumValue("WVIb"),
			};

		public static readonly EnumType Instance = new EnumType(EnumKind.VexOpCodeHandlerKind, documentation, GetValues(), EnumTypeFlags.None);
	}
}
