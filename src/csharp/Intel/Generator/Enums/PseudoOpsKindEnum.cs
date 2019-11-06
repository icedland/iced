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
	static class PseudoOpsKindEnum {
		const string? documentation = null;

		static EnumValue[] GetValues() =>
			new EnumValue[] {
				new EnumValue("cmpps"),
				new EnumValue("vcmpps"),
				new EnumValue("cmppd"),
				new EnumValue("vcmppd"),
				new EnumValue("cmpss"),
				new EnumValue("vcmpss"),
				new EnumValue("cmpsd"),
				new EnumValue("vcmpsd"),
				new EnumValue("pclmulqdq"),
				new EnumValue("vpclmulqdq"),
				new EnumValue("vpcomb"),
				new EnumValue("vpcomw"),
				new EnumValue("vpcomd"),
				new EnumValue("vpcomq"),
				new EnumValue("vpcomub"),
				new EnumValue("vpcomuw"),
				new EnumValue("vpcomud"),
				new EnumValue("vpcomuq"),
			};

		public static readonly EnumType Instance = new EnumType(EnumKind.PseudoOpsKind, documentation, GetValues(), EnumTypeFlags.None);
	}
}
