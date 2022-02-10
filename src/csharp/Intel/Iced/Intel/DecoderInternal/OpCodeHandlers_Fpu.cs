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
			instruction.InternalSetCodeNoCheck(code);
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = Register.ST0;
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op1Kind = OpKind.Register;
			instruction.Op1Register = Register.ST0 + (int)decoder.state.rm;
		}
	}

	sealed class OpCodeHandler_STi_ST : OpCodeHandlerModRM {
		readonly Code code;

		public OpCodeHandler_STi_ST(Code code) => this.code = code;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			instruction.InternalSetCodeNoCheck(code);
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = Register.ST0 + (int)decoder.state.rm;
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op1Kind = OpKind.Register;
			instruction.Op1Register = Register.ST0;
		}
	}

	sealed class OpCodeHandler_STi : OpCodeHandlerModRM {
		readonly Code code;

		public OpCodeHandler_STi(Code code) => this.code = code;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			instruction.InternalSetCodeNoCheck(code);
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = Register.ST0 + (int)decoder.state.rm;
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
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			if (decoder.state.operandSize != OpSize.Size16)
				instruction.InternalSetCodeNoCheck(code32);
			else
				instruction.InternalSetCodeNoCheck(code16);
			Debug.Assert(decoder.state.mod != 3);
			instruction.Op0Kind = OpKind.Memory;
			decoder.ReadOpMem(ref instruction);
		}
	}
}
#endif
