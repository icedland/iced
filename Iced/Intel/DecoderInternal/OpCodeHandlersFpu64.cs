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
