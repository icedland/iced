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
	static class RoundingControlEnum {
		const string documentation = "Rounding control";

		static EnumValue[] GetValues() =>
			new EnumValue[] {
				new EnumValue("None", "No rounding mode"),
				new EnumValue("RoundToNearest", "Round to nearest (even)"),
				new EnumValue("RoundDown", "Round down (toward -inf)"),
				new EnumValue("RoundUp", "Round up (toward +inf)"),
				new EnumValue("RoundTowardZero", "Round toward zero (truncate)"),
			};

		public static readonly EnumType Instance = new EnumType(TypeIds.RoundingControl, documentation, GetValues(), EnumTypeFlags.Public);
	}
}
