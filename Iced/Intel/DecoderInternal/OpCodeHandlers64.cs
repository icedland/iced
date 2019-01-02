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
	sealed class OpCodeHandler_Ev_Iz : OpCodeHandlerModRM {
		readonly Code code16;
		readonly Code code32;
		readonly Code code64;
		readonly HandlerFlags flags;

		public OpCodeHandler_Ev_Iz(Code code16, Code code32, Code code64) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
		}

		public OpCodeHandler_Ev_Iz(Code code16, Code code32, Code code64, HandlerFlags flags) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
			this.flags = flags;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				uint index = state.rm + state.extraBaseRegisterBase;
				if (state.operandSize == OpSize.Size32)
					instruction.InternalOp0Register = (int)index + Register.EAX;
				else if (state.operandSize == OpSize.Size64)
					instruction.InternalOp0Register = (int)index + Register.RAX;
				else
					instruction.InternalOp0Register = (int)index + Register.AX;
			}
			else {
				instruction.InternalOp0Kind = OpKind.Memory;
				decoder.ReadOpMem_m64(ref instruction);
				if ((flags & HandlerFlags.XacquireRelease) != 0)
					decoder.SetXacquireRelease(ref instruction, flags);
			}
			if (state.operandSize == OpSize.Size32) {
				instruction.InternalCode = code32;
				instruction.InternalOp1Kind = OpKind.Immediate32;
				instruction.Immediate32 = decoder.ReadUInt32();
			}
			else if (state.operandSize == OpSize.Size64) {
				instruction.InternalCode = code64;
				instruction.InternalOp1Kind = OpKind.Immediate32to64;
				instruction.Immediate32 = decoder.ReadUInt32();
			}
			else {
				instruction.InternalCode = code16;
				instruction.InternalOp1Kind = OpKind.Immediate16;
				instruction.InternalImmediate16 = decoder.ReadUInt16();
			}
		}
	}

	sealed class OpCodeHandler_Ev_Ib : OpCodeHandlerModRM {
		readonly Code code16;
		readonly Code code32;
		readonly Code code64;
		readonly HandlerFlags flags;

		public OpCodeHandler_Ev_Ib(Code code16, Code code32, Code code64) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
		}

		public OpCodeHandler_Ev_Ib(Code code16, Code code32, Code code64, HandlerFlags flags) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
			this.flags = flags;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				uint index = state.rm + state.extraBaseRegisterBase;
				if (state.operandSize == OpSize.Size32)
					instruction.InternalOp0Register = (int)index + Register.EAX;
				else if (state.operandSize == OpSize.Size64)
					instruction.InternalOp0Register = (int)index + Register.RAX;
				else
					instruction.InternalOp0Register = (int)index + Register.AX;
			}
			else {
				instruction.InternalOp0Kind = OpKind.Memory;
				decoder.ReadOpMem_m64(ref instruction);
				if ((flags & HandlerFlags.XacquireRelease) != 0)
					decoder.SetXacquireRelease(ref instruction, flags);
			}
			if (state.operandSize == OpSize.Size32) {
				instruction.InternalCode = code32;
				instruction.InternalOp1Kind = OpKind.Immediate8to32;
				instruction.InternalImmediate8 = decoder.ReadIb();
			}
			else if (state.operandSize == OpSize.Size64) {
				instruction.InternalCode = code64;
				instruction.InternalOp1Kind = OpKind.Immediate8to64;
				instruction.InternalImmediate8 = decoder.ReadIb();
			}
			else {
				instruction.InternalCode = code16;
				instruction.InternalOp1Kind = OpKind.Immediate8to16;
				instruction.InternalImmediate8 = decoder.ReadIb();
			}
		}
	}

	sealed class OpCodeHandler_Ev_Ib2 : OpCodeHandlerModRM {
		readonly Code code16;
		readonly Code code32;
		readonly Code code64;
		readonly HandlerFlags flags;

		public OpCodeHandler_Ev_Ib2(Code code16, Code code32, Code code64) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
		}

		public OpCodeHandler_Ev_Ib2(Code code16, Code code32, Code code64, HandlerFlags flags) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
			this.flags = flags;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if (state.operandSize == OpSize.Size32)
				instruction.InternalCode = code32;
			else if (state.operandSize == OpSize.Size64)
				instruction.InternalCode = code64;
			else
				instruction.InternalCode = code16;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				uint index = state.rm + state.extraBaseRegisterBase;
				if (state.operandSize == OpSize.Size32)
					instruction.InternalOp0Register = (int)index + Register.EAX;
				else if (state.operandSize == OpSize.Size64)
					instruction.InternalOp0Register = (int)index + Register.RAX;
				else
					instruction.InternalOp0Register = (int)index + Register.AX;
			}
			else {
				instruction.InternalOp0Kind = OpKind.Memory;
				decoder.ReadOpMem_m64(ref instruction);
				if ((flags & HandlerFlags.XacquireRelease) != 0)
					decoder.SetXacquireRelease(ref instruction, flags);
			}
			instruction.InternalOp1Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadIb();
		}
	}

	sealed class OpCodeHandler_Ev_1 : OpCodeHandlerModRM {
		readonly Code code16;
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_Ev_1(Code code16, Code code32, Code code64) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if (state.operandSize == OpSize.Size32)
				instruction.InternalCode = code32;
			else if (state.operandSize == OpSize.Size64)
				instruction.InternalCode = code64;
			else
				instruction.InternalCode = code16;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				uint index = state.rm + state.extraBaseRegisterBase;
				if (state.operandSize == OpSize.Size32)
					instruction.InternalOp0Register = (int)index + Register.EAX;
				else if (state.operandSize == OpSize.Size64)
					instruction.InternalOp0Register = (int)index + Register.RAX;
				else
					instruction.InternalOp0Register = (int)index + Register.AX;
			}
			else {
				instruction.InternalOp0Kind = OpKind.Memory;
				decoder.ReadOpMem_m64(ref instruction);
			}
			instruction.InternalOp1Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = 1;
			state.flags |= StateFlags.NoImm;
		}
	}

	sealed class OpCodeHandler_Ev_CL : OpCodeHandlerModRM {
		readonly Code code16;
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_Ev_CL(Code code16, Code code32, Code code64) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if (state.operandSize == OpSize.Size32)
				instruction.InternalCode = code32;
			else if (state.operandSize == OpSize.Size64)
				instruction.InternalCode = code64;
			else
				instruction.InternalCode = code16;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				uint index = state.rm + state.extraBaseRegisterBase;
				if (state.operandSize == OpSize.Size32)
					instruction.InternalOp0Register = (int)index + Register.EAX;
				else if (state.operandSize == OpSize.Size64)
					instruction.InternalOp0Register = (int)index + Register.RAX;
				else
					instruction.InternalOp0Register = (int)index + Register.AX;
			}
			else {
				instruction.InternalOp0Kind = OpKind.Memory;
				decoder.ReadOpMem_m64(ref instruction);
			}
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.InternalOp1Register = Register.CL;
		}
	}

	sealed class OpCodeHandler_Ev : OpCodeHandlerModRM {
		readonly Code code16;
		readonly Code code32;
		readonly Code code64;
		readonly HandlerFlags flags;

		public OpCodeHandler_Ev(Code code16, Code code32, Code code64) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
		}

		public OpCodeHandler_Ev(Code code16, Code code32, Code code64, HandlerFlags flags) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
			this.flags = flags;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if (state.operandSize == OpSize.Size32)
				instruction.InternalCode = code32;
			else if (state.operandSize == OpSize.Size64)
				instruction.InternalCode = code64;
			else
				instruction.InternalCode = code16;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				uint index = state.rm + state.extraBaseRegisterBase;
				if (state.operandSize == OpSize.Size32)
					instruction.InternalOp0Register = (int)index + Register.EAX;
				else if (state.operandSize == OpSize.Size64)
					instruction.InternalOp0Register = (int)index + Register.RAX;
				else
					instruction.InternalOp0Register = (int)index + Register.AX;
			}
			else {
				instruction.InternalOp0Kind = OpKind.Memory;
				decoder.ReadOpMem_m64(ref instruction);
				if ((flags & HandlerFlags.XacquireRelease) != 0)
					decoder.SetXacquireRelease(ref instruction, flags);
			}
		}
	}

	sealed class OpCodeHandler_Rv : OpCodeHandlerModRM {
		readonly Code code16;
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_Rv(Code code16, Code code32, Code code64) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if (state.operandSize == OpSize.Size32) {
				instruction.InternalCode = code32;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.rm + state.extraBaseRegisterBase) + Register.EAX;
			}
			else if (state.operandSize == OpSize.Size64) {
				instruction.InternalCode = code64;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.rm + state.extraBaseRegisterBase) + Register.RAX;
			}
			else {
				instruction.InternalCode = code16;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.rm + state.extraBaseRegisterBase) + Register.AX;
			}
			if (state.mod != 3)
				decoder.SetInvalidInstruction();
		}
	}

	sealed class OpCodeHandler_Rv_32_64 : OpCodeHandlerModRM {
		readonly Code code64;

		public OpCodeHandler_Rv_32_64(Code code32, Code code64) => this.code64 = code64;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code64;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.rm + state.extraBaseRegisterBase) + Register.RAX;
			}
			else
				decoder.SetInvalidInstruction();
		}
	}

	sealed class OpCodeHandler_Ev_REXW : OpCodeHandlerModRM {
		readonly Code code32;
		readonly Code code64;
		readonly bool allowReg;
		readonly bool allowMem;

		public OpCodeHandler_Ev_REXW(Code code32, Code code64, bool allowReg, bool allowMem) {
			this.code32 = code32;
			this.code64 = code64;
			this.allowReg = allowReg;
			this.allowMem = allowMem;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if ((state.flags & StateFlags.W) != 0)
				instruction.InternalCode = code64;
			else
				instruction.InternalCode = code32;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.rm + state.extraBaseRegisterBase) + ((state.flags & StateFlags.W) != 0 ? Register.RAX : Register.EAX);
				if (!allowReg)
					decoder.SetInvalidInstruction();
			}
			else {
				instruction.InternalOp0Kind = OpKind.Memory;
				decoder.ReadOpMem_m64(ref instruction);
				if (!allowMem)
					decoder.SetInvalidInstruction();
			}
		}
	}

	sealed class OpCodeHandler_Evj : OpCodeHandlerModRM {
		readonly Code code16;
		readonly Code code64;

		public OpCodeHandler_Evj(Code code16, Code code64) {
			this.code16 = code16;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if ((decoder.options & DecoderOptions.AMD) == 0 || state.operandSize == OpSize.Size32)
				instruction.InternalCode = code64;
			else
				instruction.InternalCode = code16;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				if ((decoder.options & DecoderOptions.AMD) == 0 || state.operandSize == OpSize.Size32)
					instruction.InternalOp0Register = (int)(state.rm + state.extraBaseRegisterBase) + Register.RAX;
				else
					instruction.InternalOp0Register = (int)(state.rm + state.extraBaseRegisterBase) + Register.AX;
			}
			else {
				instruction.InternalOp0Kind = OpKind.Memory;
				decoder.ReadOpMem_m64(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_Ep : OpCodeHandlerModRM {
		readonly Code code16;
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_Ep(Code code16, Code code32, Code code64) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if (state.operandSize == OpSize.Size32)
				instruction.InternalCode = code32;
			else if (state.operandSize == OpSize.Size64)
				instruction.InternalCode = code64;
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

	sealed class OpCodeHandler_Evw : OpCodeHandlerModRM {
		readonly Code code16;
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_Evw(Code code16, Code code32, Code code64) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if (state.operandSize == OpSize.Size32)
				instruction.InternalCode = code32;
			else if (state.operandSize == OpSize.Size64)
				instruction.InternalCode = code64;
			else
				instruction.InternalCode = code16;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				uint index = state.rm + state.extraBaseRegisterBase;
				if (state.operandSize == OpSize.Size32)
					instruction.InternalOp0Register = (int)index + Register.EAX;
				else if (state.operandSize == OpSize.Size64)
					instruction.InternalOp0Register = (int)index + Register.RAX;
				else
					instruction.InternalOp0Register = (int)index + Register.AX;
			}
			else {
				instruction.InternalOp0Kind = OpKind.Memory;
				decoder.ReadOpMem_m64(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_Ew : OpCodeHandlerModRM {
		readonly Code code16;
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_Ew(Code code16, Code code32, Code code64) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if (state.operandSize == OpSize.Size32)
				instruction.InternalCode = code32;
			else if (state.operandSize == OpSize.Size64)
				instruction.InternalCode = code64;
			else
				instruction.InternalCode = code16;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				uint index = state.rm + state.extraBaseRegisterBase;
				if (state.operandSize == OpSize.Size32)
					instruction.InternalOp0Register = (int)index + Register.EAX;
				else if (state.operandSize == OpSize.Size64)
					instruction.InternalOp0Register = (int)index + Register.RAX;
				else
					instruction.InternalOp0Register = (int)index + Register.AX;
			}
			else {
				instruction.InternalOp0Kind = OpKind.Memory;
				decoder.ReadOpMem_m64(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_Ms : OpCodeHandlerModRM {
		readonly Code code64;

		public OpCodeHandler_Ms(Code code16, Code code32, Code code64) => this.code64 = code64;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code64;
			if (state.mod == 3)
				decoder.SetInvalidInstruction();
			else {
				instruction.InternalOp0Kind = OpKind.Memory;
				decoder.ReadOpMem_m64(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_Gv_Ev : OpCodeHandlerModRM {
		readonly Code code16;
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_Gv_Ev(Code code16, Code code32, Code code64) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if (state.operandSize == OpSize.Size32) {
				instruction.InternalCode = code32;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + Register.EAX;
			}
			else if (state.operandSize == OpSize.Size64) {
				instruction.InternalCode = code64;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + Register.RAX;
			}
			else {
				instruction.InternalCode = code16;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + Register.AX;
			}
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				uint index = state.rm + state.extraBaseRegisterBase;
				if (state.operandSize == OpSize.Size32)
					instruction.InternalOp1Register = (int)index + Register.EAX;
				else if (state.operandSize == OpSize.Size64)
					instruction.InternalOp1Register = (int)index + Register.RAX;
				else
					instruction.InternalOp1Register = (int)index + Register.AX;
			}
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				decoder.ReadOpMem_m64(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_Gv_M_as : OpCodeHandlerModRM {
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_Gv_M_as(Code code16, Code code32, Code code64) {
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if (state.addressSize == OpSize.Size64) {
				instruction.InternalCode = code64;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + Register.RAX;
			}
			else {
				Debug.Assert(state.addressSize == OpSize.Size32);
				instruction.InternalCode = code32;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + Register.EAX;
			}
			if (state.mod == 3)
				decoder.SetInvalidInstruction();
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				decoder.ReadOpMem_m64(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_Gdq_Ev : OpCodeHandlerModRM {
		readonly Code code16;
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_Gdq_Ev(Code code16, Code code32, Code code64) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if (state.operandSize == OpSize.Size64) {
				instruction.InternalCode = code64;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + Register.RAX;
			}
			else if (state.operandSize == OpSize.Size32) {
				instruction.InternalCode = code32;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + Register.EAX;
			}
			else {
				instruction.InternalCode = code16;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + Register.EAX;
			}
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				uint index = state.rm + state.extraBaseRegisterBase;
				if (state.operandSize == OpSize.Size64)
					instruction.InternalOp1Register = (int)index + Register.RAX;
				else if (state.operandSize == OpSize.Size32)
					instruction.InternalOp1Register = (int)index + Register.EAX;
				else
					instruction.InternalOp1Register = (int)index + Register.AX;
			}
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				decoder.ReadOpMem_m64(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_Gv_Ev3 : OpCodeHandlerModRM {
		readonly Code code16;
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_Gv_Ev3(Code code16, Code code32, Code code64) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if (state.operandSize == OpSize.Size32) {
				instruction.InternalCode = code32;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + Register.EAX;
			}
			else if (state.operandSize == OpSize.Size64) {
				instruction.InternalCode = code64;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + Register.RAX;
			}
			else {
				instruction.InternalCode = code16;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + Register.AX;
			}
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				uint index = state.rm + state.extraBaseRegisterBase;
				if (state.operandSize == OpSize.Size32)
					instruction.InternalOp1Register = (int)index + Register.EAX;
				else if (state.operandSize == OpSize.Size64)
					instruction.InternalOp1Register = (int)index + Register.RAX;
				else
					instruction.InternalOp1Register = (int)index + Register.AX;
			}
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				decoder.ReadOpMem_m64(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_Gv_Ev2 : OpCodeHandlerModRM {
		readonly Code code16;
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_Gv_Ev2(Code code16, Code code32, Code code64) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if (state.operandSize == OpSize.Size32) {
				instruction.InternalCode = code32;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + Register.EAX;
			}
			else if (state.operandSize == OpSize.Size64) {
				instruction.InternalCode = code64;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + Register.RAX;
			}
			else {
				instruction.InternalCode = code16;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + Register.AX;
			}
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				uint index = state.rm + state.extraBaseRegisterBase;
				if (state.operandSize == OpSize.Size16)
					instruction.InternalOp1Register = (int)index + Register.AX;
				else
					instruction.InternalOp1Register = (int)index + Register.EAX;
			}
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				decoder.ReadOpMem_m64(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_Rq_Cq : OpCodeHandlerModRM {
		readonly Code code;
		readonly Register baseReg;

		public OpCodeHandler_Rq_Cq(Code code, Register baseReg) {
			this.code = code;
			this.baseReg = baseReg;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = (int)(state.rm + state.extraBaseRegisterBase) + Register.RAX;
			// LOCK MOV CR0 is supported by some AMD CPUs
			if (baseReg == Register.CR0 && state.reg == 0 && state.extraRegisterBase == 0 && instruction.HasLockPrefix) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.InternalOp1Register = Register.CR8;
				instruction.InternalClearHasLockPrefix();
			}
			else {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.InternalOp1Register = (int)(state.reg + state.extraRegisterBase) + baseReg;
			}
		}
	}

	sealed class OpCodeHandler_Cq_Rq : OpCodeHandlerModRM {
		readonly Code code;
		readonly Register baseReg;

		public OpCodeHandler_Cq_Rq(Code code, Register baseReg) {
			this.code = code;
			this.baseReg = baseReg;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code;
			// LOCK MOV CR0 is supported by some AMD CPUs
			if (baseReg == Register.CR0 && state.reg == 0 && state.extraRegisterBase == 0 && instruction.HasLockPrefix) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = Register.CR8;
				instruction.InternalClearHasLockPrefix();
			}
			else {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + baseReg;
			}
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.InternalOp1Register = (int)(state.rm + state.extraBaseRegisterBase) + Register.RAX;
		}
	}

	sealed class OpCodeHandler_Jb : OpCodeHandler {
		readonly Code code16;
		readonly Code code64;

		public OpCodeHandler_Jb(Code code16, Code code64) {
			this.code16 = code16;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if ((decoder.options & DecoderOptions.AMD) == 0 || state.operandSize == OpSize.Size32) {
				instruction.InternalCode = code64;
				instruction.InternalOp0Kind = OpKind.NearBranch64;
				instruction.NearBranch64 = (ulong)(sbyte)decoder.ReadByte() + decoder.GetCurrentInstructionPointer64();
			}
			else {
				instruction.InternalCode = code16;
				instruction.InternalOp0Kind = OpKind.NearBranch16;
				instruction.InternalNearBranch16 = (ushort)((sbyte)decoder.ReadByte() + decoder.GetCurrentInstructionPointer32());
			}
		}
	}

	sealed class OpCodeHandler_Jx : OpCodeHandler {
		readonly Code code16;
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_Jx(Code code16, Code code32, Code code64) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if (state.operandSize == OpSize.Size32) {
				instruction.InternalCode = code32;
				instruction.InternalOp0Kind = OpKind.NearBranch64;
				instruction.NearBranch64 = (ulong)(int)decoder.ReadUInt32() + decoder.GetCurrentInstructionPointer64();
			}
			else if (state.operandSize == OpSize.Size64) {
				instruction.InternalCode = code64;
				instruction.InternalOp0Kind = OpKind.NearBranch64;
				instruction.NearBranch64 = (ulong)(int)decoder.ReadUInt32() + decoder.GetCurrentInstructionPointer64();
			}
			else {
				Debug.Assert(state.operandSize == OpSize.Size16);
				instruction.InternalCode = code16;
				instruction.InternalOp0Kind = OpKind.NearBranch64;
				instruction.NearBranch64 = (ulong)(short)decoder.ReadUInt16() + decoder.GetCurrentInstructionPointer64();
			}
		}
	}

	sealed class OpCodeHandler_Jz : OpCodeHandler {
		readonly Code code16;
		readonly Code code64;

		public OpCodeHandler_Jz(Code code16, Code code64) {
			this.code16 = code16;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if ((decoder.options & DecoderOptions.AMD) == 0 || state.operandSize == OpSize.Size32) {
				instruction.InternalCode = code64;
				instruction.InternalOp0Kind = OpKind.NearBranch64;
				instruction.NearBranch64 = (ulong)(int)decoder.ReadUInt32() + decoder.GetCurrentInstructionPointer64();
			}
			else {
				instruction.InternalCode = code16;
				instruction.InternalOp0Kind = OpKind.NearBranch16;
				instruction.InternalNearBranch16 = (ushort)(decoder.ReadUInt16() + decoder.GetCurrentInstructionPointer32());
			}
		}
	}

	sealed class OpCodeHandler_Jb2 : OpCodeHandler {
		readonly Code code16_32;
		readonly Code code16_64;
		readonly Code code64_32;
		readonly Code code64_64;

		public OpCodeHandler_Jb2(Code code16_32, Code code16_64, Code code64_32, Code code64_64) {
			this.code16_32 = code16_32;
			this.code16_64 = code16_64;
			this.code64_32 = code64_32;
			this.code64_64 = code64_64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if ((decoder.options & DecoderOptions.AMD) == 0 || state.operandSize == OpSize.Size32) {
				if (state.addressSize == OpSize.Size64)
					instruction.InternalCode = code64_64;
				else
					instruction.InternalCode = code64_32;
				instruction.InternalOp0Kind = OpKind.NearBranch64;
				instruction.NearBranch64 = (ulong)(sbyte)decoder.ReadByte() + decoder.GetCurrentInstructionPointer64();
			}
			else {
				if (state.addressSize == OpSize.Size64)
					instruction.InternalCode = code16_64;
				else
					instruction.InternalCode = code16_32;
				instruction.InternalOp0Kind = OpKind.NearBranch16;
				instruction.InternalNearBranch16 = (ushort)((sbyte)decoder.ReadByte() + decoder.GetCurrentInstructionPointer32());
			}
		}
	}

	sealed class OpCodeHandler_PushOpSizeReg : OpCodeHandler {
		readonly Code code16;
		readonly Code code64;
		readonly Register reg;

		public OpCodeHandler_PushOpSizeReg(Code code16, Code code64, Register reg) {
			this.code16 = code16;
			this.code64 = code64;
			this.reg = reg;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if (state.operandSize != OpSize.Size16)
				instruction.InternalCode = code64;
			else
				instruction.InternalCode = code16;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = reg;
		}
	}

	sealed class OpCodeHandler_PushEv : OpCodeHandlerModRM {
		readonly Code code16;
		readonly Code code64;

		public OpCodeHandler_PushEv(Code code16, Code code64) {
			this.code16 = code16;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if (state.operandSize != OpSize.Size16)
				instruction.InternalCode = code64;
			else
				instruction.InternalCode = code16;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				uint index = state.rm + state.extraBaseRegisterBase;
				if (state.operandSize != OpSize.Size16)
					instruction.InternalOp0Register = (int)index + Register.RAX;
				else
					instruction.InternalOp0Register = (int)index + Register.AX;
			}
			else {
				instruction.InternalOp0Kind = OpKind.Memory;
				decoder.ReadOpMem_m64(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_Ev_Gv : OpCodeHandlerModRM {
		readonly Code code16;
		readonly Code code32;
		readonly Code code64;
		readonly HandlerFlags flags;

		public OpCodeHandler_Ev_Gv(Code code16, Code code32, Code code64) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
		}

		public OpCodeHandler_Ev_Gv(Code code16, Code code32, Code code64, HandlerFlags flags) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
			this.flags = flags;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				uint index = state.rm + state.extraBaseRegisterBase;
				if (state.operandSize == OpSize.Size32)
					instruction.InternalOp0Register = (int)index + Register.EAX;
				else if (state.operandSize == OpSize.Size64)
					instruction.InternalOp0Register = (int)index + Register.RAX;
				else
					instruction.InternalOp0Register = (int)index + Register.AX;
			}
			else {
				instruction.InternalOp0Kind = OpKind.Memory;
				decoder.ReadOpMem_m64(ref instruction);
				if ((flags & HandlerFlags.XacquireRelease) != 0)
					decoder.SetXacquireRelease(ref instruction, flags);
			}
			if (state.operandSize == OpSize.Size32) {
				instruction.InternalCode = code32;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.InternalOp1Register = (int)(state.reg + state.extraRegisterBase) + Register.EAX;
			}
			else if (state.operandSize == OpSize.Size64) {
				instruction.InternalCode = code64;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.InternalOp1Register = (int)(state.reg + state.extraRegisterBase) + Register.RAX;
			}
			else {
				instruction.InternalCode = code16;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.InternalOp1Register = (int)(state.reg + state.extraRegisterBase) + Register.AX;
			}
		}
	}

	sealed class OpCodeHandler_Ev_Gv_32_64 : OpCodeHandlerModRM {
		readonly Code code64;

		public OpCodeHandler_Ev_Gv_32_64(Code code32, Code code64) => this.code64 = code64;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.rm + state.extraBaseRegisterBase) + Register.RAX;
			}
			else {
				instruction.InternalOp0Kind = OpKind.Memory;
				decoder.ReadOpMem_m64(ref instruction);
			}
			instruction.InternalCode = code64;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.InternalOp1Register = (int)(state.reg + state.extraRegisterBase) + Register.RAX;
		}
	}

	sealed class OpCodeHandler_Ev_Gv_Ib : OpCodeHandlerModRM {
		readonly Code code16;
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_Ev_Gv_Ib(Code code16, Code code32, Code code64) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				uint index = state.rm + state.extraBaseRegisterBase;
				if (state.operandSize == OpSize.Size32)
					instruction.InternalOp0Register = (int)index + Register.EAX;
				else if (state.operandSize == OpSize.Size64)
					instruction.InternalOp0Register = (int)index + Register.RAX;
				else
					instruction.InternalOp0Register = (int)index + Register.AX;
			}
			else {
				instruction.InternalOp0Kind = OpKind.Memory;
				decoder.ReadOpMem_m64(ref instruction);
			}
			if (state.operandSize == OpSize.Size32) {
				instruction.InternalCode = code32;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.InternalOp1Register = (int)(state.reg + state.extraRegisterBase) + Register.EAX;
			}
			else if (state.operandSize == OpSize.Size64) {
				instruction.InternalCode = code64;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.InternalOp1Register = (int)(state.reg + state.extraRegisterBase) + Register.RAX;
			}
			else {
				instruction.InternalCode = code16;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.InternalOp1Register = (int)(state.reg + state.extraRegisterBase) + Register.AX;
			}
			instruction.InternalOp2Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadIb();
		}
	}

	sealed class OpCodeHandler_Ev_Gv_CL : OpCodeHandlerModRM {
		readonly Code code16;
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_Ev_Gv_CL(Code code16, Code code32, Code code64) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				uint index = state.rm + state.extraBaseRegisterBase;
				if (state.operandSize == OpSize.Size32)
					instruction.InternalOp0Register = (int)index + Register.EAX;
				else if (state.operandSize == OpSize.Size64)
					instruction.InternalOp0Register = (int)index + Register.RAX;
				else
					instruction.InternalOp0Register = (int)index + Register.AX;
			}
			else {
				instruction.InternalOp0Kind = OpKind.Memory;
				decoder.ReadOpMem_m64(ref instruction);
			}
			if (state.operandSize == OpSize.Size32) {
				instruction.InternalCode = code32;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.InternalOp1Register = (int)(state.reg + state.extraRegisterBase) + Register.EAX;
			}
			else if (state.operandSize == OpSize.Size64) {
				instruction.InternalCode = code64;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.InternalOp1Register = (int)(state.reg + state.extraRegisterBase) + Register.RAX;
			}
			else {
				instruction.InternalCode = code16;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.InternalOp1Register = (int)(state.reg + state.extraRegisterBase) + Register.AX;
			}
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp2Kind = OpKind.Register;
			instruction.InternalOp2Register = Register.CL;
		}
	}

	sealed class OpCodeHandler_Gv_Mp : OpCodeHandlerModRM {
		readonly Code code16;
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_Gv_Mp(Code code16, Code code32, Code code64) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if (state.operandSize == OpSize.Size32) {
				instruction.InternalCode = code32;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + Register.EAX;
			}
			else if (state.operandSize == OpSize.Size64) {
				instruction.InternalCode = code64;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + Register.RAX;
			}
			else {
				instruction.InternalCode = code16;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + Register.AX;
			}
			if (state.mod == 3)
				decoder.SetInvalidInstruction();
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				decoder.ReadOpMem_m64(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_Gv_Eb : OpCodeHandlerModRM {
		readonly Code code16;
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_Gv_Eb(Code code16, Code code32, Code code64) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if (state.operandSize == OpSize.Size32) {
				instruction.InternalCode = code32;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + Register.EAX;
			}
			else if (state.operandSize == OpSize.Size64) {
				instruction.InternalCode = code64;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + Register.RAX;
			}
			else {
				instruction.InternalCode = code16;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + Register.AX;
			}
			if (state.mod == 3) {
				uint index = state.rm + state.extraBaseRegisterBase;
				if ((state.flags & StateFlags.HasRex) != 0 && index >= 4)
					index += 4;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.InternalOp1Register = (int)index + Register.AL;
			}
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				decoder.ReadOpMem_m64(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_Gv_Ew : OpCodeHandlerModRM {
		readonly Code code16;
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_Gv_Ew(Code code16, Code code32, Code code64) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if (state.operandSize == OpSize.Size32) {
				instruction.InternalCode = code32;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + Register.EAX;
			}
			else if (state.operandSize == OpSize.Size64) {
				instruction.InternalCode = code64;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + Register.RAX;
			}
			else {
				instruction.InternalCode = code16;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + Register.AX;
			}
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.InternalOp1Register = (int)(state.rm + state.extraBaseRegisterBase) + Register.AX;
			}
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				decoder.ReadOpMem_m64(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_PushSimple2 : OpCodeHandler {
		readonly Code code16;
		readonly Code code64;

		public OpCodeHandler_PushSimple2(Code code16, Code code64) {
			this.code16 = code16;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if (state.operandSize != OpSize.Size16)
				instruction.InternalCode = code64;
			else
				instruction.InternalCode = code16;
		}
	}

	sealed class OpCodeHandler_Simple2 : OpCodeHandler {
		readonly Code code16;
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_Simple2(Code code16, Code code32, Code code64) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if (state.operandSize == OpSize.Size32)
				instruction.InternalCode = code32;
			else if (state.operandSize == OpSize.Size64)
				instruction.InternalCode = code64;
			else
				instruction.InternalCode = code16;
		}
	}

	sealed class OpCodeHandler_Simple2Iw : OpCodeHandler {
		readonly Code code16;
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_Simple2Iw(Code code16, Code code32, Code code64) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if (state.operandSize == OpSize.Size32)
				instruction.InternalCode = code32;
			else if (state.operandSize == OpSize.Size64)
				instruction.InternalCode = code64;
			else
				instruction.InternalCode = code16;
			instruction.InternalOp0Kind = OpKind.Immediate16;
			instruction.InternalImmediate16 = decoder.ReadUInt16();
		}
	}

	sealed class OpCodeHandler_Simple3 : OpCodeHandler {
		readonly Code code16;
		readonly Code code64;

		public OpCodeHandler_Simple3(Code code16, Code code64) {
			this.code16 = code16;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if (state.operandSize != OpSize.Size16)
				instruction.InternalCode = code64;
			else
				instruction.InternalCode = code16;
		}
	}

	sealed class OpCodeHandler_Simple5 : OpCodeHandler {
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_Simple5(Code code16, Code code32, Code code64) {
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if (state.addressSize == OpSize.Size64)
				instruction.InternalCode = code64;
			else {
				Debug.Assert(state.addressSize == OpSize.Size32);
				instruction.InternalCode = code32;
			}
		}
	}

	sealed class OpCodeHandler_Simple5_ModRM_as : OpCodeHandlerModRM {
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_Simple5_ModRM_as(Code code16, Code code32, Code code64) {
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if (state.addressSize == OpSize.Size64) {
				instruction.InternalCode = code64;
				instruction.InternalOp0Register = (int)(state.rm + state.extraBaseRegisterBase) + Register.RAX;
			}
			else {
				Debug.Assert(state.addressSize == OpSize.Size32);
				instruction.InternalCode = code32;
				instruction.InternalOp0Register = (int)(state.rm + state.extraBaseRegisterBase) + Register.EAX;
			}
		}
	}

	sealed class OpCodeHandler_Simple4 : OpCodeHandler {
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_Simple4(Code code32, Code code64) {
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if ((state.flags & StateFlags.W) != 0)
				instruction.InternalCode = code64;
			else
				instruction.InternalCode = code32;
		}
	}

	sealed class OpCodeHandler_PushSimpleReg : OpCodeHandler {
		readonly Code code16_lo;
		readonly Code code16_hi;
		readonly Code code64_lo;
		readonly Code code64_hi;
		readonly Register reg16_lo;
		readonly Register reg16_hi;
		readonly Register reg64_lo;
		readonly Register reg64_hi;

		public OpCodeHandler_PushSimpleReg(Code code16_lo, Code code16_hi, Code code64_lo, Code code64_hi, Register reg16_lo, Register reg16_hi, Register reg64_lo, Register reg64_hi) {
			this.code16_lo = code16_lo;
			this.code16_hi = code16_hi;
			this.code64_lo = code64_lo;
			this.code64_hi = code64_hi;
			this.reg16_lo = reg16_lo;
			this.reg16_hi = reg16_hi;
			this.reg64_lo = reg64_lo;
			this.reg64_hi = reg64_hi;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if (state.operandSize != OpSize.Size16) {
				if (state.extraBaseRegisterBase == 0) {
					instruction.InternalCode = code64_lo;
					Debug.Assert(OpKind.Register == 0);
					//instruction.InternalOp0Kind = OpKind.Register;
					instruction.InternalOp0Register = reg64_lo;
				}
				else {
					instruction.InternalCode = code64_hi;
					Debug.Assert(OpKind.Register == 0);
					//instruction.InternalOp0Kind = OpKind.Register;
					instruction.InternalOp0Register = reg64_hi;
				}
			}
			else {
				if (state.extraBaseRegisterBase == 0) {
					instruction.InternalCode = code16_lo;
					Debug.Assert(OpKind.Register == 0);
					//instruction.InternalOp0Kind = OpKind.Register;
					instruction.InternalOp0Register = reg16_lo;
				}
				else {
					instruction.InternalCode = code16_hi;
					Debug.Assert(OpKind.Register == 0);
					//instruction.InternalOp0Kind = OpKind.Register;
					instruction.InternalOp0Register = reg16_hi;
				}
			}
		}
	}

	sealed class OpCodeHandler_SimpleReg : OpCodeHandler {
		readonly int index;

		public OpCodeHandler_SimpleReg(int index) {
			Debug.Assert(0 <= index && index <= 7);
			this.index = index;
		}

		static readonly Code[] codes = new Code[3] {
			Code.Bswap_r16,
			Code.Bswap_r32,
			Code.Bswap_r64,
		};

		static readonly Register[,] registers = new Register[3, 16] {
			{
				Register.AX,
				Register.CX,
				Register.DX,
				Register.BX,
				Register.SP,
				Register.BP,
				Register.SI,
				Register.DI,
				Register.R8W,
				Register.R9W,
				Register.R10W,
				Register.R11W,
				Register.R12W,
				Register.R13W,
				Register.R14W,
				Register.R15W,
			},
			{
				Register.EAX,
				Register.ECX,
				Register.EDX,
				Register.EBX,
				Register.ESP,
				Register.EBP,
				Register.ESI,
				Register.EDI,
				Register.R8D,
				Register.R9D,
				Register.R10D,
				Register.R11D,
				Register.R12D,
				Register.R13D,
				Register.R14D,
				Register.R15D,
			},
			{
				Register.RAX,
				Register.RCX,
				Register.RDX,
				Register.RBX,
				Register.RSP,
				Register.RBP,
				Register.RSI,
				Register.RDI,
				Register.R8,
				Register.R9,
				Register.R10,
				Register.R11,
				Register.R12,
				Register.R13,
				Register.R14,
				Register.R15,
			},
		};

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			Debug.Assert((int)OpSize.Size16 == 0);
			Debug.Assert((int)OpSize.Size32 == 1);
			Debug.Assert((int)OpSize.Size64 == 2);
			int sizeIndex = (int)state.operandSize;
			int codeIndex = index + (int)state.extraBaseRegisterBase;

			instruction.InternalCode = codes[sizeIndex];
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = registers[sizeIndex, codeIndex];
		}
	}

	sealed class OpCodeHandler_Xchg_Reg_rAX : OpCodeHandler {
		readonly int index;

		public OpCodeHandler_Xchg_Reg_rAX(int index) {
			Debug.Assert(0 <= index && index <= 7);
			this.index = index;
		}

		static readonly Code[,] codes = new Code[3, 16] {
			{
				Code.Nopw,
				Code.Xchg_r16_AX,
				Code.Xchg_r16_AX,
				Code.Xchg_r16_AX,
				Code.Xchg_r16_AX,
				Code.Xchg_r16_AX,
				Code.Xchg_r16_AX,
				Code.Xchg_r16_AX,
				Code.Xchg_r16_AX,
				Code.Xchg_r16_AX,
				Code.Xchg_r16_AX,
				Code.Xchg_r16_AX,
				Code.Xchg_r16_AX,
				Code.Xchg_r16_AX,
				Code.Xchg_r16_AX,
				Code.Xchg_r16_AX,
			},
			{
				Code.Nopd,
				Code.Xchg_r32_EAX,
				Code.Xchg_r32_EAX,
				Code.Xchg_r32_EAX,
				Code.Xchg_r32_EAX,
				Code.Xchg_r32_EAX,
				Code.Xchg_r32_EAX,
				Code.Xchg_r32_EAX,
				Code.Xchg_r32_EAX,
				Code.Xchg_r32_EAX,
				Code.Xchg_r32_EAX,
				Code.Xchg_r32_EAX,
				Code.Xchg_r32_EAX,
				Code.Xchg_r32_EAX,
				Code.Xchg_r32_EAX,
				Code.Xchg_r32_EAX,
			},
			{
				Code.Nopq,
				Code.Xchg_r64_RAX,
				Code.Xchg_r64_RAX,
				Code.Xchg_r64_RAX,
				Code.Xchg_r64_RAX,
				Code.Xchg_r64_RAX,
				Code.Xchg_r64_RAX,
				Code.Xchg_r64_RAX,
				Code.Xchg_r64_RAX,
				Code.Xchg_r64_RAX,
				Code.Xchg_r64_RAX,
				Code.Xchg_r64_RAX,
				Code.Xchg_r64_RAX,
				Code.Xchg_r64_RAX,
				Code.Xchg_r64_RAX,
				Code.Xchg_r64_RAX,
			},
		};

		static readonly Register[,] registers = new Register[3, 16] {
			{
				Register.None,
				Register.CX,
				Register.DX,
				Register.BX,
				Register.SP,
				Register.BP,
				Register.SI,
				Register.DI,
				Register.R8W,
				Register.R9W,
				Register.R10W,
				Register.R11W,
				Register.R12W,
				Register.R13W,
				Register.R14W,
				Register.R15W,
			},
			{
				Register.None,
				Register.ECX,
				Register.EDX,
				Register.EBX,
				Register.ESP,
				Register.EBP,
				Register.ESI,
				Register.EDI,
				Register.R8D,
				Register.R9D,
				Register.R10D,
				Register.R11D,
				Register.R12D,
				Register.R13D,
				Register.R14D,
				Register.R15D,
			},
			{
				Register.None,
				Register.RCX,
				Register.RDX,
				Register.RBX,
				Register.RSP,
				Register.RBP,
				Register.RSI,
				Register.RDI,
				Register.R8,
				Register.R9,
				Register.R10,
				Register.R11,
				Register.R12,
				Register.R13,
				Register.R14,
				Register.R15,
			},
		};

		static readonly Register[] accumulatorRegister = new Register[3] {
			Register.AX,
			Register.EAX,
			Register.RAX,
		};

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);

			if (index == 0 && state.mandatoryPrefix == MandatoryPrefix.PF3) {
				decoder.ClearMandatoryPrefixF3(ref instruction);
				instruction.InternalCode = Code.Pause;
			}
			else {
				Debug.Assert((int)OpSize.Size16 == 0);
				Debug.Assert((int)OpSize.Size32 == 1);
				Debug.Assert((int)OpSize.Size64 == 2);
				int sizeIndex = (int)state.operandSize;
				int codeIndex = index + (int)state.extraBaseRegisterBase;

				instruction.InternalCode = codes[sizeIndex, codeIndex];
				var reg = registers[sizeIndex, codeIndex];
				if (reg != Register.None) {
					Debug.Assert(OpKind.Register == 0);
					//instruction.InternalOp0Kind = OpKind.Register;
					instruction.InternalOp0Register = reg;
					Debug.Assert(OpKind.Register == 0);
					//instruction.InternalOp1Kind = OpKind.Register;
					instruction.InternalOp1Register = accumulatorRegister[sizeIndex];
				}
			}
		}
	}

	sealed class OpCodeHandler_Reg_Iz : OpCodeHandler {
		readonly Code code16;
		readonly Code code32;
		readonly Code code64;
		readonly Register reg16;
		readonly Register reg32;
		readonly Register reg64;

		public OpCodeHandler_Reg_Iz(Code code16, Code code32, Code code64, Register reg16, Register reg32, Register reg64) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
			this.reg16 = reg16;
			this.reg32 = reg32;
			this.reg64 = reg64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if (state.operandSize == OpSize.Size32) {
				instruction.InternalCode = code32;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = reg32;
				instruction.InternalOp1Kind = OpKind.Immediate32;
				instruction.Immediate32 = decoder.ReadUInt32();
			}
			else if (state.operandSize == OpSize.Size64) {
				instruction.InternalCode = code64;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = reg64;
				instruction.InternalOp1Kind = OpKind.Immediate32to64;
				instruction.Immediate32 = decoder.ReadUInt32();
			}
			else {
				instruction.InternalCode = code16;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = reg16;
				instruction.InternalOp1Kind = OpKind.Immediate16;
				instruction.InternalImmediate16 = decoder.ReadUInt16();
			}
		}
	}

	sealed class OpCodeHandler_RegIb3 : OpCodeHandler {
		readonly int index;

		public OpCodeHandler_RegIb3(int index) {
			Debug.Assert(0 <= index && index <= 7);
			this.index = index;
		}

		// Code.Mov_r8_imm8
		static readonly Register[] noRexPrefix = new Register[8] {
			Register.AL,
			Register.CL,
			Register.DL,
			Register.BL,
			Register.AH,
			Register.CH,
			Register.DH,
			Register.BH,
		};

		// Code.Mov_r8_imm8
		static readonly Register[] withRexPrefix = new Register[16] {
			Register.AL,
			Register.CL,
			Register.DL,
			Register.BL,
			Register.SPL,
			Register.BPL,
			Register.SIL,
			Register.DIL,
			Register.R8L,
			Register.R9L,
			Register.R10L,
			Register.R11L,
			Register.R12L,
			Register.R13L,
			Register.R14L,
			Register.R15L,
		};

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			Register register;
			if ((state.flags & StateFlags.HasRex) != 0)
				register = withRexPrefix[index + (int)state.extraBaseRegisterBase];
			else
				register = noRexPrefix[index];
			instruction.InternalCode = Code.Mov_r8_imm8;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = register;
			instruction.InternalOp1Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadIb();
		}
	}

	sealed class OpCodeHandler_RegIz2 : OpCodeHandler {
		readonly int index;

		public OpCodeHandler_RegIz2(int index) {
			Debug.Assert(0 <= index && index <= 7);
			this.index = index;
		}

		// Code.Mov_r16_imm16
		static readonly Register[] info16 = new Register[16] {
			Register.AX,
			Register.CX,
			Register.DX,
			Register.BX,
			Register.SP,
			Register.BP,
			Register.SI,
			Register.DI,
			Register.R8W,
			Register.R9W,
			Register.R10W,
			Register.R11W,
			Register.R12W,
			Register.R13W,
			Register.R14W,
			Register.R15W,
		};

		// Code.Mov_r32_imm32
		static readonly Register[] info32 = new Register[16] {
			Register.EAX,
			Register.ECX,
			Register.EDX,
			Register.EBX,
			Register.ESP,
			Register.EBP,
			Register.ESI,
			Register.EDI,
			Register.R8D,
			Register.R9D,
			Register.R10D,
			Register.R11D,
			Register.R12D,
			Register.R13D,
			Register.R14D,
			Register.R15D,
		};

		// Code.Mov_r64_imm64
		static readonly Register[] info64 = new Register[16] {
			Register.RAX,
			Register.RCX,
			Register.RDX,
			Register.RBX,
			Register.RSP,
			Register.RBP,
			Register.RSI,
			Register.RDI,
			Register.R8,
			Register.R9,
			Register.R10,
			Register.R11,
			Register.R12,
			Register.R13,
			Register.R14,
			Register.R15,
		};

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			Register register;
			int index = this.index + (int)state.extraBaseRegisterBase;
			if (state.operandSize == OpSize.Size32) {
				register = info32[index];
				instruction.InternalCode = Code.Mov_r32_imm32;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = register;
				instruction.InternalOp1Kind = OpKind.Immediate32;
				instruction.Immediate32 = decoder.ReadUInt32();
			}
			else if (state.operandSize == OpSize.Size64) {
				register = info64[index];
				instruction.InternalCode = Code.Mov_r64_imm64;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = register;
				instruction.InternalOp1Kind = OpKind.Immediate64;
				instruction.InternalImmediate64_lo = decoder.ReadUInt32();
				instruction.InternalImmediate64_hi = decoder.ReadUInt32();
			}
			else {
				register = info16[index];
				instruction.InternalCode = Code.Mov_r16_imm16;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = register;
				instruction.InternalOp1Kind = OpKind.Immediate16;
				instruction.InternalImmediate16 = decoder.ReadUInt16();
			}
		}
	}

	sealed class OpCodeHandler_PushIb2 : OpCodeHandler {
		readonly Code code16;
		readonly Code code64;

		public OpCodeHandler_PushIb2(Code code16, Code code64) {
			this.code16 = code16;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if (state.operandSize != OpSize.Size16) {
				instruction.InternalCode = code64;
				instruction.InternalOp0Kind = OpKind.Immediate8to64;
				instruction.InternalImmediate8 = decoder.ReadIb();
			}
			else {
				instruction.InternalCode = code16;
				instruction.InternalOp0Kind = OpKind.Immediate8to16;
				instruction.InternalImmediate8 = decoder.ReadIb();
			}
		}
	}

	sealed class OpCodeHandler_PushIz : OpCodeHandler {
		readonly Code code16;
		readonly Code code64;

		public OpCodeHandler_PushIz(Code code16, Code code64) {
			this.code16 = code16;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if (state.operandSize != OpSize.Size16) {
				instruction.InternalCode = code64;
				instruction.InternalOp0Kind = OpKind.Immediate32to64;
				instruction.Immediate32 = decoder.ReadUInt32();
			}
			else {
				instruction.InternalCode = code16;
				instruction.InternalOp0Kind = OpKind.Immediate16;
				instruction.InternalImmediate16 = decoder.ReadUInt16();
			}
		}
	}

	sealed class OpCodeHandler_Gv_Ev_Ib : OpCodeHandlerModRM {
		readonly Code code16;
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_Gv_Ev_Ib(Code code16, Code code32, Code code64) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				uint index = state.rm + state.extraBaseRegisterBase;
				if (state.operandSize == OpSize.Size32)
					instruction.InternalOp1Register = (int)index + Register.EAX;
				else if (state.operandSize == OpSize.Size64)
					instruction.InternalOp1Register = (int)index + Register.RAX;
				else
					instruction.InternalOp1Register = (int)index + Register.AX;
			}
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				decoder.ReadOpMem_m64(ref instruction);
			}
			if (state.operandSize == OpSize.Size32) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + Register.EAX;
				instruction.InternalCode = code32;
				instruction.InternalOp2Kind = OpKind.Immediate8to32;
				instruction.InternalImmediate8 = decoder.ReadIb();
			}
			else if (state.operandSize == OpSize.Size64) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + Register.RAX;
				instruction.InternalCode = code64;
				instruction.InternalOp2Kind = OpKind.Immediate8to64;
				instruction.InternalImmediate8 = decoder.ReadIb();
			}
			else {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + Register.AX;
				instruction.InternalCode = code16;
				instruction.InternalOp2Kind = OpKind.Immediate8to16;
				instruction.InternalImmediate8 = decoder.ReadIb();
			}
		}
	}

	sealed class OpCodeHandler_Gv_Ev_Ib_REX : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code code32;
		readonly Code code64;
		readonly bool allowMem;

		public OpCodeHandler_Gv_Ev_Ib_REX(Register baseReg, Code code32, Code code64, bool allowMem) {
			this.baseReg = baseReg;
			this.code32 = code32;
			this.code64 = code64;
			this.allowMem = allowMem;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if ((state.flags & StateFlags.W) != 0) {
				instruction.InternalCode = code64;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + Register.RAX;
			}
			else {
				instruction.InternalCode = code32;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + Register.EAX;
			}
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.InternalOp1Register = (int)(state.rm + state.extraBaseRegisterBase) + baseReg;
			}
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				decoder.ReadOpMem_m64(ref instruction);
				if (!allowMem)
					decoder.SetInvalidInstruction();
			}
			instruction.InternalOp2Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadIb();
		}
	}

	sealed class OpCodeHandler_Gv_Ev_32_64 : OpCodeHandlerModRM {
		readonly Code code;
		readonly bool allowReg;
		readonly bool allowMem;

		public OpCodeHandler_Gv_Ev_32_64(Code code32, Code code64, bool allowReg, bool allowMem) {
			code = code64;
			this.allowReg = allowReg;
			this.allowMem = allowMem;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + Register.RAX;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.InternalOp1Register = (int)(state.rm + state.extraBaseRegisterBase) + Register.RAX;
				if (!allowReg)
					decoder.SetInvalidInstruction();
			}
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				decoder.ReadOpMem_m64(ref instruction);
				if (!allowMem)
					decoder.SetInvalidInstruction();
			}
		}
	}

	sealed class OpCodeHandler_Gv_Ev_Iz : OpCodeHandlerModRM {
		readonly Code code16;
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_Gv_Ev_Iz(Code code16, Code code32, Code code64) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				uint index = state.rm + state.extraBaseRegisterBase;
				if (state.operandSize == OpSize.Size32)
					instruction.InternalOp1Register = (int)index + Register.EAX;
				else if (state.operandSize == OpSize.Size64)
					instruction.InternalOp1Register = (int)index + Register.RAX;
				else
					instruction.InternalOp1Register = (int)index + Register.AX;
			}
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				decoder.ReadOpMem_m64(ref instruction);
			}
			if (state.operandSize == OpSize.Size32) {
				instruction.InternalCode = code32;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + Register.EAX;
				instruction.InternalOp2Kind = OpKind.Immediate32;
				instruction.Immediate32 = decoder.ReadUInt32();
			}
			else if (state.operandSize == OpSize.Size64) {
				instruction.InternalCode = code64;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + Register.RAX;
				instruction.InternalOp2Kind = OpKind.Immediate32to64;
				instruction.Immediate32 = decoder.ReadUInt32();
			}
			else {
				instruction.InternalCode = code16;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + Register.AX;
				instruction.InternalOp2Kind = OpKind.Immediate16;
				instruction.InternalImmediate16 = decoder.ReadUInt16();
			}
		}
	}

	sealed class OpCodeHandler_Yb_Reg : OpCodeHandler {
		readonly Code code;
		readonly Register reg;

		public OpCodeHandler_Yb_Reg(Code code, Register reg) {
			this.code = code;
			this.reg = reg;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code;
			if (state.addressSize == OpSize.Size64)
				instruction.InternalOp0Kind = OpKind.MemoryESRDI;
			else
				instruction.InternalOp0Kind = OpKind.MemoryESEDI;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.InternalOp1Register = reg;
		}
	}

	sealed class OpCodeHandler_Yv_Reg : OpCodeHandler {
		readonly Code code16;
		readonly Code code32;
		readonly Code code64;
		readonly Register reg16;
		readonly Register reg32;
		readonly Register reg64;

		public OpCodeHandler_Yv_Reg(Code code16, Code code32, Code code64, Register reg16, Register reg32, Register reg64) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
			this.reg16 = reg16;
			this.reg32 = reg32;
			this.reg64 = reg64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if (state.addressSize == OpSize.Size64)
				instruction.InternalOp0Kind = OpKind.MemoryESRDI;
			else
				instruction.InternalOp0Kind = OpKind.MemoryESEDI;
			if (state.operandSize == OpSize.Size32) {
				instruction.InternalCode = code32;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.InternalOp1Register = reg32;
			}
			else if (state.operandSize == OpSize.Size64) {
				instruction.InternalCode = code64;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.InternalOp1Register = reg64;
			}
			else {
				instruction.InternalCode = code16;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.InternalOp1Register = reg16;
			}
		}
	}

	sealed class OpCodeHandler_Yv_Reg2 : OpCodeHandler {
		readonly Code code16;
		readonly Code code32;
		readonly Register reg16;
		readonly Register reg32;

		public OpCodeHandler_Yv_Reg2(Code code16, Code code32, Register reg16, Register reg32) {
			this.code16 = code16;
			this.code32 = code32;
			this.reg16 = reg16;
			this.reg32 = reg32;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if (state.addressSize == OpSize.Size64)
				instruction.InternalOp0Kind = OpKind.MemoryESRDI;
			else
				instruction.InternalOp0Kind = OpKind.MemoryESEDI;
			if (state.operandSize == OpSize.Size16) {
				instruction.InternalCode = code16;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.InternalOp1Register = reg16;
			}
			else {
				instruction.InternalCode = code32;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.InternalOp1Register = reg32;
			}
		}
	}

	sealed class OpCodeHandler_Reg_Xb : OpCodeHandler {
		readonly Code code;
		readonly Register reg;

		public OpCodeHandler_Reg_Xb(Code code, Register reg) {
			this.code = code;
			this.reg = reg;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = reg;
			if (state.addressSize == OpSize.Size64)
				instruction.InternalOp1Kind = OpKind.MemorySegRSI;
			else
				instruction.InternalOp1Kind = OpKind.MemorySegESI;
		}
	}

	sealed class OpCodeHandler_Reg_Xv : OpCodeHandler {
		readonly Code code16;
		readonly Code code32;
		readonly Code code64;
		readonly Register reg16;
		readonly Register reg32;
		readonly Register reg64;

		public OpCodeHandler_Reg_Xv(Code code16, Code code32, Code code64, Register reg16, Register reg32, Register reg64) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
			this.reg16 = reg16;
			this.reg32 = reg32;
			this.reg64 = reg64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if (state.addressSize == OpSize.Size64)
				instruction.InternalOp1Kind = OpKind.MemorySegRSI;
			else
				instruction.InternalOp1Kind = OpKind.MemorySegESI;
			if (state.operandSize == OpSize.Size32) {
				instruction.InternalCode = code32;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = reg32;
			}
			else if (state.operandSize == OpSize.Size64) {
				instruction.InternalCode = code64;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = reg64;
			}
			else {
				instruction.InternalCode = code16;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = reg16;
			}
		}
	}

	sealed class OpCodeHandler_Reg_Xv2 : OpCodeHandler {
		readonly Code code16;
		readonly Code code32;
		readonly Code code64;
		readonly Register reg16;
		readonly Register reg32;
		readonly Register reg64;

		public OpCodeHandler_Reg_Xv2(Code code16, Code code32, Code code64, Register reg16, Register reg32, Register reg64) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
			this.reg16 = reg16;
			this.reg32 = reg32;
			this.reg64 = reg64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if (state.addressSize == OpSize.Size64)
				instruction.InternalOp1Kind = OpKind.MemorySegRSI;
			else
				instruction.InternalOp1Kind = OpKind.MemorySegESI;
			if (state.operandSize == OpSize.Size32) {
				instruction.InternalCode = code32;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = reg32;
			}
			else if (state.operandSize == OpSize.Size64) {
				instruction.InternalCode = code64;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = reg64;
			}
			else {
				instruction.InternalCode = code16;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = reg16;
			}
		}
	}

	sealed class OpCodeHandler_Reg_Yb : OpCodeHandler {
		readonly Code code;
		readonly Register reg;

		public OpCodeHandler_Reg_Yb(Code code, Register reg) {
			this.code = code;
			this.reg = reg;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = reg;
			if (state.addressSize == OpSize.Size64)
				instruction.InternalOp1Kind = OpKind.MemoryESRDI;
			else
				instruction.InternalOp1Kind = OpKind.MemoryESEDI;
		}
	}

	sealed class OpCodeHandler_Reg_Yv : OpCodeHandler {
		readonly Code code16;
		readonly Code code32;
		readonly Code code64;
		readonly Register reg16;
		readonly Register reg32;
		readonly Register reg64;

		public OpCodeHandler_Reg_Yv(Code code16, Code code32, Code code64, Register reg16, Register reg32, Register reg64) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
			this.reg16 = reg16;
			this.reg32 = reg32;
			this.reg64 = reg64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if (state.addressSize == OpSize.Size64)
				instruction.InternalOp1Kind = OpKind.MemoryESRDI;
			else
				instruction.InternalOp1Kind = OpKind.MemoryESEDI;
			if (state.operandSize == OpSize.Size32) {
				instruction.InternalCode = code32;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = reg32;
			}
			else if (state.operandSize == OpSize.Size64) {
				instruction.InternalCode = code64;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = reg64;
			}
			else {
				instruction.InternalCode = code16;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = reg16;
			}
		}
	}

	sealed class OpCodeHandler_Yb_Xb : OpCodeHandler {
		readonly Code code;

		public OpCodeHandler_Yb_Xb(Code code) => this.code = code;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code;
			if (state.addressSize == OpSize.Size64) {
				instruction.InternalOp0Kind = OpKind.MemoryESRDI;
				instruction.InternalOp1Kind = OpKind.MemorySegRSI;
			}
			else {
				instruction.InternalOp0Kind = OpKind.MemoryESEDI;
				instruction.InternalOp1Kind = OpKind.MemorySegESI;
			}
		}
	}

	sealed class OpCodeHandler_Yv_Xv : OpCodeHandler {
		readonly Code code16;
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_Yv_Xv(Code code16, Code code32, Code code64) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if (state.addressSize == OpSize.Size64) {
				instruction.InternalOp0Kind = OpKind.MemoryESRDI;
				instruction.InternalOp1Kind = OpKind.MemorySegRSI;
			}
			else {
				instruction.InternalOp0Kind = OpKind.MemoryESEDI;
				instruction.InternalOp1Kind = OpKind.MemorySegESI;
			}
			if (state.operandSize == OpSize.Size32) {
				instruction.InternalCode = code32;
			}
			else if (state.operandSize == OpSize.Size64) {
				instruction.InternalCode = code64;
			}
			else {
				instruction.InternalCode = code16;
			}
		}
	}

	sealed class OpCodeHandler_Xb_Yb : OpCodeHandler {
		readonly Code code;

		public OpCodeHandler_Xb_Yb(Code code) => this.code = code;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code;
			if (state.addressSize == OpSize.Size64) {
				instruction.InternalOp0Kind = OpKind.MemorySegRSI;
				instruction.InternalOp1Kind = OpKind.MemoryESRDI;
			}
			else {
				instruction.InternalOp0Kind = OpKind.MemorySegESI;
				instruction.InternalOp1Kind = OpKind.MemoryESEDI;
			}
		}
	}

	sealed class OpCodeHandler_Xv_Yv : OpCodeHandler {
		readonly Code code16;
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_Xv_Yv(Code code16, Code code32, Code code64) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if (state.addressSize == OpSize.Size64) {
				instruction.InternalOp0Kind = OpKind.MemorySegRSI;
				instruction.InternalOp1Kind = OpKind.MemoryESRDI;
			}
			else {
				instruction.InternalOp0Kind = OpKind.MemorySegESI;
				instruction.InternalOp1Kind = OpKind.MemoryESEDI;
			}
			if (state.operandSize == OpSize.Size32)
				instruction.InternalCode = code32;
			else if (state.operandSize == OpSize.Size64)
				instruction.InternalCode = code64;
			else
				instruction.InternalCode = code16;
		}
	}

	sealed class OpCodeHandler_Ev_Sw : OpCodeHandlerModRM {
		readonly Code code16;
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_Ev_Sw(Code code16, Code code32, Code code64) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if (state.operandSize == OpSize.Size32)
				instruction.InternalCode = code32;
			else if (state.operandSize == OpSize.Size64)
				instruction.InternalCode = code64;
			else
				instruction.InternalCode = code16;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				uint index = state.rm + state.extraBaseRegisterBase;
				if (state.operandSize == OpSize.Size32)
					instruction.InternalOp0Register = (int)index + Register.EAX;
				else if (state.operandSize == OpSize.Size64)
					instruction.InternalOp0Register = (int)index + Register.RAX;
				else
					instruction.InternalOp0Register = (int)index + Register.AX;
			}
			else {
				instruction.InternalOp0Kind = OpKind.Memory;
				decoder.ReadOpMem_m64(ref instruction);
			}
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.InternalOp1Register = decoder.ReadOpSw();
		}
	}

	sealed class OpCodeHandler_Gv_M : OpCodeHandlerModRM {
		readonly Code code16;
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_Gv_M(Code code16, Code code32, Code code64) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if (state.operandSize == OpSize.Size32) {
				instruction.InternalCode = code32;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + Register.EAX;
			}
			else if (state.operandSize == OpSize.Size64) {
				instruction.InternalCode = code64;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + Register.RAX;
			}
			else {
				instruction.InternalCode = code16;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + Register.AX;
			}
			if (state.mod == 3)
				decoder.SetInvalidInstruction();
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				decoder.ReadOpMem_m64(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_Sw_Ev : OpCodeHandlerModRM {
		readonly Code code16;
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_Sw_Ev(Code code16, Code code32, Code code64) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if (state.operandSize == OpSize.Size32)
				instruction.InternalCode = code32;
			else if (state.operandSize == OpSize.Size64)
				instruction.InternalCode = code64;
			else
				instruction.InternalCode = code16;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = decoder.ReadOpSw();
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				uint index = state.rm + state.extraBaseRegisterBase;
				if (state.operandSize == OpSize.Size32)
					instruction.InternalOp1Register = (int)index + Register.EAX;
				else if (state.operandSize == OpSize.Size64)
					instruction.InternalOp1Register = (int)index + Register.RAX;
				else
					instruction.InternalOp1Register = (int)index + Register.AX;
			}
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				decoder.ReadOpMem_m64(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_Reg_Ob : OpCodeHandler {
		readonly Code code;
		readonly Register reg;

		public OpCodeHandler_Reg_Ob(Code code, Register reg) {
			this.code = code;
			this.reg = reg;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = reg;
			Debug.Assert(state.addressSize == OpSize.Size32 || state.addressSize == OpSize.Size64);
			decoder.displIndex = state.instructionLength;
			if (state.addressSize == OpSize.Size64) {
				instruction.InternalSetMemoryDisplSize(4);
				state.flags |= StateFlags.Addr64;
				instruction.InternalMemoryAddress64_lo = decoder.ReadUInt32();
				instruction.InternalMemoryAddress64_hi = decoder.ReadUInt32();
				instruction.InternalOp1Kind = OpKind.Memory64;
			}
			else {
				instruction.InternalSetMemoryDisplSize(3);
				instruction.MemoryDisplacement = decoder.ReadUInt32();
				//instruction.InternalMemoryIndexScale = 0;
				//instruction.InternalMemoryBase = Register.None;
				//instruction.InternalMemoryIndex = Register.None;
				instruction.InternalOp1Kind = OpKind.Memory;
			}
		}
	}

	sealed class OpCodeHandler_Ob_Reg : OpCodeHandler {
		readonly Code code;
		readonly Register reg;

		public OpCodeHandler_Ob_Reg(Code code, Register reg) {
			this.code = code;
			this.reg = reg;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code;
			Debug.Assert(state.addressSize == OpSize.Size32 || state.addressSize == OpSize.Size64);
			decoder.displIndex = state.instructionLength;
			if (state.addressSize == OpSize.Size64) {
				instruction.InternalSetMemoryDisplSize(4);
				state.flags |= StateFlags.Addr64;
				instruction.InternalMemoryAddress64_lo = decoder.ReadUInt32();
				instruction.InternalMemoryAddress64_hi = decoder.ReadUInt32();
				instruction.InternalOp0Kind = OpKind.Memory64;
			}
			else {
				instruction.InternalSetMemoryDisplSize(3);
				instruction.MemoryDisplacement = decoder.ReadUInt32();
				//instruction.InternalMemoryIndexScale = 0;
				//instruction.InternalMemoryBase = Register.None;
				//instruction.InternalMemoryIndex = Register.None;
				instruction.InternalOp0Kind = OpKind.Memory;
			}
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.InternalOp1Register = reg;
		}
	}

	sealed class OpCodeHandler_Reg_Ov : OpCodeHandler {
		readonly Code code16;
		readonly Code code32;
		readonly Code code64;
		readonly Register reg16;
		readonly Register reg32;
		readonly Register reg64;

		public OpCodeHandler_Reg_Ov(Code code16, Code code32, Code code64, Register reg16, Register reg32, Register reg64) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
			this.reg16 = reg16;
			this.reg32 = reg32;
			this.reg64 = reg64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			Debug.Assert(state.addressSize == OpSize.Size32 || state.addressSize == OpSize.Size64);
			decoder.displIndex = state.instructionLength;
			if (state.addressSize == OpSize.Size64) {
				instruction.InternalSetMemoryDisplSize(4);
				state.flags |= StateFlags.Addr64;
				instruction.InternalMemoryAddress64_lo = decoder.ReadUInt32();
				instruction.InternalMemoryAddress64_hi = decoder.ReadUInt32();
				instruction.InternalOp1Kind = OpKind.Memory64;
			}
			else {
				instruction.InternalSetMemoryDisplSize(3);
				instruction.MemoryDisplacement = decoder.ReadUInt32();
				//instruction.InternalMemoryIndexScale = 0;
				//instruction.InternalMemoryBase = Register.None;
				//instruction.InternalMemoryIndex = Register.None;
				instruction.InternalOp1Kind = OpKind.Memory;
			}
			if (state.operandSize == OpSize.Size32) {
				instruction.InternalCode = code32;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = reg32;
			}
			else if (state.operandSize == OpSize.Size64) {
				instruction.InternalCode = code64;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = reg64;
			}
			else {
				instruction.InternalCode = code16;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = reg16;
			}
		}
	}

	sealed class OpCodeHandler_Ov_Reg : OpCodeHandler {
		readonly Code code16;
		readonly Code code32;
		readonly Code code64;
		readonly Register reg16;
		readonly Register reg32;
		readonly Register reg64;

		public OpCodeHandler_Ov_Reg(Code code16, Code code32, Code code64, Register reg16, Register reg32, Register reg64) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
			this.reg16 = reg16;
			this.reg32 = reg32;
			this.reg64 = reg64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			decoder.displIndex = state.instructionLength;
			if (state.addressSize == OpSize.Size64) {
				instruction.InternalSetMemoryDisplSize(4);
				state.flags |= StateFlags.Addr64;
				instruction.InternalMemoryAddress64_lo = decoder.ReadUInt32();
				instruction.InternalMemoryAddress64_hi = decoder.ReadUInt32();
				instruction.InternalOp0Kind = OpKind.Memory64;
			}
			else {
				instruction.InternalSetMemoryDisplSize(3);
				instruction.MemoryDisplacement = decoder.ReadUInt32();
				//instruction.InternalMemoryIndexScale = 0;
				//instruction.InternalMemoryBase = Register.None;
				//instruction.InternalMemoryIndex = Register.None;
				instruction.InternalOp0Kind = OpKind.Memory;
			}
			if (state.operandSize == OpSize.Size32) {
				instruction.InternalCode = code32;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.InternalOp1Register = reg32;
			}
			else if (state.operandSize == OpSize.Size64) {
				instruction.InternalCode = code64;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.InternalOp1Register = reg64;
			}
			else {
				instruction.InternalCode = code16;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.InternalOp1Register = reg16;
			}
		}
	}

	sealed class OpCodeHandler_BranchIw : OpCodeHandler {
		readonly Code code16;
		readonly Code code64;

		public OpCodeHandler_BranchIw(Code code16, Code code64) {
			this.code16 = code16;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if ((decoder.options & DecoderOptions.AMD) == 0 || state.operandSize == OpSize.Size32)
				instruction.InternalCode = code64;
			else
				instruction.InternalCode = code16;
			instruction.InternalOp0Kind = OpKind.Immediate16;
			instruction.InternalImmediate16 = decoder.ReadUInt16();
		}
	}

	sealed class OpCodeHandler_BranchSimple : OpCodeHandler {
		readonly Code code16;
		readonly Code code64;

		public OpCodeHandler_BranchSimple(Code code16, Code code64) {
			this.code16 = code16;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			if ((decoder.options & DecoderOptions.AMD) == 0 || decoder.state.operandSize == OpSize.Size32)
				instruction.InternalCode = code64;
			else
				instruction.InternalCode = code16;
		}
	}

	sealed class OpCodeHandler_Iw_Ib : OpCodeHandler {
		readonly Code code16;
		readonly Code code64;

		public OpCodeHandler_Iw_Ib(Code code16, Code code64) {
			this.code16 = code16;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if (state.operandSize != OpSize.Size16)
				instruction.InternalCode = code64;
			else
				instruction.InternalCode = code16;
			instruction.InternalOp0Kind = OpKind.Immediate16;
			instruction.InternalImmediate16 = decoder.ReadUInt16();
			instruction.InternalOp1Kind = OpKind.Immediate8_2nd;
			instruction.InternalImmediate8_2nd = decoder.ReadIb();
		}
	}

	sealed class OpCodeHandler_Reg_Ib2 : OpCodeHandler {
		readonly Code code16;
		readonly Code code32;
		readonly Register reg16;
		readonly Register reg32;

		public OpCodeHandler_Reg_Ib2(Code code16, Code code32, Register reg16, Register reg32) {
			this.code16 = code16;
			this.code32 = code32;
			this.reg16 = reg16;
			this.reg32 = reg32;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			if (state.operandSize != OpSize.Size16) {
				instruction.InternalCode = code32;
				instruction.InternalOp0Register = reg32;
			}
			else {
				instruction.InternalCode = code16;
				instruction.InternalOp0Register = reg16;
			}
			instruction.InternalOp1Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadIb();
		}
	}

	sealed class OpCodeHandler_IbReg2 : OpCodeHandler {
		readonly Code code16;
		readonly Code code32;
		readonly Register reg16;
		readonly Register reg32;

		public OpCodeHandler_IbReg2(Code code16, Code code32, Register reg16, Register reg32) {
			this.code16 = code16;
			this.code32 = code32;
			this.reg16 = reg16;
			this.reg32 = reg32;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalOp0Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadIb();
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			if (state.operandSize != OpSize.Size16) {
				instruction.InternalCode = code32;
				instruction.InternalOp1Register = reg32;
			}
			else {
				instruction.InternalCode = code16;
				instruction.InternalOp1Register = reg16;
			}
		}
	}

	sealed class OpCodeHandler_eAX_DX : OpCodeHandler {
		readonly Code code16;
		readonly Code code32;

		public OpCodeHandler_eAX_DX(Code code16, Code code32) {
			this.code16 = code16;
			this.code32 = code32;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if (state.operandSize != OpSize.Size16) {
				instruction.InternalCode = code32;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = Register.EAX;
			}
			else {
				instruction.InternalCode = code16;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = Register.AX;
			}
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.InternalOp1Register = Register.DX;
		}
	}

	sealed class OpCodeHandler_DX_eAX : OpCodeHandler {
		readonly Code code16;
		readonly Code code32;

		public OpCodeHandler_DX_eAX(Code code16, Code code32) {
			this.code16 = code16;
			this.code32 = code32;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = Register.DX;
			if (state.operandSize != OpSize.Size16) {
				instruction.InternalCode = code32;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.InternalOp1Register = Register.EAX;
			}
			else {
				instruction.InternalCode = code16;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.InternalOp1Register = Register.AX;
			}
		}
	}

	sealed class OpCodeHandler_Eb_Ib : OpCodeHandlerModRM {
		readonly Code code;
		readonly HandlerFlags flags;

		public OpCodeHandler_Eb_Ib(Code code) => this.code = code;

		public OpCodeHandler_Eb_Ib(Code code, HandlerFlags flags) {
			this.code = code;
			this.flags = flags;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code;
			if (state.mod == 3) {
				uint index = state.rm + state.extraBaseRegisterBase;
				if ((state.flags & StateFlags.HasRex) != 0 && index >= 4)
					index += 4;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)index + Register.AL;
			}
			else {
				instruction.InternalOp0Kind = OpKind.Memory;
				decoder.ReadOpMem_m64(ref instruction);
				if ((flags & HandlerFlags.XacquireRelease) != 0)
					decoder.SetXacquireRelease(ref instruction, flags);
			}
			instruction.InternalOp1Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadIb();
		}
	}

	sealed class OpCodeHandler_Eb_1 : OpCodeHandlerModRM {
		readonly Code code;

		public OpCodeHandler_Eb_1(Code code) => this.code = code;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code;
			if (state.mod == 3) {
				uint index = state.rm + state.extraBaseRegisterBase;
				if ((state.flags & StateFlags.HasRex) != 0 && index >= 4)
					index += 4;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)index + Register.AL;
			}
			else {
				instruction.InternalOp0Kind = OpKind.Memory;
				decoder.ReadOpMem_m64(ref instruction);
			}
			instruction.InternalOp1Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = 1;
			state.flags |= StateFlags.NoImm;
		}
	}

	sealed class OpCodeHandler_Eb_CL : OpCodeHandlerModRM {
		readonly Code code;

		public OpCodeHandler_Eb_CL(Code code) => this.code = code;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code;
			if (state.mod == 3) {
				uint index = state.rm + state.extraBaseRegisterBase;
				if ((state.flags & StateFlags.HasRex) != 0 && index >= 4)
					index += 4;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)index + Register.AL;
			}
			else {
				instruction.InternalOp0Kind = OpKind.Memory;
				decoder.ReadOpMem_m64(ref instruction);
			}
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.InternalOp1Register = Register.CL;
		}
	}

	sealed class OpCodeHandler_Eb : OpCodeHandlerModRM {
		readonly Code code;
		readonly HandlerFlags flags;

		public OpCodeHandler_Eb(Code code) => this.code = code;

		public OpCodeHandler_Eb(Code code, HandlerFlags flags) {
			this.code = code;
			this.flags = flags;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code;
			if (state.mod == 3) {
				uint index = state.rm + state.extraBaseRegisterBase;
				if ((state.flags & StateFlags.HasRex) != 0 && index >= 4)
					index += 4;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)index + Register.AL;
			}
			else {
				instruction.InternalOp0Kind = OpKind.Memory;
				decoder.ReadOpMem_m64(ref instruction);
				if ((flags & HandlerFlags.XacquireRelease) != 0)
					decoder.SetXacquireRelease(ref instruction, flags);
			}
		}
	}

	sealed class OpCodeHandler_Eb_Gb : OpCodeHandlerModRM {
		readonly Code code;
		readonly HandlerFlags flags;

		public OpCodeHandler_Eb_Gb(Code code) => this.code = code;

		public OpCodeHandler_Eb_Gb(Code code, HandlerFlags flags) {
			this.code = code;
			this.flags = flags;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code;
			uint index;
			if (state.mod == 3) {
				index = state.rm + state.extraBaseRegisterBase;
				if ((state.flags & StateFlags.HasRex) != 0 && index >= 4)
					index += 4;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)index + Register.AL;
			}
			else {
				instruction.InternalOp0Kind = OpKind.Memory;
				decoder.ReadOpMem_m64(ref instruction);
				if ((flags & HandlerFlags.XacquireRelease) != 0)
					decoder.SetXacquireRelease(ref instruction, flags);
			}
			index = state.reg + state.extraRegisterBase;
			if ((state.flags & StateFlags.HasRex) != 0 && index >= 4)
				index += 4;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.InternalOp1Register = (int)index + Register.AL;
		}
	}

	sealed class OpCodeHandler_Gb_Eb : OpCodeHandlerModRM {
		readonly Code code;

		public OpCodeHandler_Gb_Eb(Code code) => this.code = code;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code;
			uint index = state.reg + state.extraRegisterBase;
			if ((state.flags & StateFlags.HasRex) != 0 && index >= 4)
				index += 4;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = (int)index + Register.AL;

			if (state.mod == 3) {
				index = state.rm + state.extraBaseRegisterBase;
				if ((state.flags & StateFlags.HasRex) != 0 && index >= 4)
					index += 4;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.InternalOp1Register = (int)index + Register.AL;
			}
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				decoder.ReadOpMem_m64(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_M : OpCodeHandlerModRM {
		readonly Code codeW0;
		readonly Code codeW1;

		public OpCodeHandler_M(Code codeW0, Code codeW1) {
			this.codeW0 = codeW0;
			this.codeW1 = codeW1;
		}

		public OpCodeHandler_M(Code codeW0) {
			this.codeW0 = codeW0;
			codeW1 = codeW0;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if ((state.flags & StateFlags.W) != 0)
				instruction.InternalCode = codeW1;
			else
				instruction.InternalCode = codeW0;
			if (state.mod == 3)
				decoder.SetInvalidInstruction();
			else {
				instruction.InternalOp0Kind = OpKind.Memory;
				decoder.ReadOpMem_m64(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_M_REXW : OpCodeHandlerModRM {
		readonly Code code32;
		readonly Code code64;
		readonly HandlerFlags flags32;
		readonly HandlerFlags flags64;

		public OpCodeHandler_M_REXW(Code code32, Code code64) {
			this.code32 = code32;
			this.code64 = code64;
		}

		public OpCodeHandler_M_REXW(Code code32, Code code64, HandlerFlags flags32, HandlerFlags flags64) {
			this.code32 = code32;
			this.code64 = code64;
			this.flags32 = flags32;
			this.flags64 = flags64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if ((state.flags & StateFlags.W) != 0)
				instruction.InternalCode = code64;
			else
				instruction.InternalCode = code32;
			if (state.mod == 3)
				decoder.SetInvalidInstruction();
			else {
				instruction.InternalOp0Kind = OpKind.Memory;
				HandlerFlags flags;
				if ((state.flags & StateFlags.W) != 0)
					flags = flags64;
				else
					flags = flags32;
				decoder.ReadOpMem_m64(ref instruction);
				if ((flags & HandlerFlags.XacquireRelease) != 0)
					decoder.SetXacquireRelease(ref instruction, flags);
			}
		}
	}

	sealed class OpCodeHandler_MemBx : OpCodeHandler {
		readonly Code code;

		public OpCodeHandler_MemBx(Code code) => this.code = code;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code;
			//instruction.MemoryDisplacement = 0;
			//instruction.InternalMemoryIndexScale = 0;
			//instruction.InternalSetMemoryDisplSize(0);
			if (state.addressSize == OpSize.Size64)
				instruction.InternalMemoryBase = Register.RBX;
			else {
				Debug.Assert(state.addressSize == OpSize.Size32);
				instruction.InternalMemoryBase = Register.EBX;
			}
			instruction.InternalMemoryIndex = Register.AL;
			instruction.InternalOp0Kind = OpKind.Memory;
		}
	}

	sealed class OpCodeHandler_VW : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code codeR;
		readonly Code codeM;

		public OpCodeHandler_VW(Register baseReg, Code codeR, Code codeM) {
			this.baseReg = baseReg;
			this.codeR = codeR;
			this.codeM = codeM;
		}

		public OpCodeHandler_VW(Register baseReg, Code code) {
			this.baseReg = baseReg;
			codeR = code;
			codeM = code;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + baseReg;
			if (state.mod == 3) {
				instruction.InternalCode = codeR;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.InternalOp1Register = (int)(state.rm + state.extraBaseRegisterBase) + baseReg;
			}
			else {
				instruction.InternalCode = codeM;
				instruction.InternalOp1Kind = OpKind.Memory;
				decoder.ReadOpMem_m64(ref instruction);
				if (codeM == Code.INVALID)
					decoder.SetInvalidInstruction();
			}
		}
	}

	sealed class OpCodeHandler_WV : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code code;

		public OpCodeHandler_WV(Register baseReg, Code code) {
			this.baseReg = baseReg;
			this.code = code;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.rm + state.extraBaseRegisterBase) + baseReg;
			}
			else {
				instruction.InternalOp0Kind = OpKind.Memory;
				decoder.ReadOpMem_m64(ref instruction);
			}
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.InternalOp1Register = (int)(state.reg + state.extraRegisterBase) + baseReg;
		}
	}

	sealed class OpCodeHandler_rDI_VX_RX : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code code;

		public OpCodeHandler_rDI_VX_RX(Register baseReg, Code code) {
			this.baseReg = baseReg;
			this.code = code;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code;
			Debug.Assert(state.addressSize == OpSize.Size32 || state.addressSize == OpSize.Size64);
			if (state.addressSize == OpSize.Size64)
				instruction.InternalOp0Kind = OpKind.MemorySegRDI;
			else
				instruction.InternalOp0Kind = OpKind.MemorySegEDI;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.InternalOp1Register = (int)(state.reg + state.extraRegisterBase) + baseReg;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp2Kind = OpKind.Register;
				instruction.InternalOp2Register = (int)(state.rm + state.extraBaseRegisterBase) + baseReg;
			}
			else
				decoder.SetInvalidInstruction();
		}
	}

	sealed class OpCodeHandler_rDI_P_N : OpCodeHandlerModRM {
		readonly Code code;

		public OpCodeHandler_rDI_P_N(Code code) {
			this.code = code;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code;
			Debug.Assert(state.addressSize == OpSize.Size32 || state.addressSize == OpSize.Size64);
			if (state.addressSize == OpSize.Size64)
				instruction.InternalOp0Kind = OpKind.MemorySegRDI;
			else
				instruction.InternalOp0Kind = OpKind.MemorySegEDI;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.InternalOp1Register = (int)state.reg + Register.MM0;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp2Kind = OpKind.Register;
				instruction.InternalOp2Register = (int)state.rm + Register.MM0;
			}
			else
				decoder.SetInvalidInstruction();
		}
	}

	sealed class OpCodeHandler_VM : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code code;

		public OpCodeHandler_VM(Register baseReg, Code code) {
			this.baseReg = baseReg;
			this.code = code;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + baseReg;
			if (state.mod == 3)
				decoder.SetInvalidInstruction();
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				decoder.ReadOpMem_m64(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_MV : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code code;

		public OpCodeHandler_MV(Register baseReg, Code code) {
			this.baseReg = baseReg;
			this.code = code;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code;
			if (state.mod == 3)
				decoder.SetInvalidInstruction();
			else {
				instruction.InternalOp0Kind = OpKind.Memory;
				decoder.ReadOpMem_m64(ref instruction);
			}
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.InternalOp1Register = (int)(state.reg + state.extraRegisterBase) + baseReg;
		}
	}

	sealed class OpCodeHandler_VQ : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code code;

		public OpCodeHandler_VQ(Register baseReg, Code code) {
			this.baseReg = baseReg;
			this.code = code;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + baseReg;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.InternalOp1Register = (int)state.rm + Register.MM0;
			}
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				decoder.ReadOpMem_m64(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_P_Q : OpCodeHandlerModRM {
		readonly Code code;

		public OpCodeHandler_P_Q(Code code) {
			this.code = code;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = (int)state.reg + Register.MM0;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.InternalOp1Register = (int)state.rm + Register.MM0;
			}
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				decoder.ReadOpMem_m64(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_Q_P : OpCodeHandlerModRM {
		readonly Code code;

		public OpCodeHandler_Q_P(Code code) {
			this.code = code;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)state.rm + Register.MM0;
			}
			else {
				instruction.InternalOp0Kind = OpKind.Memory;
				decoder.ReadOpMem_m64(ref instruction);
			}
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.InternalOp1Register = (int)state.reg + Register.MM0;
		}
	}

	sealed class OpCodeHandler_MP : OpCodeHandlerModRM {
		readonly Code code;

		public OpCodeHandler_MP(Code code) {
			this.code = code;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code;
			if (state.mod == 3)
				decoder.SetInvalidInstruction();
			else {
				instruction.InternalOp0Kind = OpKind.Memory;
				decoder.ReadOpMem_m64(ref instruction);
			}
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.InternalOp1Register = (int)state.reg + Register.MM0;
		}
	}

	sealed class OpCodeHandler_P_Q_Ib : OpCodeHandlerModRM {
		readonly Code code;

		public OpCodeHandler_P_Q_Ib(Code code) {
			this.code = code;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = (int)state.reg + Register.MM0;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.InternalOp1Register = (int)state.rm + Register.MM0;
			}
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				decoder.ReadOpMem_m64(ref instruction);
			}
			instruction.InternalOp2Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadByte();
		}
	}

	sealed class OpCodeHandler_P_W : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code code;

		public OpCodeHandler_P_W(Register baseReg, Code code) {
			this.baseReg = baseReg;
			this.code = code;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = (int)state.reg + Register.MM0;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.InternalOp1Register = (int)(state.rm + state.extraBaseRegisterBase) + baseReg;
			}
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				decoder.ReadOpMem_m64(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_P_R : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code code;

		public OpCodeHandler_P_R(Register baseReg, Code code) {
			this.baseReg = baseReg;
			this.code = code;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = (int)state.reg + Register.MM0;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.InternalOp1Register = (int)(state.rm + state.extraBaseRegisterBase) + baseReg;
			}
			else
				decoder.SetInvalidInstruction();
		}
	}

	sealed class OpCodeHandler_P_Ev : OpCodeHandlerModRM {
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_P_Ev(Code code32, Code code64) {
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			Register gpr;
			if ((state.flags & StateFlags.W) != 0) {
				instruction.InternalCode = code64;
				gpr = Register.RAX;
			}
			else {
				instruction.InternalCode = code32;
				gpr = Register.EAX;
			}
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = (int)state.reg + Register.MM0;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.InternalOp1Register = (int)(state.rm + state.extraBaseRegisterBase) + gpr;
			}
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				decoder.ReadOpMem_m64(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_P_Ev_Ib : OpCodeHandlerModRM {
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_P_Ev_Ib(Code code32, Code code64) {
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			Register gpr;
			if ((state.flags & StateFlags.W) != 0) {
				instruction.InternalCode = code64;
				gpr = Register.RAX;
			}
			else {
				instruction.InternalCode = code32;
				gpr = Register.EAX;
			}
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = (int)state.reg + Register.MM0;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.InternalOp1Register = (int)(state.rm + state.extraBaseRegisterBase) + gpr;
			}
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				decoder.ReadOpMem_m64(ref instruction);
			}
			instruction.InternalOp2Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadByte();
		}
	}

	sealed class OpCodeHandler_Ev_P : OpCodeHandlerModRM {
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_Ev_P(Code code32, Code code64) {
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			Register gpr;
			if ((state.flags & StateFlags.W) != 0) {
				instruction.InternalCode = code64;
				gpr = Register.RAX;
			}
			else {
				instruction.InternalCode = code32;
				gpr = Register.EAX;
			}
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.rm + state.extraBaseRegisterBase) + gpr;
			}
			else {
				instruction.InternalOp0Kind = OpKind.Memory;
				decoder.ReadOpMem_m64(ref instruction);
			}
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.InternalOp1Register = (int)state.reg + Register.MM0;
		}
	}

	sealed class OpCodeHandler_Gv_W : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code codeW0;
		readonly Code codeW1;

		public OpCodeHandler_Gv_W(Register baseReg, Code codeW0, Code codeW1) {
			this.baseReg = baseReg;
			this.codeW0 = codeW0;
			this.codeW1 = codeW1;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if ((state.flags & StateFlags.W) != 0) {
				instruction.InternalCode = codeW1;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + Register.RAX;
			}
			else {
				instruction.InternalCode = codeW0;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + Register.EAX;
			}
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.InternalOp1Register = (int)(state.rm + state.extraBaseRegisterBase) + baseReg;
			}
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				decoder.ReadOpMem_m64(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_V_Ev : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code codeW0;
		readonly Code codeW1;

		public OpCodeHandler_V_Ev(Register baseReg, Code codeW0, Code codeW1) {
			this.baseReg = baseReg;
			this.codeW0 = codeW0;
			this.codeW1 = codeW1;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if (state.operandSize == OpSize.Size64)
				instruction.InternalCode = codeW1;
			else
				instruction.InternalCode = codeW0;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + baseReg;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.InternalOp1Register = (int)(state.rm + state.extraBaseRegisterBase) + (state.operandSize == OpSize.Size64 ? Register.RAX : Register.EAX);
			}
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				decoder.ReadOpMem_m64(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_VWIb : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code codeW0;
		readonly Code codeW1;

		public OpCodeHandler_VWIb(Register baseReg, Code code) {
			this.baseReg = baseReg;
			codeW0 = code;
			codeW1 = code;
		}

		public OpCodeHandler_VWIb(Register baseReg, Code codeW0, Code codeW1) {
			this.baseReg = baseReg;
			this.codeW0 = codeW0;
			this.codeW1 = codeW1;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if ((state.flags & StateFlags.W) != 0)
				instruction.InternalCode = codeW1;
			else
				instruction.InternalCode = codeW0;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + baseReg;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.InternalOp1Register = (int)(state.rm + state.extraBaseRegisterBase) + baseReg;
			}
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				decoder.ReadOpMem_m64(ref instruction);
			}
			instruction.InternalOp2Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadByte();
		}
	}

	sealed class OpCodeHandler_VRIbIb : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code code;

		public OpCodeHandler_VRIbIb(Register baseReg, Code code) {
			this.baseReg = baseReg;
			this.code = code;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + baseReg;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.InternalOp1Register = (int)(state.rm + state.extraBaseRegisterBase) + baseReg;
			}
			else
				decoder.SetInvalidInstruction();
			instruction.InternalOp2Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadByte();
			instruction.InternalOp3Kind = OpKind.Immediate8_2nd;
			instruction.InternalImmediate8_2nd = decoder.ReadByte();
		}
	}

	sealed class OpCodeHandler_RIbIb : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code code;

		public OpCodeHandler_RIbIb(Register baseReg, Code code) {
			this.baseReg = baseReg;
			this.code = code;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.rm + state.extraBaseRegisterBase) + baseReg;
			}
			else
				decoder.SetInvalidInstruction();
			instruction.InternalOp1Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadByte();
			instruction.InternalOp2Kind = OpKind.Immediate8_2nd;
			instruction.InternalImmediate8_2nd = decoder.ReadByte();
		}
	}

	sealed class OpCodeHandler_RIb : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code code;

		public OpCodeHandler_RIb(Register baseReg, Code code) {
			this.baseReg = baseReg;
			this.code = code;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.rm + state.extraBaseRegisterBase) + baseReg;
			}
			else
				decoder.SetInvalidInstruction();
			instruction.InternalOp1Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadByte();
		}
	}

	sealed class OpCodeHandler_Ed_V_Ib : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_Ed_V_Ib(Register baseReg, Code code32, Code code64) {
			this.baseReg = baseReg;
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			Register gpr;
			if ((state.flags & StateFlags.W) != 0) {
				instruction.InternalCode = code64;
				gpr = Register.RAX;
			}
			else {
				instruction.InternalCode = code32;
				gpr = Register.EAX;
			}
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.rm + state.extraBaseRegisterBase) + gpr;
			}
			else {
				instruction.InternalOp0Kind = OpKind.Memory;
				decoder.ReadOpMem_m64(ref instruction);
			}
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.InternalOp1Register = (int)(state.reg + state.extraRegisterBase) + baseReg;
			instruction.InternalOp2Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadByte();
		}
	}

	sealed class OpCodeHandler_VX_Ev : OpCodeHandlerModRM {
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_VX_Ev(Code code32, Code code64) {
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			Register gpr;
			if ((state.flags & StateFlags.W) != 0) {
				instruction.InternalCode = code64;
				gpr = Register.RAX;
			}
			else {
				instruction.InternalCode = code32;
				gpr = Register.EAX;
			}
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + Register.XMM0;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.InternalOp1Register = (int)(state.rm + state.extraBaseRegisterBase) + gpr;
			}
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				decoder.ReadOpMem_m64(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_Ev_VX : OpCodeHandlerModRM {
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_Ev_VX(Code code32, Code code64) {
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			Register gpr;
			if ((state.flags & StateFlags.W) != 0) {
				instruction.InternalCode = code64;
				gpr = Register.RAX;
			}
			else {
				instruction.InternalCode = code32;
				gpr = Register.EAX;
			}
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.rm + state.extraBaseRegisterBase) + gpr;
			}
			else {
				instruction.InternalOp0Kind = OpKind.Memory;
				decoder.ReadOpMem_m64(ref instruction);
			}
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.InternalOp1Register = (int)(state.reg + state.extraRegisterBase) + Register.XMM0;
		}
	}

	sealed class OpCodeHandler_VX_E_Ib : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_VX_E_Ib(Register baseReg, Code code32, Code code64) {
			this.baseReg = baseReg;
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			Register gpr;
			if ((state.flags & StateFlags.W) != 0) {
				instruction.InternalCode = code64;
				gpr = Register.RAX;
			}
			else {
				instruction.InternalCode = code32;
				gpr = Register.EAX;
			}
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + baseReg;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.InternalOp1Register = (int)(state.rm + state.extraBaseRegisterBase) + gpr;
			}
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				decoder.ReadOpMem_m64(ref instruction);
			}
			instruction.InternalOp2Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadIb();
		}
	}

	sealed class OpCodeHandler_Gv_RX : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_Gv_RX(Register baseReg, Code code32, Code code64) {
			this.baseReg = baseReg;
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if ((state.flags & StateFlags.W) != 0)
				instruction.InternalCode = code64;
			else
				instruction.InternalCode = code32;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + ((state.flags & StateFlags.W) != 0 ? Register.RAX : Register.EAX);
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.InternalOp1Register = (int)(state.rm + state.extraBaseRegisterBase) + baseReg;
			}
			else
				decoder.SetInvalidInstruction();
		}
	}

	sealed class OpCodeHandler_B_MIB : OpCodeHandlerModRM {
		readonly Code code;

		public OpCodeHandler_B_MIB(Code code) => this.code = code;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = (int)(state.reg & 3) + Register.BND0;
			if (state.mod == 3) {
				// Should never be reached
				decoder.SetInvalidInstruction();
			}
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				decoder.ReadOpMem_m64_FORCE64(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_MIB_B : OpCodeHandlerModRM {
		readonly Code code;

		public OpCodeHandler_MIB_B(Code code) => this.code = code;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code;
			if (state.mod == 3) {
				// Should never be reached
				decoder.SetInvalidInstruction();
			}
			else {
				instruction.InternalOp0Kind = OpKind.Memory;
				decoder.ReadOpMem_m64_FORCE64(ref instruction);
			}
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.InternalOp1Register = (int)(state.reg & 3) + Register.BND0;
		}
	}

	sealed class OpCodeHandler_B_BM : OpCodeHandlerModRM {
		readonly Code code64;

		public OpCodeHandler_B_BM(Code code32, Code code64) =>
			this.code64 = code64;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code64;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = (int)(state.reg & 3) + Register.BND0;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.InternalOp1Register = (int)(state.rm & 3) + Register.BND0;
			}
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				decoder.ReadOpMem_m64_FORCE64(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_BM_B : OpCodeHandlerModRM {
		readonly Code code64;

		public OpCodeHandler_BM_B(Code code32, Code code64) {
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code64;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.rm & 3) + Register.BND0;
			}
			else {
				instruction.InternalOp0Kind = OpKind.Memory;
				decoder.ReadOpMem_m64_FORCE64(ref instruction);
			}
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.InternalOp1Register = (int)(state.reg & 3) + Register.BND0;
		}
	}

	sealed class OpCodeHandler_B_Ev : OpCodeHandlerModRM {
		readonly Code code64;

		public OpCodeHandler_B_Ev(Code code32, Code code64) => this.code64 = code64;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code64;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = (int)(state.reg & 3) + Register.BND0;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.InternalOp1Register = (int)(state.rm + state.extraBaseRegisterBase) + Register.RAX;
			}
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				decoder.ReadOpMem_m64_FORCE64(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_Mv_Gv_REXW : OpCodeHandlerModRM {
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_Mv_Gv_REXW(Code code32, Code code64) {
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if (state.mod == 3)
				decoder.SetInvalidInstruction();
			else {
				instruction.InternalOp0Kind = OpKind.Memory;
				decoder.ReadOpMem_m64(ref instruction);
			}
			if ((state.flags & StateFlags.W) != 0) {
				instruction.InternalCode = code64;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.InternalOp1Register = (int)(state.reg + state.extraRegisterBase) + Register.RAX;
			}
			else {
				instruction.InternalCode = code32;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.InternalOp1Register = (int)(state.reg + state.extraRegisterBase) + Register.EAX;
			}
		}
	}

	sealed class OpCodeHandler_Gv_N_Ib_REX : OpCodeHandlerModRM {
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_Gv_N_Ib_REX(Code code32, Code code64) {
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if ((state.flags & StateFlags.W) != 0)
				instruction.InternalCode = code64;
			else
				instruction.InternalCode = code32;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + ((state.flags & StateFlags.W) != 0 ? Register.RAX : Register.EAX);
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.InternalOp1Register = (int)state.rm + Register.MM0;
			}
			else
				decoder.SetInvalidInstruction();
			instruction.InternalOp2Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadIb();
		}
	}

	sealed class OpCodeHandler_Gv_N : OpCodeHandlerModRM {
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_Gv_N(Code code32, Code code64) {
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if ((state.flags & StateFlags.W) != 0)
				instruction.InternalCode = code64;
			else
				instruction.InternalCode = code32;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + ((state.flags & StateFlags.W) != 0 ? Register.RAX : Register.EAX);
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.InternalOp1Register = (int)state.rm + Register.MM0;
			}
			else
				decoder.SetInvalidInstruction();
		}
	}

	sealed class OpCodeHandler_VN : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code code;

		public OpCodeHandler_VN(Register baseReg, Code code) {
			this.baseReg = baseReg;
			this.code = code;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + baseReg;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.InternalOp1Register = (int)state.rm + Register.MM0;
			}
			else
				decoder.SetInvalidInstruction();
		}
	}

	sealed class OpCodeHandler_Gv_Mv : OpCodeHandlerModRM {
		readonly Code code16;
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_Gv_Mv(Code code16, Code code32, Code code64) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if (state.operandSize == OpSize.Size32) {
				instruction.InternalCode = code32;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + Register.EAX;
			}
			else if (state.operandSize == OpSize.Size64) {
				instruction.InternalCode = code64;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + Register.RAX;
			}
			else {
				instruction.InternalCode = code16;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + Register.AX;
			}
			if (state.mod == 3)
				decoder.SetInvalidInstruction();
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				decoder.ReadOpMem_m64(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_Mv_Gv : OpCodeHandlerModRM {
		readonly Code code16;
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_Mv_Gv(Code code16, Code code32, Code code64) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if (state.mod == 3)
				decoder.SetInvalidInstruction();
			else {
				instruction.InternalOp0Kind = OpKind.Memory;
				decoder.ReadOpMem_m64(ref instruction);
			}
			if (state.operandSize == OpSize.Size32) {
				instruction.InternalCode = code32;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.InternalOp1Register = (int)(state.reg + state.extraRegisterBase) + Register.EAX;
			}
			else if (state.operandSize == OpSize.Size64) {
				instruction.InternalCode = code64;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.InternalOp1Register = (int)(state.reg + state.extraRegisterBase) + Register.RAX;
			}
			else {
				instruction.InternalCode = code16;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.InternalOp1Register = (int)(state.reg + state.extraRegisterBase) + Register.AX;
			}
		}
	}

	sealed class OpCodeHandler_Gv_Eb_REX : OpCodeHandlerModRM {
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_Gv_Eb_REX(Code code32, Code code64) {
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if ((state.flags & StateFlags.W) != 0) {
				instruction.InternalCode = code64;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + Register.RAX;
			}
			else {
				instruction.InternalCode = code32;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + Register.EAX;
			}
			if (state.mod == 3) {
				uint index = state.rm + state.extraBaseRegisterBase;
				if ((state.flags & StateFlags.HasRex) != 0 && index >= 4)
					index += 4;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.InternalOp1Register = (int)index + Register.AL;
			}
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				decoder.ReadOpMem_m64(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_Gv_Ev_REX : OpCodeHandlerModRM {
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_Gv_Ev_REX(Code code32, Code code64) {
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if ((state.flags & StateFlags.W) != 0) {
				instruction.InternalCode = code64;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + Register.RAX;
			}
			else {
				instruction.InternalCode = code32;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + Register.EAX;
			}
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.InternalOp1Register = (int)(state.rm + state.extraBaseRegisterBase) + ((state.flags & StateFlags.W) != 0 ? Register.RAX : Register.EAX);
			}
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				decoder.ReadOpMem_m64(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_Ev_Gv_REX : OpCodeHandlerModRM {
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_Ev_Gv_REX(Code code32, Code code64) {
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.rm + state.extraBaseRegisterBase) + ((state.flags & StateFlags.W) != 0 ? Register.RAX : Register.EAX);
			}
			else {
				instruction.InternalOp0Kind = OpKind.Memory;
				decoder.ReadOpMem_m64(ref instruction);
			}
			if ((state.flags & StateFlags.W) != 0) {
				instruction.InternalCode = code64;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.InternalOp1Register = (int)(state.reg + state.extraRegisterBase) + Register.RAX;
			}
			else {
				instruction.InternalCode = code32;
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.InternalOp1Register = (int)(state.reg + state.extraRegisterBase) + Register.EAX;
			}
		}
	}

	sealed class OpCodeHandler_GvM_VX_Ib : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_GvM_VX_Ib(Register baseReg, Code code32, Code code64) {
			this.baseReg = baseReg;
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			Register gpr;
			if ((state.flags & StateFlags.W) != 0) {
				instruction.InternalCode = code64;
				gpr = Register.RAX;
			}
			else {
				instruction.InternalCode = code32;
				gpr = Register.EAX;
			}
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.rm + state.extraBaseRegisterBase) + gpr;
			}
			else {
				instruction.InternalOp0Kind = OpKind.Memory;
				decoder.ReadOpMem_m64(ref instruction);
			}
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.InternalOp1Register = (int)(state.reg + state.extraRegisterBase) + baseReg;
			instruction.InternalOp2Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadIb();
		}
	}

	sealed class OpCodeHandler_ESrSI : OpCodeHandlerModRM {
		readonly Code code;

		public OpCodeHandler_ESrSI(Code code) => this.code = code;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code;
			if (state.addressSize == OpSize.Size64)
				instruction.InternalOp0Kind = OpKind.MemoryESRSI;
			else
				instruction.InternalOp0Kind = OpKind.MemoryESESI;
		}
	}

	sealed class OpCodeHandler_ESrDI : OpCodeHandlerModRM {
		readonly Code code;

		public OpCodeHandler_ESrDI(Code code) => this.code = code;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code;
			if (state.addressSize == OpSize.Size64)
				instruction.InternalOp0Kind = OpKind.MemoryESRDI;
			else
				instruction.InternalOp0Kind = OpKind.MemoryESEDI;
		}
	}
}
#endif
