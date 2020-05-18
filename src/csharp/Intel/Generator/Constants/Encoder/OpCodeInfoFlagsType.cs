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

namespace Generator.Constants.Encoder {
	[TypeGen(TypeGenOrders.NoDeps)]
	sealed class OpCodeInfoFlagsType {
		OpCodeInfoFlagsType(GenTypes genTypes) {
			var type = new ConstantsType(TypeIds.OpCodeInfoFlags, ConstantsTypeFlags.None, null, GetConstants());
			genTypes.Add(type);
		}

		static Constant[] GetConstants() =>
			new Constant[] {
				new Constant(ConstantKind.String, "NotInstruction", "notinstr"),
				new Constant(ConstantKind.String, "Bit16", "16b"),
				new Constant(ConstantKind.String, "Bit32", "32b"),
				new Constant(ConstantKind.String, "Bit64", "64b"),
				new Constant(ConstantKind.String, "Fwait", "fwait"),
				new Constant(ConstantKind.String, "OperandSize16", "o16"),
				new Constant(ConstantKind.String, "OperandSize32", "o32"),
				new Constant(ConstantKind.String, "OperandSize64", "o64"),
				new Constant(ConstantKind.String, "AddressSize16", "a16"),
				new Constant(ConstantKind.String, "AddressSize32", "a32"),
				new Constant(ConstantKind.String, "AddressSize64", "a64"),
				new Constant(ConstantKind.String, "LIG", "LIG"),
				new Constant(ConstantKind.String, "L0", "L0"),
				new Constant(ConstantKind.String, "L1", "L1"),
				new Constant(ConstantKind.String, "L128", "L128"),
				new Constant(ConstantKind.String, "L256", "L256"),
				new Constant(ConstantKind.String, "L512", "L512"),
				new Constant(ConstantKind.String, "WIG", "WIG"),
				new Constant(ConstantKind.String, "WIG32", "WIG32"),
				new Constant(ConstantKind.String, "W0", "W0"),
				new Constant(ConstantKind.String, "W1", "W1"),
				new Constant(ConstantKind.String, "Broadcast", "b"),
				new Constant(ConstantKind.String, "RoundingControl", "er"),
				new Constant(ConstantKind.String, "SuppressAllExceptions", "sae"),
				new Constant(ConstantKind.String, "OpMaskRegister", "k"),
				new Constant(ConstantKind.String, "RequireNonZeroOpMaskRegister", "knz"),
				new Constant(ConstantKind.String, "ZeroingMasking", "z"),
				new Constant(ConstantKind.String, "LockPrefix", "lock"),
				new Constant(ConstantKind.String, "XacquirePrefix", "xacquire"),
				new Constant(ConstantKind.String, "XreleasePrefix", "xrelease"),
				new Constant(ConstantKind.String, "RepPrefix", "rep"),
				new Constant(ConstantKind.String, "RepePrefix", "repe"),
				new Constant(ConstantKind.String, "RepnePrefix", "repne"),
				new Constant(ConstantKind.String, "BndPrefix", "bnd"),
				new Constant(ConstantKind.String, "HintTakenPrefix", "ht"),
				new Constant(ConstantKind.String, "NotrackPrefix", "notrack"),
			};
	}
}
