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
	static class TupleTypeEnum {
		const string documentation = "Tuple type (EVEX) which can be used to get the disp8 scale factor N";

		static EnumValue[] GetValues() =>
			new EnumValue[] {
				new EnumValue("None", "N = 1"),
				new EnumValue("Full_128", "N = b ? (W ? 8 : 4) : 16"),
				new EnumValue("Full_256", "N = b ? (W ? 8 : 4) : 32"),
				new EnumValue("Full_512", "N = b ? (W ? 8 : 4) : 64"),
				new EnumValue("Half_128", "N = b ? 4 : 8"),
				new EnumValue("Half_256", "N = b ? 4 : 16"),
				new EnumValue("Half_512", "N = b ? 4 : 32"),
				new EnumValue("Full_Mem_128", "N = 16"),
				new EnumValue("Full_Mem_256", "N = 32"),
				new EnumValue("Full_Mem_512", "N = 64"),
				new EnumValue("Tuple1_Scalar", "N = W ? 8 : 4"),
				new EnumValue("Tuple1_Scalar_1", "N = 1"),
				new EnumValue("Tuple1_Scalar_2", "N = 2"),
				new EnumValue("Tuple1_Scalar_4", "N = 4"),
				new EnumValue("Tuple1_Scalar_8", "N = 8"),
				new EnumValue("Tuple1_Fixed_4", "N = 4"),
				new EnumValue("Tuple1_Fixed_8", "N = 8"),
				new EnumValue("Tuple2", "N = W ? 16 : 8"),
				new EnumValue("Tuple4", "N = W ? 32 : 16"),
				new EnumValue("Tuple8", "N = W ? error : 32"),
				new EnumValue("Tuple1_4X", "N = 16"),
				new EnumValue("Half_Mem_128", "N = 8"),
				new EnumValue("Half_Mem_256", "N = 16"),
				new EnumValue("Half_Mem_512", "N = 32"),
				new EnumValue("Quarter_Mem_128", "N = 4"),
				new EnumValue("Quarter_Mem_256", "N = 8"),
				new EnumValue("Quarter_Mem_512", "N = 16"),
				new EnumValue("Eighth_Mem_128", "N = 2"),
				new EnumValue("Eighth_Mem_256", "N = 4"),
				new EnumValue("Eighth_Mem_512", "N = 8"),
				new EnumValue("Mem128", "N = 16"),
				new EnumValue("MOVDDUP_128", "N = 8"),
				new EnumValue("MOVDDUP_256", "N = 32"),
				new EnumValue("MOVDDUP_512", "N = 64"),
			};

		public static readonly EnumType Instance = new EnumType(EnumKind.TupleType, documentation, GetValues(), EnumTypeFlags.Public);
	}
}
