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
	sealed class OpCodeHandler_Mf32 : OpCodeHandlerModRM {
		readonly Code code;

		public OpCodeHandler_Mf32(Code code) => this.code = code;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code;
			if (decoder.state.mod == 3)
				decoder.SetInvalidInstruction();
			else {
				instruction.InternalOp0Kind = OpKind.Memory;
				decoder.ReadOpMem_m64(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_Mf64 : OpCodeHandlerModRM {
		readonly Code code;

		public OpCodeHandler_Mf64(Code code) => this.code = code;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code;
			if (decoder.state.mod == 3)
				decoder.SetInvalidInstruction();
			else {
				instruction.InternalOp0Kind = OpKind.Memory;
				decoder.ReadOpMem_m64(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_Mf80 : OpCodeHandlerModRM {
		readonly Code code;

		public OpCodeHandler_Mf80(Code code) => this.code = code;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code;
			if (decoder.state.mod == 3)
				decoder.SetInvalidInstruction();
			else {
				instruction.InternalOp0Kind = OpKind.Memory;
				decoder.ReadOpMem_m64(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_Mfi16 : OpCodeHandlerModRM {
		readonly Code code;

		public OpCodeHandler_Mfi16(Code code) => this.code = code;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code;
			if (decoder.state.mod == 3)
				decoder.SetInvalidInstruction();
			else {
				instruction.InternalOp0Kind = OpKind.Memory;
				decoder.ReadOpMem_m64(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_Mfi32 : OpCodeHandlerModRM {
		readonly Code code;

		public OpCodeHandler_Mfi32(Code code) => this.code = code;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code;
			if (decoder.state.mod == 3)
				decoder.SetInvalidInstruction();
			else {
				instruction.InternalOp0Kind = OpKind.Memory;
				decoder.ReadOpMem_m64(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_Mfi64 : OpCodeHandlerModRM {
		readonly Code code;

		public OpCodeHandler_Mfi64(Code code) => this.code = code;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code;
			if (decoder.state.mod == 3)
				decoder.SetInvalidInstruction();
			else {
				instruction.InternalOp0Kind = OpKind.Memory;
				decoder.ReadOpMem_m64(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_Mfbcd : OpCodeHandlerModRM {
		readonly Code code;

		public OpCodeHandler_Mfbcd(Code code) => this.code = code;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code;
			if (decoder.state.mod == 3)
				decoder.SetInvalidInstruction();
			else {
				instruction.InternalOp0Kind = OpKind.Memory;
				decoder.ReadOpMem_m64(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_Mf : OpCodeHandlerModRM {
		readonly Code code16;
		readonly Code code32;

		public OpCodeHandler_Mf(Code code16, Code code32) {
			this.code16 = code16;
			this.code32 = code32;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if (state.operandSize != OpSize.Size16)
				instruction.InternalCode = code32;
			else
				instruction.InternalCode = code16;
			if (state.mod == 3)
				decoder.SetInvalidInstruction();
			else {
				instruction.InternalOp0Kind = OpKind.Memory;
				decoder.ReadOpMem_m64(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_Mf2 : OpCodeHandlerModRM {
		readonly Code code;

		public OpCodeHandler_Mf2(Code code) => this.code = code;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code;
			if (decoder.state.mod == 3)
				decoder.SetInvalidInstruction();
			else {
				instruction.InternalOp0Kind = OpKind.Memory;
				decoder.ReadOpMem_m64(ref instruction);
			}
		}
	}
}
#endif
