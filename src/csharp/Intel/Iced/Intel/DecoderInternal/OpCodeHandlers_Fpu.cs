// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if DECODER
using System.Diagnostics;

namespace Iced.Intel.DecoderInternal {
	sealed class OpCodeHandler_ST_STi : OpCodeHandlerModRM {
		readonly Code code;

		public OpCodeHandler_ST_STi(Code code) => this.code = code;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code;
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = Register.ST0;
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
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
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = Register.ST0 + (int)decoder.state.rm;
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
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
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = Register.ST0 + (int)decoder.state.rm;
		}
	}

	sealed class OpCodeHandler_Mf : OpCodeHandlerModRM {
		readonly Code code16;
		readonly Code code32;

		public OpCodeHandler_Mf(Code code) {
			code16 = code;
			code32 = code;
		}

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
			Debug.Assert(state.mod != 3);
			instruction.InternalOp0Kind = OpKind.Memory;
			decoder.ReadOpMem(ref instruction);
		}
	}
}
#endif
