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

#if (!NO_DECODER32 || !NO_DECODER64) && !NO_DECODER
using System.Diagnostics;

namespace Iced.Intel.DecoderInternal {
	sealed class OpCodeHandler_ST_STi : OpCodeHandlerModRM {
		readonly Code code;

		public OpCodeHandler_ST_STi(Code code) => this.code = code;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = Register.ST0;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.InternalOp1Register = Register.ST0 + (int)decoder.state.rm;
		}
	}

	sealed class OpCodeHandler_STi_ST : OpCodeHandlerModRM {
		readonly Code code;

		public OpCodeHandler_STi_ST(Code code) => this.code = code;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = Register.ST0 + (int)decoder.state.rm;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.InternalOp1Register = Register.ST0;
		}
	}

	sealed class OpCodeHandler_STi : OpCodeHandlerModRM {
		readonly Code code;

		public OpCodeHandler_STi(Code code) => this.code = code;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = Register.ST0 + (int)decoder.state.rm;
		}
	}
}
#endif
