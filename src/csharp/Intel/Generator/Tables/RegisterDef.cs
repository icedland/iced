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

using Generator.Enums;

namespace Generator.Tables {
	[Enum("RegisterKind")]
	enum RegisterKind {
		None,
		GPR8,
		GPR16,
		GPR32,
		GPR64,
		IP,
		Segment,
		ST,
		CR,
		DR,
		TR,
		BND,
		K,
		MM,
		XMM,
		YMM,
		ZMM,
		TMM,
	}

	[Enum("RegisterClass")]
	enum RegisterClass {
		None,
		GPR,
		IP,
		Segment,
		ST,
		CR,
		DR,
		TR,
		BND,
		K,
		MM,
		Vector,
		TMM,
	}

	sealed class RegisterDef {
		public readonly EnumValue Register;
		public readonly EnumValue BaseRegister;
		public readonly EnumValue FullRegister32;
		public readonly EnumValue FullRegister;
		public readonly EnumValue RegisterClass;
		public readonly EnumValue RegisterKind;
		public readonly uint Index;
		public readonly uint Size;

		public RegisterDef(EnumValue register, EnumValue baseRegister, EnumValue fullRegister32, EnumValue fullRegister, EnumValue registerClass, EnumValue registerKind, uint size) {
			Register = register;
			BaseRegister = baseRegister;
			FullRegister32 = fullRegister32;
			FullRegister = fullRegister;
			RegisterClass = registerClass;
			RegisterKind = registerKind;
			Size = size;
			Index = register.Value - baseRegister.Value;
		}
	}
}
