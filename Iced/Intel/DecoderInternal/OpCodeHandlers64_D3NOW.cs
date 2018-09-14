/*
    Copyright (C) 2018 de4dot@gmail.com

    This file is part of Iced.

    Iced is free software: you can redistribute it and/or modify
    it under the terms of the GNU Lesser General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    Iced is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public License
    along with Iced.  If not, see <https://www.gnu.org/licenses/>.
*/

#if !NO_DECODER64 && !NO_DECODER
using System.Diagnostics;

namespace Iced.Intel.DecoderInternal.OpCodeHandlers64 {
	sealed class OpCodeHandler_D3NOW : OpCodeHandlerModRM {
		readonly Code[] codeValues = OpCodeHandlers_D3NOW.CodeValues;
		readonly MemorySize[] memorySizes = OpCodeHandlers_D3NOW.MemorySizes;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalOpCount = 2;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.Op0Register = (int)state.reg + Register.MM0;
			uint ib;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.Op1Register = (int)state.rm + Register.MM0;
				ib = decoder.ReadByte();
			}
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				decoder.ReadOpMem_m64(ref instruction);
				ib = decoder.ReadByte();
				instruction.InternalMemorySize = memorySizes[(int)ib];
			}
			var code = codeValues[(int)ib];
			instruction.InternalCode = code;
			if (code == Code.INVALID)
				decoder.SetInvalidInstruction();
		}
	}
}
#endif
