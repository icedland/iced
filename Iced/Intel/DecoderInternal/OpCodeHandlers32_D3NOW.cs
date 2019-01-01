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

#if !NO_DECODER32 && !NO_DECODER
using System.Diagnostics;

namespace Iced.Intel.DecoderInternal.OpCodeHandlers32 {
	sealed class OpCodeHandler_D3NOW : OpCodeHandlerModRM {
		readonly Code[] codeValues = OpCodeHandlers_D3NOW.CodeValues;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = (int)state.reg + Register.MM0;
			uint ib;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.InternalOp1Register = (int)state.rm + Register.MM0;
				ib = decoder.ReadByte();
			}
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				decoder.ReadOpMem_m32(ref instruction);
				ib = decoder.ReadByte();
			}
			var code = codeValues[(int)ib];
			instruction.InternalCode = code;
			if (code == Code.INVALID)
				decoder.SetInvalidInstruction();
		}
	}
}
#endif
