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

namespace Generator.Enums.Instruction {
	/// <summary>
	/// [4:0]	= Operand #0's <c>OpKind</c>
	/// [9:5]	= Operand #1's <c>OpKind</c>
	/// [14:10]	= Operand #2's <c>OpKind</c>
	/// [19:15]	= Operand #3's <c>OpKind</c>
	/// [23:20]	= db/dw/dd/dq element count (1-16, 1-8, 1-4, or 1-2)
	/// [29:24]	= Not used
	/// [31:30]	= CodeSize
	/// </summary>
	[Enum(nameof(OpKindFlags), "Instruction_OpKindFlags", Flags = true, NoInitialize = true)]
	[Flags]
	enum OpKindFlags : uint {
		OpKindBits				= 5,
		OpKindMask				= (1 << (int)OpKindBits) - 1,
		Op1KindShift			= 5,
		Op2KindShift			= 10,
		Op3KindShift			= 15,
		DataLengthMask			= 0xF,
		DataLengthShift			= 20,
		// Unused bits here
		CodeSizeMask			= 3,
		CodeSizeShift			= 30,

		// Bits ignored by Equals()
		EqualsIgnoreMask		= CodeSizeMask << (int)CodeSizeShift,
	}

	[TypeGen(TypeGenOrders.CreateSimpleTypes)]
	sealed class OpKindFlagsEnum {
		OpKindFlagsEnum(GenTypes genTypes) {
			ConstantUtils.VerifyMask<OpKind>((uint)OpKindFlags.OpKindMask);
			ConstantUtils.VerifyMask<CodeSize>((uint)OpKindFlags.CodeSizeMask);
		}
	}
}
