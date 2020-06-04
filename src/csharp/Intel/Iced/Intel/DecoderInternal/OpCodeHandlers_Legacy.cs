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

#if DECODER
using System;
using System.Diagnostics;

namespace Iced.Intel.DecoderInternal {
	sealed class OpCodeHandler_VEX2 : OpCodeHandlerModRM {
		readonly OpCodeHandler handlerMem;

		public OpCodeHandler_VEX2(OpCodeHandler handlerMem) => this.handlerMem = handlerMem ?? throw new ArgumentNullException(nameof(handlerMem));

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			if (decoder.is64Mode)
				decoder.VEX2(ref instruction);
			else if (decoder.state.mod == 3)
				decoder.VEX2(ref instruction);
			else
				handlerMem.Decode(decoder, ref instruction);
		}
	}

	sealed class OpCodeHandler_VEX3 : OpCodeHandlerModRM {
		readonly OpCodeHandler handlerMem;

		public OpCodeHandler_VEX3(OpCodeHandler handlerMem) => this.handlerMem = handlerMem ?? throw new ArgumentNullException(nameof(handlerMem));

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			if (decoder.is64Mode)
				decoder.VEX3(ref instruction);
			else if (decoder.state.mod == 3)
				decoder.VEX3(ref instruction);
			else
				handlerMem.Decode(decoder, ref instruction);
		}
	}

	sealed class OpCodeHandler_XOP : OpCodeHandlerModRM {
		readonly OpCodeHandler handler_reg0;

		public OpCodeHandler_XOP(OpCodeHandler handler_reg0) => this.handler_reg0 = handler_reg0 ?? throw new ArgumentNullException(nameof(handler_reg0));

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			if ((decoder.state.modrm & 0x1F) < 8)
				handler_reg0.Decode(decoder, ref instruction);
			else
				decoder.XOP(ref instruction);
		}
	}

	sealed class OpCodeHandler_EVEX : OpCodeHandlerModRM {
		readonly OpCodeHandler handlerMem;

		public OpCodeHandler_EVEX(OpCodeHandler handlerMem) => this.handlerMem = handlerMem ?? throw new ArgumentNullException(nameof(handlerMem));

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			if (decoder.is64Mode)
				decoder.EVEX_MVEX(ref instruction);
			else if (decoder.state.mod == 3)
				decoder.EVEX_MVEX(ref instruction);
			else
				handlerMem.Decode(decoder, ref instruction);
		}
	}

	sealed class OpCodeHandler_Reg : OpCodeHandler {
		readonly Code code;
		readonly Register reg;

		public OpCodeHandler_Reg(Code code, Register reg) {
			this.code = code;
			this.reg = reg;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code;
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = reg;
		}
	}

	sealed class OpCodeHandler_RegIb : OpCodeHandler {
		readonly Code code;
		readonly Register reg;

		public OpCodeHandler_RegIb(Code code, Register reg) {
			this.code = code;
			this.reg = reg;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code;
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = reg;
			instruction.InternalOp1Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadByte();
		}
	}

	sealed class OpCodeHandler_IbReg : OpCodeHandler {
		readonly Code code;
		readonly Register reg;

		public OpCodeHandler_IbReg(Code code, Register reg) {
			this.code = code;
			this.reg = reg;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code;
			instruction.InternalOp0Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadByte();
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.InternalOp1Register = reg;
		}
	}

	sealed class OpCodeHandler_AL_DX : OpCodeHandler {
		readonly Code code;

		public OpCodeHandler_AL_DX(Code code) => this.code = code;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code;
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = Register.AL;
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.InternalOp1Register = Register.DX;
		}
	}

	sealed class OpCodeHandler_DX_AL : OpCodeHandler {
		readonly Code code;

		public OpCodeHandler_DX_AL(Code code) => this.code = code;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code;
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = Register.DX;
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.InternalOp1Register = Register.AL;
		}
	}

	sealed class OpCodeHandler_Ib : OpCodeHandler {
		readonly Code code;

		public OpCodeHandler_Ib(Code code) => this.code = code;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code;
			instruction.InternalOp0Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadByte();
		}
	}

	sealed class OpCodeHandler_Ib3 : OpCodeHandlerModRM {
		readonly Code code;

		public OpCodeHandler_Ib3(Code code) => this.code = code;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code;
			instruction.InternalOp0Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadByte();
		}
	}

	sealed class OpCodeHandler_MandatoryPrefix : OpCodeHandlerModRM {
		readonly OpCodeHandler[] handlers;

		public OpCodeHandler_MandatoryPrefix(OpCodeHandler handler, OpCodeHandler handler66, OpCodeHandler handlerF3, OpCodeHandler handlerF2) {
			Static.Assert((int)MandatoryPrefixByte.None == 0 ? 0 : -1);
			Static.Assert((int)MandatoryPrefixByte.P66 == 1 ? 0 : -1);
			Static.Assert((int)MandatoryPrefixByte.PF3 == 2 ? 0 : -1);
			Static.Assert((int)MandatoryPrefixByte.PF2 == 3 ? 0 : -1);
			handlers = new OpCodeHandler[4] {
				handler ?? throw new ArgumentNullException(nameof(handler)),
				handler66 ?? throw new ArgumentNullException(nameof(handler66)),
				handlerF3 ?? throw new ArgumentNullException(nameof(handlerF3)),
				handlerF2 ?? throw new ArgumentNullException(nameof(handlerF2)),
			};
			Debug.Assert(handler.HasModRM == HasModRM);
			Debug.Assert(handler66.HasModRM == HasModRM);
			Debug.Assert(handlerF3.HasModRM == HasModRM);
			Debug.Assert(handlerF2.HasModRM == HasModRM);
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			decoder.ClearMandatoryPrefix(ref instruction);
			handlers[(int)decoder.state.mandatoryPrefix].Decode(decoder, ref instruction);
		}
	}

	sealed class OpCodeHandler_MandatoryPrefix3 : OpCodeHandlerModRM {
		readonly Info[] handlers_reg;
		readonly Info[] handlers_mem;

		readonly struct Info {
			public readonly OpCodeHandler handler;
			public readonly bool mandatoryPrefix;
			public Info(OpCodeHandler handler, bool mandatoryPrefix) {
				this.handler = handler;
				this.mandatoryPrefix = mandatoryPrefix;
			}
		}

		public OpCodeHandler_MandatoryPrefix3(OpCodeHandler handler_reg, OpCodeHandler handler_mem, OpCodeHandler handler66_reg, OpCodeHandler handler66_mem, OpCodeHandler handlerF3_reg, OpCodeHandler handlerF3_mem, OpCodeHandler handlerF2_reg, OpCodeHandler handlerF2_mem, LegacyHandlerFlags flags) {
			Static.Assert((int)MandatoryPrefixByte.None == 0 ? 0 : -1);
			Static.Assert((int)MandatoryPrefixByte.P66 == 1 ? 0 : -1);
			Static.Assert((int)MandatoryPrefixByte.PF3 == 2 ? 0 : -1);
			Static.Assert((int)MandatoryPrefixByte.PF2 == 3 ? 0 : -1);
			handlers_reg = new Info[4] {
				new Info(handler_reg ?? throw new ArgumentNullException(nameof(handler_reg)), (flags & LegacyHandlerFlags.HandlerReg) == 0),
				new Info(handler66_reg ?? throw new ArgumentNullException(nameof(handler66_reg)), (flags & LegacyHandlerFlags.Handler66Reg) == 0),
				new Info(handlerF3_reg ?? throw new ArgumentNullException(nameof(handlerF3_reg)), (flags & LegacyHandlerFlags.HandlerF3Reg) == 0),
				new Info(handlerF2_reg ?? throw new ArgumentNullException(nameof(handlerF2_reg)), (flags & LegacyHandlerFlags.HandlerF2Reg) == 0),
			};
			handlers_mem = new Info[4] {
				new Info(handler_mem ?? throw new ArgumentNullException(nameof(handler_mem)), (flags & LegacyHandlerFlags.HandlerMem) == 0),
				new Info(handler66_mem ?? throw new ArgumentNullException(nameof(handler66_mem)), (flags & LegacyHandlerFlags.Handler66Mem) == 0),
				new Info(handlerF3_mem ?? throw new ArgumentNullException(nameof(handlerF3_mem)), (flags & LegacyHandlerFlags.HandlerF3Mem) == 0),
				new Info(handlerF2_mem ?? throw new ArgumentNullException(nameof(handlerF2_mem)), (flags & LegacyHandlerFlags.HandlerF2Mem) == 0),
			};
			Debug.Assert(handler_reg.HasModRM == HasModRM);
			Debug.Assert(handler_mem.HasModRM == HasModRM);
			Debug.Assert(handler66_reg.HasModRM == HasModRM);
			Debug.Assert(handler66_mem.HasModRM == HasModRM);
			Debug.Assert(handlerF3_reg.HasModRM == HasModRM);
			Debug.Assert(handlerF3_mem.HasModRM == HasModRM);
			Debug.Assert(handlerF2_reg.HasModRM == HasModRM);
			Debug.Assert(handlerF2_mem.HasModRM == HasModRM);
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			var handlers = decoder.state.mod == 3 ? handlers_reg : handlers_mem;
			var info = handlers[(int)decoder.state.mandatoryPrefix];
			if (info.mandatoryPrefix)
				decoder.ClearMandatoryPrefix(ref instruction);
			info.handler.Decode(decoder, ref instruction);
		}
	}

	sealed class OpCodeHandler_MandatoryPrefix_F3_F2 : OpCodeHandler {
		readonly OpCodeHandler handlerNormal;
		readonly OpCodeHandler handlerF3;
		readonly OpCodeHandler handlerF2;
		readonly bool clearF3;
		readonly bool clearF2;

		public OpCodeHandler_MandatoryPrefix_F3_F2(OpCodeHandler handlerNormal, OpCodeHandler handlerF3, bool clearF3, OpCodeHandler handlerF2, bool clearF2) {
			this.handlerNormal = handlerNormal ?? throw new ArgumentNullException(nameof(handlerNormal));
			this.handlerF3 = handlerF3 ?? throw new ArgumentNullException(nameof(handlerF3));
			this.clearF3 = clearF3;
			this.handlerF2 = handlerF2 ?? throw new ArgumentNullException(nameof(handlerF2));
			this.clearF2 = clearF2;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			OpCodeHandler handler;
			var prefix = decoder.state.mandatoryPrefix;
			if (prefix == MandatoryPrefixByte.PF3) {
				if (clearF3)
					decoder.ClearMandatoryPrefixF3(ref instruction);
				handler = handlerF3;
			}
			else if (prefix == MandatoryPrefixByte.PF2) {
				if (clearF2)
					decoder.ClearMandatoryPrefixF2(ref instruction);
				handler = handlerF2;
			}
			else {
				Debug.Assert(prefix == MandatoryPrefixByte.None || prefix == MandatoryPrefixByte.P66);
				handler = handlerNormal;
			}
			if (handler.HasModRM)
				decoder.ReadModRM();
			handler.Decode(decoder, ref instruction);
		}
	}

	sealed class OpCodeHandler_MandatoryPrefix_NoModRM : OpCodeHandler {
		readonly OpCodeHandler[] handlers;

		public OpCodeHandler_MandatoryPrefix_NoModRM(OpCodeHandler handler, OpCodeHandler handler66, OpCodeHandler handlerF3, OpCodeHandler handlerF2) {
			Static.Assert((int)MandatoryPrefixByte.None == 0 ? 0 : -1);
			Static.Assert((int)MandatoryPrefixByte.P66 == 1 ? 0 : -1);
			Static.Assert((int)MandatoryPrefixByte.PF3 == 2 ? 0 : -1);
			Static.Assert((int)MandatoryPrefixByte.PF2 == 3 ? 0 : -1);
			handlers = new OpCodeHandler[4] {
				handler ?? throw new ArgumentNullException(nameof(handler)),
				handler66 ?? throw new ArgumentNullException(nameof(handler66)),
				handlerF3 ?? throw new ArgumentNullException(nameof(handlerF3)),
				handlerF2 ?? throw new ArgumentNullException(nameof(handlerF2)),
			};
			Debug.Assert(handler.HasModRM == HasModRM);
			Debug.Assert(handler66.HasModRM == HasModRM);
			Debug.Assert(handlerF3.HasModRM == HasModRM);
			Debug.Assert(handlerF2.HasModRM == HasModRM);
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			decoder.ClearMandatoryPrefix(ref instruction);
			handlers[(int)decoder.state.mandatoryPrefix].Decode(decoder, ref instruction);
		}
	}

	sealed class OpCodeHandler_NIb : OpCodeHandlerModRM {
		readonly Code code;

		public OpCodeHandler_NIb(Code code) => this.code = code;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code;
			if (state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)state.rm + Register.MM0;
			}
			else
				decoder.SetInvalidInstruction();
			instruction.InternalOp1Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadByte();
		}
	}

	sealed class OpCodeHandler_ReservedNop : OpCodeHandlerModRM {
		readonly OpCodeHandler reservedNopHandler;
		readonly OpCodeHandler otherHandler;

		public OpCodeHandler_ReservedNop(OpCodeHandler reservedNopHandler, OpCodeHandler otherHandler) {
			this.reservedNopHandler = reservedNopHandler ?? throw new ArgumentNullException(nameof(reservedNopHandler));
			this.otherHandler = otherHandler ?? throw new ArgumentNullException(nameof(otherHandler));
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			((decoder.options & DecoderOptions.ForceReservedNop) != 0 ? reservedNopHandler : otherHandler).Decode(decoder, ref instruction);
		}
	}

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
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
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
				decoder.ReadOpMem(ref instruction);
				if ((flags & (HandlerFlags.Xacquire | HandlerFlags.Xrelease)) != 0)
					decoder.SetXacquireXrelease(ref instruction, flags);
				Static.Assert((int)HandlerFlags.Lock == 8 ? 0 : -1);
				Static.Assert((int)StateFlags.AllowLock == 0x00002000 ? 0 : -1);
				state.flags |= (StateFlags)((uint)(flags & HandlerFlags.Lock) << (13 - 3));
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
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
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
				decoder.ReadOpMem(ref instruction);
				if ((flags & (HandlerFlags.Xacquire | HandlerFlags.Xrelease)) != 0)
					decoder.SetXacquireXrelease(ref instruction, flags);
				Static.Assert((int)HandlerFlags.Lock == 8 ? 0 : -1);
				Static.Assert((int)StateFlags.AllowLock == 0x00002000 ? 0 : -1);
				state.flags |= (StateFlags)((uint)(flags & HandlerFlags.Lock) << (13 - 3));
			}
			if (state.operandSize == OpSize.Size32) {
				instruction.InternalCode = code32;
				instruction.InternalOp1Kind = OpKind.Immediate8to32;
				instruction.InternalImmediate8 = decoder.ReadByte();
			}
			else if (state.operandSize == OpSize.Size64) {
				instruction.InternalCode = code64;
				instruction.InternalOp1Kind = OpKind.Immediate8to64;
				instruction.InternalImmediate8 = decoder.ReadByte();
			}
			else {
				instruction.InternalCode = code16;
				instruction.InternalOp1Kind = OpKind.Immediate8to16;
				instruction.InternalImmediate8 = decoder.ReadByte();
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
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
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
				decoder.ReadOpMem(ref instruction);
				if ((flags & (HandlerFlags.Xacquire | HandlerFlags.Xrelease)) != 0)
					decoder.SetXacquireXrelease(ref instruction, flags);
				Static.Assert((int)HandlerFlags.Lock == 8 ? 0 : -1);
				Static.Assert((int)StateFlags.AllowLock == 0x00002000 ? 0 : -1);
				state.flags |= (StateFlags)((uint)(flags & HandlerFlags.Lock) << (13 - 3));
			}
			instruction.InternalOp1Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadByte();
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
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
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
				decoder.ReadOpMem(ref instruction);
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
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
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
				decoder.ReadOpMem(ref instruction);
			}
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
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
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
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
				decoder.ReadOpMem(ref instruction);
				if ((flags & (HandlerFlags.Xacquire | HandlerFlags.Xrelease)) != 0)
					decoder.SetXacquireXrelease(ref instruction, flags);
				Static.Assert((int)HandlerFlags.Lock == 8 ? 0 : -1);
				Static.Assert((int)StateFlags.AllowLock == 0x00002000 ? 0 : -1);
				state.flags |= (StateFlags)((uint)(flags & HandlerFlags.Lock) << (13 - 3));
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
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.rm + state.extraBaseRegisterBase) + Register.EAX;
			}
			else if (state.operandSize == OpSize.Size64) {
				instruction.InternalCode = code64;
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.rm + state.extraBaseRegisterBase) + Register.RAX;
			}
			else {
				instruction.InternalCode = code16;
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.rm + state.extraBaseRegisterBase) + Register.AX;
			}
			Debug.Assert(state.mod == 3);
		}
	}

	sealed class OpCodeHandler_Rv_32_64 : OpCodeHandlerModRM {
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_Rv_32_64(Code code32, Code code64) {
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			Register baseReg;
			if (decoder.is64Mode) {
				instruction.InternalCode = code64;
				baseReg = Register.RAX;
			}
			else {
				instruction.InternalCode = code32;
				baseReg = Register.EAX;
			}
			Debug.Assert(state.mod == 3);
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = (int)(state.rm + state.extraBaseRegisterBase) + baseReg;
		}
	}

	sealed class OpCodeHandler_Ev_REXW : OpCodeHandlerModRM {
		readonly Code code32;
		readonly Code code64;
		readonly uint disallowReg;
		readonly uint disallowMem;

		public OpCodeHandler_Ev_REXW(Code code32, Code code64, bool allowReg, bool allowMem) {
			this.code32 = code32;
			this.code64 = code64;
			disallowReg = allowReg ? 0 : uint.MaxValue;
			disallowMem = allowMem ? 0 : uint.MaxValue;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if ((state.flags & StateFlags.W) != 0)
				instruction.InternalCode = code64;
			else
				instruction.InternalCode = code32;
			if (state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp0Kind = OpKind.Register;
				if ((state.flags & StateFlags.W) != 0)
					instruction.InternalOp0Register = (int)(state.rm + state.extraBaseRegisterBase) + Register.RAX;
				else
					instruction.InternalOp0Register = (int)(state.rm + state.extraBaseRegisterBase) + Register.EAX;
				if ((disallowReg & decoder.invalidCheckMask) != 0)
					decoder.SetInvalidInstruction();
			}
			else {
				instruction.InternalOp0Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
				if ((disallowMem & decoder.invalidCheckMask) != 0)
					decoder.SetInvalidInstruction();
			}
		}
	}

	sealed class OpCodeHandler_Evj : OpCodeHandlerModRM {
		readonly Code code16;
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_Evj(Code code16, Code code32, Code code64) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if (decoder.is64Mode) {
				if ((decoder.options & DecoderOptions.AMD) == 0 || state.operandSize == OpSize.Size32)
					instruction.InternalCode = code64;
				else
					instruction.InternalCode = code16;
				if (state.mod == 3) {
					Static.Assert(OpKind.Register == 0 ? 0 : -1);
					//instruction.InternalOp0Kind = OpKind.Register;
					if ((decoder.options & DecoderOptions.AMD) == 0 || state.operandSize == OpSize.Size32)
						instruction.InternalOp0Register = (int)(state.rm + state.extraBaseRegisterBase) + Register.RAX;
					else
						instruction.InternalOp0Register = (int)(state.rm + state.extraBaseRegisterBase) + Register.AX;
				}
				else {
					instruction.InternalOp0Kind = OpKind.Memory;
					decoder.ReadOpMem(ref instruction);
				}
			}
			else {
				if (state.operandSize == OpSize.Size32)
					instruction.InternalCode = code32;
				else
					instruction.InternalCode = code16;
				if (state.mod == 3) {
					Static.Assert(OpKind.Register == 0 ? 0 : -1);
					//instruction.InternalOp0Kind = OpKind.Register;
					if (state.operandSize == OpSize.Size32)
						instruction.InternalOp0Register = (int)state.rm + Register.EAX;
					else
						instruction.InternalOp0Register = (int)state.rm + Register.AX;
				}
				else {
					instruction.InternalOp0Kind = OpKind.Memory;
					decoder.ReadOpMem(ref instruction);
				}
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
			if (state.operandSize == OpSize.Size64 && (decoder.options & DecoderOptions.AMD) == 0)
				instruction.InternalCode = code64;
			else if (state.operandSize == OpSize.Size16)
				instruction.InternalCode = code16;
			else
				instruction.InternalCode = code32;
			if (state.mod == 3)
				decoder.SetInvalidInstruction();
			else {
				instruction.InternalOp0Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
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
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
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
				decoder.ReadOpMem(ref instruction);
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
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
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
				decoder.ReadOpMem(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_Ms : OpCodeHandlerModRM {
		readonly Code code16;
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_Ms(Code code16, Code code32, Code code64) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if (decoder.is64Mode)
				instruction.InternalCode = code64;
			else if (state.operandSize == OpSize.Size32)
				instruction.InternalCode = code32;
			else
				instruction.InternalCode = code16;
			Debug.Assert(state.mod != 3);
			instruction.InternalOp0Kind = OpKind.Memory;
			decoder.ReadOpMem(ref instruction);
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
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + Register.EAX;
			}
			else if (state.operandSize == OpSize.Size64) {
				instruction.InternalCode = code64;
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + Register.RAX;
			}
			else {
				instruction.InternalCode = code16;
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + Register.AX;
			}
			if (state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
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
				decoder.ReadOpMem(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_Gv_M_as : OpCodeHandlerModRM {
		readonly Code code16;
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_Gv_M_as(Code code16, Code code32, Code code64) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if (state.addressSize == OpSize.Size64) {
				instruction.InternalCode = code64;
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + Register.RAX;
			}
			else if (state.addressSize == OpSize.Size32) {
				instruction.InternalCode = code32;
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + Register.EAX;
			}
			else {
				instruction.InternalCode = code16;
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + Register.AX;
			}
			if (state.mod == 3)
				decoder.SetInvalidInstruction();
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
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
			if (state.operandSize == OpSize.Size32) {
				instruction.InternalCode = code32;
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + Register.EAX;
			}
			else if (state.operandSize == OpSize.Size64) {
				instruction.InternalCode = code64;
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + Register.RAX;
			}
			else {
				instruction.InternalCode = code16;
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + Register.EAX;
			}
			if (state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
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
				decoder.ReadOpMem(ref instruction);
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
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + Register.EAX;
			}
			else if (state.operandSize == OpSize.Size64) {
				instruction.InternalCode = code64;
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + Register.RAX;
			}
			else {
				instruction.InternalCode = code16;
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + Register.AX;
			}
			if (state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
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
				decoder.ReadOpMem(ref instruction);
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
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + Register.EAX;
			}
			else if (state.operandSize == OpSize.Size64) {
				instruction.InternalCode = code64;
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + Register.RAX;
			}
			else {
				instruction.InternalCode = code16;
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + Register.AX;
			}
			if (state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp1Kind = OpKind.Register;
				uint index = state.rm + state.extraBaseRegisterBase;
				if (state.operandSize != OpSize.Size16)
					instruction.InternalOp1Register = (int)index + Register.EAX;
				else
					instruction.InternalOp1Register = (int)index + Register.AX;
			}
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_R_C : OpCodeHandlerModRM {
		readonly Code code32;
		readonly Code code64;
		readonly Register baseReg;

		public OpCodeHandler_R_C(Code code32, Code code64, Register baseReg) {
			this.code32 = code32;
			this.code64 = code64;
			this.baseReg = baseReg;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if (decoder.is64Mode) {
				instruction.InternalCode = code64;
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.rm + state.extraBaseRegisterBase) + Register.RAX;
			}
			else {
				instruction.InternalCode = code32;
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.rm + state.extraBaseRegisterBase) + Register.EAX;
			}
			var extraRegisterBase = state.extraRegisterBase;
			// LOCK MOV CR0 is supported by some AMD CPUs
			if (baseReg == Register.CR0 && extraRegisterBase == 0 && instruction.HasLockPrefix && (decoder.options & DecoderOptions.NoLockMovCR0) == 0) {
				extraRegisterBase = 8;
				instruction.InternalClearHasLockPrefix();
				state.flags &= ~StateFlags.Lock;
			}
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.InternalOp1Kind = OpKind.Register;
			var reg = (int)(state.reg + extraRegisterBase);
			if (decoder.invalidCheckMask != 0) {
				if (baseReg == Register.CR0) {
					if (reg == 1 || (reg != 8 && reg >= 5))
						decoder.SetInvalidInstruction();
				}
				else if (baseReg == Register.DR0) {
					if (reg > 7)
						decoder.SetInvalidInstruction();
				}
				else
					Debug.Assert(baseReg == Register.TR0);
			}
			instruction.InternalOp1Register = reg + baseReg;
		}
	}

	sealed class OpCodeHandler_C_R : OpCodeHandlerModRM {
		readonly Code code32;
		readonly Code code64;
		readonly Register baseReg;

		public OpCodeHandler_C_R(Code code32, Code code64, Register baseReg) {
			this.code32 = code32;
			this.code64 = code64;
			this.baseReg = baseReg;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if (decoder.is64Mode) {
				instruction.InternalCode = code64;
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.InternalOp1Register = (int)(state.rm + state.extraBaseRegisterBase) + Register.RAX;
			}
			else {
				instruction.InternalCode = code32;
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.InternalOp1Register = (int)(state.rm + state.extraBaseRegisterBase) + Register.EAX;
			}
			var extraRegisterBase = state.extraRegisterBase;
			// LOCK MOV CR0 is supported by some AMD CPUs
			if (baseReg == Register.CR0 && extraRegisterBase == 0 && instruction.HasLockPrefix && (decoder.options & DecoderOptions.NoLockMovCR0) == 0) {
				extraRegisterBase = 8;
				instruction.InternalClearHasLockPrefix();
				state.flags &= ~StateFlags.Lock;
			}
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.InternalOp0Kind = OpKind.Register;
			var reg = (int)(state.reg + extraRegisterBase);
			if (decoder.invalidCheckMask != 0) {
				if (baseReg == Register.CR0) {
					if (reg == 1 || (reg != 8 && reg >= 5))
						decoder.SetInvalidInstruction();
				}
				else if (baseReg == Register.DR0) {
					if (reg > 7)
						decoder.SetInvalidInstruction();
				}
				else
					Debug.Assert(baseReg == Register.TR0);
			}
			instruction.InternalOp0Register = reg + baseReg;
		}
	}

	sealed class OpCodeHandler_Jb : OpCodeHandler {
		readonly Code code16;
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_Jb(Code code16, Code code32, Code code64) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			state.flags |= StateFlags.BranchImm8;
			if (decoder.is64Mode) {
				if ((decoder.options & DecoderOptions.AMD) == 0 || state.operandSize == OpSize.Size32) {
					instruction.InternalCode = code64;
					instruction.InternalOp0Kind = OpKind.NearBranch64;
					instruction.NearBranch64 = (ulong)(sbyte)decoder.ReadByte() + decoder.GetCurrentInstructionPointer64();
				}
				else {
					instruction.InternalCode = code16;
					instruction.InternalOp0Kind = OpKind.NearBranch16;
					instruction.InternalNearBranch16 = (ushort)((uint)(sbyte)decoder.ReadByte() + decoder.GetCurrentInstructionPointer32());
				}
			}
			else {
				if (state.operandSize != OpSize.Size16) {
					instruction.InternalCode = code32;
					instruction.InternalOp0Kind = OpKind.NearBranch32;
					instruction.NearBranch32 = (uint)(sbyte)decoder.ReadByte() + decoder.GetCurrentInstructionPointer32();
				}
				else {
					instruction.InternalCode = code16;
					instruction.InternalOp0Kind = OpKind.NearBranch16;
					instruction.InternalNearBranch16 = (ushort)((uint)(sbyte)decoder.ReadByte() + decoder.GetCurrentInstructionPointer32());
				}
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
			state.flags |= StateFlags.Xbegin;
			if (decoder.is64Mode) {
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
					instruction.InternalCode = code16;
					instruction.InternalOp0Kind = OpKind.NearBranch64;
					instruction.NearBranch64 = (ulong)(short)decoder.ReadUInt16() + decoder.GetCurrentInstructionPointer64();
				}
			}
			else {
				Debug.Assert(decoder.defaultCodeSize == CodeSize.Code16 || decoder.defaultCodeSize == CodeSize.Code32);
				if (state.operandSize == OpSize.Size32) {
					instruction.InternalCode = code32;
					instruction.InternalOp0Kind = OpKind.NearBranch32;
					instruction.NearBranch32 = decoder.ReadUInt32() + decoder.GetCurrentInstructionPointer32();
				}
				else {
					Debug.Assert(state.operandSize == OpSize.Size16);
					instruction.InternalCode = code16;
					instruction.InternalOp0Kind = OpKind.NearBranch32;
					instruction.NearBranch32 = (uint)(short)decoder.ReadUInt16() + decoder.GetCurrentInstructionPointer32();
				}
			}
		}
	}

	sealed class OpCodeHandler_Jz : OpCodeHandler {
		readonly Code code16;
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_Jz(Code code16, Code code32, Code code64) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if (decoder.is64Mode) {
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
			else {
				if (state.operandSize != OpSize.Size16) {
					instruction.InternalCode = code32;
					instruction.InternalOp0Kind = OpKind.NearBranch32;
					instruction.NearBranch32 = decoder.ReadUInt32() + decoder.GetCurrentInstructionPointer32();
				}
				else {
					instruction.InternalCode = code16;
					instruction.InternalOp0Kind = OpKind.NearBranch16;
					instruction.InternalNearBranch16 = (ushort)(decoder.ReadUInt16() + decoder.GetCurrentInstructionPointer32());
				}
			}
		}
	}

	sealed class OpCodeHandler_Jb2 : OpCodeHandler {
		readonly Code code16_16;
		readonly Code code16_32;
		readonly Code code16_64;
		readonly Code code32_16;
		readonly Code code32_32;
		readonly Code code64_32;
		readonly Code code64_64;

		public OpCodeHandler_Jb2(Code code16_16, Code code16_32, Code code16_64, Code code32_16, Code code32_32, Code code64_32, Code code64_64) {
			this.code16_16 = code16_16;
			this.code16_32 = code16_32;
			this.code16_64 = code16_64;
			this.code32_16 = code32_16;
			this.code32_32 = code32_32;
			this.code64_32 = code64_32;
			this.code64_64 = code64_64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			state.flags |= StateFlags.BranchImm8;
			if (decoder.is64Mode) {
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
					instruction.InternalNearBranch16 = (ushort)((uint)(sbyte)decoder.ReadByte() + decoder.GetCurrentInstructionPointer32());
				}
			}
			else {
				if (state.operandSize == OpSize.Size32) {
					if (state.addressSize == OpSize.Size32)
						instruction.InternalCode = code32_32;
					else
						instruction.InternalCode = code32_16;
					instruction.InternalOp0Kind = OpKind.NearBranch32;
					instruction.NearBranch32 = (uint)(sbyte)decoder.ReadByte() + decoder.GetCurrentInstructionPointer32();
				}
				else {
					if (state.addressSize == OpSize.Size32)
						instruction.InternalCode = code16_32;
					else
						instruction.InternalCode = code16_16;
					instruction.InternalOp0Kind = OpKind.NearBranch16;
					instruction.InternalNearBranch16 = (ushort)((uint)(sbyte)decoder.ReadByte() + decoder.GetCurrentInstructionPointer32());
				}
			}
		}
	}

	sealed class OpCodeHandler_Jdisp : OpCodeHandler {
		readonly Code code16;
		readonly Code code32;

		public OpCodeHandler_Jdisp(Code code16, Code code32) {
			this.code16 = code16;
			this.code32 = code32;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			Debug.Assert(!decoder.is64Mode);
			if (state.operandSize != OpSize.Size16) {
				instruction.InternalCode = code32;
				instruction.InternalOp0Kind = OpKind.NearBranch32;
				instruction.NearBranch32 = decoder.ReadUInt32();
			}
			else {
				instruction.InternalCode = code16;
				instruction.InternalOp0Kind = OpKind.NearBranch16;
				instruction.InternalNearBranch16 = decoder.ReadUInt16();
			}
		}
	}

	sealed class OpCodeHandler_PushOpSizeReg : OpCodeHandler {
		readonly Code code16;
		readonly Code code32;
		readonly Code code64;
		readonly Register reg;

		public OpCodeHandler_PushOpSizeReg(Code code16, Code code32, Code code64, Register reg) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
			this.reg = reg;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if (decoder.is64Mode) {
				if (state.operandSize != OpSize.Size16)
					instruction.InternalCode = code64;
				else
					instruction.InternalCode = code16;
			}
			else {
				if (state.operandSize == OpSize.Size32)
					instruction.InternalCode = code32;
				else
					instruction.InternalCode = code16;
			}
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = reg;
		}
	}

	sealed class OpCodeHandler_PushEv : OpCodeHandlerModRM {
		readonly Code code16;
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_PushEv(Code code16, Code code32, Code code64) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if (decoder.is64Mode) {
				if (state.operandSize != OpSize.Size16)
					instruction.InternalCode = code64;
				else
					instruction.InternalCode = code16;
			}
			else {
				if (state.operandSize == OpSize.Size32)
					instruction.InternalCode = code32;
				else
					instruction.InternalCode = code16;
			}
			if (state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp0Kind = OpKind.Register;
				uint index = state.rm + state.extraBaseRegisterBase;
				if (decoder.is64Mode) {
					if (state.operandSize != OpSize.Size16)
						instruction.InternalOp0Register = (int)index + Register.RAX;
					else
						instruction.InternalOp0Register = (int)index + Register.AX;
				}
				else {
					if (state.operandSize == OpSize.Size32)
						instruction.InternalOp0Register = (int)index + Register.EAX;
					else
						instruction.InternalOp0Register = (int)index + Register.AX;
				}
			}
			else {
				instruction.InternalOp0Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
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
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
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
				decoder.ReadOpMem(ref instruction);
				if ((flags & (HandlerFlags.Xacquire | HandlerFlags.Xrelease)) != 0)
					decoder.SetXacquireXrelease(ref instruction, flags);
				Static.Assert((int)HandlerFlags.Lock == 8 ? 0 : -1);
				Static.Assert((int)StateFlags.AllowLock == 0x00002000 ? 0 : -1);
				state.flags |= (StateFlags)((uint)(flags & HandlerFlags.Lock) << (13 - 3));
			}
			if (state.operandSize == OpSize.Size32) {
				instruction.InternalCode = code32;
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.InternalOp1Register = (int)(state.reg + state.extraRegisterBase) + Register.EAX;
			}
			else if (state.operandSize == OpSize.Size64) {
				instruction.InternalCode = code64;
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.InternalOp1Register = (int)(state.reg + state.extraRegisterBase) + Register.RAX;
			}
			else {
				instruction.InternalCode = code16;
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.InternalOp1Register = (int)(state.reg + state.extraRegisterBase) + Register.AX;
			}
		}
	}

	sealed class OpCodeHandler_Ev_Gv_32_64 : OpCodeHandlerModRM {
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_Ev_Gv_32_64(Code code32, Code code64) {
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			Register baseReg;
			if (decoder.is64Mode) {
				instruction.InternalCode = code64;
				baseReg = Register.RAX;
			}
			else {
				instruction.InternalCode = code32;
				baseReg = Register.EAX;
			}
			if (state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.rm + state.extraBaseRegisterBase) + baseReg;
			}
			else {
				instruction.InternalOp0Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
			}
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.InternalOp1Register = (int)(state.reg + state.extraRegisterBase) + baseReg;
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
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
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
				decoder.ReadOpMem(ref instruction);
			}
			if (state.operandSize == OpSize.Size32) {
				instruction.InternalCode = code32;
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.InternalOp1Register = (int)(state.reg + state.extraRegisterBase) + Register.EAX;
			}
			else if (state.operandSize == OpSize.Size64) {
				instruction.InternalCode = code64;
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.InternalOp1Register = (int)(state.reg + state.extraRegisterBase) + Register.RAX;
			}
			else {
				instruction.InternalCode = code16;
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.InternalOp1Register = (int)(state.reg + state.extraRegisterBase) + Register.AX;
			}
			instruction.InternalOp2Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadByte();
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
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
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
				decoder.ReadOpMem(ref instruction);
			}
			if (state.operandSize == OpSize.Size32) {
				instruction.InternalCode = code32;
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.InternalOp1Register = (int)(state.reg + state.extraRegisterBase) + Register.EAX;
			}
			else if (state.operandSize == OpSize.Size64) {
				instruction.InternalCode = code64;
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.InternalOp1Register = (int)(state.reg + state.extraRegisterBase) + Register.RAX;
			}
			else {
				instruction.InternalCode = code16;
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.InternalOp1Register = (int)(state.reg + state.extraRegisterBase) + Register.AX;
			}
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
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
			if (state.operandSize == OpSize.Size64 && (decoder.options & DecoderOptions.AMD) == 0) {
				instruction.InternalCode = code64;
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + Register.RAX;
			}
			else if (state.operandSize == OpSize.Size16) {
				instruction.InternalCode = code16;
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + Register.AX;
			}
			else {
				instruction.InternalCode = code32;
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + Register.EAX;
			}
			if (state.mod == 3)
				decoder.SetInvalidInstruction();
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
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
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + Register.EAX;
			}
			else if (state.operandSize == OpSize.Size64) {
				instruction.InternalCode = code64;
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + Register.RAX;
			}
			else {
				instruction.InternalCode = code16;
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + Register.AX;
			}
			if (state.mod == 3) {
				uint index = state.rm + state.extraBaseRegisterBase;
				if ((state.flags & StateFlags.HasRex) != 0 && index >= 4)
					index += 4;
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.InternalOp1Register = (int)index + Register.AL;
			}
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
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
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + Register.EAX;
			}
			else if (state.operandSize == OpSize.Size64) {
				instruction.InternalCode = code64;
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + Register.RAX;
			}
			else {
				instruction.InternalCode = code16;
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + Register.AX;
			}
			if (state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.InternalOp1Register = (int)(state.rm + state.extraBaseRegisterBase) + Register.AX;
			}
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_PushSimple2 : OpCodeHandler {
		readonly Code code16;
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_PushSimple2(Code code16, Code code32, Code code64) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			if (decoder.is64Mode) {
				if (decoder.state.operandSize != OpSize.Size16)
					instruction.InternalCode = code64;
				else
					instruction.InternalCode = code16;
			}
			else {
				if (decoder.state.operandSize == OpSize.Size32)
					instruction.InternalCode = code32;
				else
					instruction.InternalCode = code16;
			}
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
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_Simple3(Code code16, Code code32, Code code64) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if (decoder.is64Mode) {
				if (state.operandSize != OpSize.Size16)
					instruction.InternalCode = code64;
				else
					instruction.InternalCode = code16;
			}
			else {
				if (state.operandSize == OpSize.Size32)
					instruction.InternalCode = code32;
				else
					instruction.InternalCode = code16;
			}
		}
	}

	sealed class OpCodeHandler_Simple5 : OpCodeHandler {
		readonly Code code16;
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_Simple5(Code code16, Code code32, Code code64) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if (state.addressSize == OpSize.Size64)
				instruction.InternalCode = code64;
			else if (state.addressSize == OpSize.Size32)
				instruction.InternalCode = code32;
			else
				instruction.InternalCode = code16;
		}
	}

	sealed class OpCodeHandler_Simple5_ModRM_as : OpCodeHandlerModRM {
		readonly Code code16;
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_Simple5_ModRM_as(Code code16, Code code32, Code code64) {
			this.code16 = code16;
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
			else if (state.addressSize == OpSize.Size32) {
				instruction.InternalCode = code32;
				instruction.InternalOp0Register = (int)(state.rm + state.extraBaseRegisterBase) + Register.EAX;
			}
			else {
				instruction.InternalCode = code16;
				instruction.InternalOp0Register = (int)(state.rm + state.extraBaseRegisterBase) + Register.AX;
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
		readonly int index;
		readonly Code code16;
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_PushSimpleReg(int index, Code code16, Code code32, Code code64) {
			Debug.Assert(0 <= index && index <= 7);
			this.index = index;
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if (decoder.is64Mode) {
				if (state.operandSize != OpSize.Size16) {
					instruction.InternalCode = code64;
					Static.Assert(OpKind.Register == 0 ? 0 : -1);
					//instruction.InternalOp0Kind = OpKind.Register;
					instruction.InternalOp0Register = index + (int)state.extraBaseRegisterBase + Register.RAX;
				}
				else {
					instruction.InternalCode = code16;
					Static.Assert(OpKind.Register == 0 ? 0 : -1);
					//instruction.InternalOp0Kind = OpKind.Register;
					instruction.InternalOp0Register = index + (int)state.extraBaseRegisterBase + Register.AX;
				}
			}
			else {
				if (state.operandSize == OpSize.Size32) {
					instruction.InternalCode = code32;
					Static.Assert(OpKind.Register == 0 ? 0 : -1);
					//instruction.InternalOp0Kind = OpKind.Register;
					instruction.InternalOp0Register = index + (int)state.extraBaseRegisterBase + Register.EAX;
				}
				else {
					instruction.InternalCode = code16;
					Static.Assert(OpKind.Register == 0 ? 0 : -1);
					//instruction.InternalOp0Kind = OpKind.Register;
					instruction.InternalOp0Register = index + (int)state.extraBaseRegisterBase + Register.AX;
				}
			}
		}
	}

	sealed class OpCodeHandler_SimpleReg : OpCodeHandler {
		readonly Code code;
		readonly int index;

		public OpCodeHandler_SimpleReg(Code code, int index) {
			Debug.Assert(0 <= index && index <= 7);
			this.code = code;
			this.index = index;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			Static.Assert((int)OpSize.Size16 == 0 ? 0 : -1);
			Static.Assert((int)OpSize.Size32 == 1 ? 0 : -1);
			Static.Assert((int)OpSize.Size64 == 2 ? 0 : -1);
			int sizeIndex = (int)state.operandSize;

			instruction.InternalCode = sizeIndex + code;
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.InternalOp0Kind = OpKind.Register;
			Static.Assert(Register.AX + 16 == Register.EAX ? 0 : -1);
			Static.Assert(Register.AX + 32 == Register.RAX ? 0 : -1);
			instruction.InternalOp0Register = sizeIndex * 16 + index + (int)state.extraBaseRegisterBase + Register.AX;
		}
	}

	sealed class OpCodeHandler_Xchg_Reg_rAX : OpCodeHandler {
		readonly int index;
		readonly Code[] codes;

		public OpCodeHandler_Xchg_Reg_rAX(int index) {
			Debug.Assert(0 <= index && index <= 7);
			this.index = index;
			codes = s_codes;
		}

		static readonly Code[] s_codes = new Code[3 * 16] {
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
		};

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);

			if (index == 0 && state.mandatoryPrefix == MandatoryPrefixByte.PF3 && (decoder.options & DecoderOptions.NoPause) == 0) {
				decoder.ClearMandatoryPrefixF3(ref instruction);
				instruction.InternalCode = Code.Pause;
			}
			else {
				Static.Assert((int)OpSize.Size16 == 0 ? 0 : -1);
				Static.Assert((int)OpSize.Size32 == 1 ? 0 : -1);
				Static.Assert((int)OpSize.Size64 == 2 ? 0 : -1);
				int sizeIndex = (int)state.operandSize;
				int codeIndex = index + (int)state.extraBaseRegisterBase;

				instruction.InternalCode = codes[sizeIndex * 16 + codeIndex];
				if (codeIndex != 0) {
					Static.Assert(Register.AX + 16 == Register.EAX ? 0 : -1);
					Static.Assert(Register.AX + 32 == Register.RAX ? 0 : -1);
					var reg = sizeIndex * 16 + codeIndex + Register.AX;
					Static.Assert(OpKind.Register == 0 ? 0 : -1);
					//instruction.InternalOp0Kind = OpKind.Register;
					instruction.InternalOp0Register = reg;
					Static.Assert(OpKind.Register == 0 ? 0 : -1);
					//instruction.InternalOp1Kind = OpKind.Register;
					instruction.InternalOp1Register = sizeIndex * 16 + Register.AX;
				}
			}
		}
	}

	sealed class OpCodeHandler_Reg_Iz : OpCodeHandler {
		readonly Code code16;
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_Reg_Iz(Code code16, Code code32, Code code64) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if (state.operandSize == OpSize.Size32) {
				instruction.InternalCode = code32;
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = Register.EAX;
				instruction.InternalOp1Kind = OpKind.Immediate32;
				instruction.Immediate32 = decoder.ReadUInt32();
			}
			else if (state.operandSize == OpSize.Size64) {
				instruction.InternalCode = code64;
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = Register.RAX;
				instruction.InternalOp1Kind = OpKind.Immediate32to64;
				instruction.Immediate32 = decoder.ReadUInt32();
			}
			else {
				instruction.InternalCode = code16;
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = Register.AX;
				instruction.InternalOp1Kind = OpKind.Immediate16;
				instruction.InternalImmediate16 = decoder.ReadUInt16();
			}
		}
	}

	sealed class OpCodeHandler_RegIb3 : OpCodeHandler {
		readonly int index;
		readonly Register[] withRexPrefix;

		public OpCodeHandler_RegIb3(int index) {
			Debug.Assert(0 <= index && index <= 7);
			this.index = index;
			withRexPrefix = s_withRexPrefix;
		}

		static readonly Register[] s_withRexPrefix = new Register[16] {
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
				register = index + Register.AL;
			instruction.InternalCode = Code.Mov_r8_imm8;
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = register;
			instruction.InternalOp1Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadByte();
		}
	}

	sealed class OpCodeHandler_RegIz2 : OpCodeHandler {
		readonly int index;

		public OpCodeHandler_RegIz2(int index) {
			Debug.Assert(0 <= index && index <= 7);
			this.index = index;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if (state.operandSize == OpSize.Size32) {
				instruction.InternalCode = Code.Mov_r32_imm32;
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = index + (int)state.extraBaseRegisterBase + Register.EAX;
				instruction.InternalOp1Kind = OpKind.Immediate32;
				instruction.Immediate32 = decoder.ReadUInt32();
			}
			else if (state.operandSize == OpSize.Size64) {
				instruction.InternalCode = Code.Mov_r64_imm64;
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = index + (int)state.extraBaseRegisterBase + Register.RAX;
				instruction.InternalOp1Kind = OpKind.Immediate64;
				instruction.InternalImmediate64_lo = decoder.ReadUInt32();
				instruction.InternalImmediate64_hi = decoder.ReadUInt32();
			}
			else {
				instruction.InternalCode = Code.Mov_r16_imm16;
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = index + (int)state.extraBaseRegisterBase + Register.AX;
				instruction.InternalOp1Kind = OpKind.Immediate16;
				instruction.InternalImmediate16 = decoder.ReadUInt16();
			}
		}
	}

	sealed class OpCodeHandler_PushIb2 : OpCodeHandler {
		readonly Code code16;
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_PushIb2(Code code16, Code code32, Code code64) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if (decoder.is64Mode) {
				if (state.operandSize != OpSize.Size16) {
					instruction.InternalCode = code64;
					instruction.InternalOp0Kind = OpKind.Immediate8to64;
					instruction.InternalImmediate8 = decoder.ReadByte();
				}
				else {
					instruction.InternalCode = code16;
					instruction.InternalOp0Kind = OpKind.Immediate8to16;
					instruction.InternalImmediate8 = decoder.ReadByte();
				}
			}
			else {
				if (state.operandSize == OpSize.Size32) {
					instruction.InternalCode = code32;
					instruction.InternalOp0Kind = OpKind.Immediate8to32;
					instruction.InternalImmediate8 = decoder.ReadByte();
				}
				else {
					instruction.InternalCode = code16;
					instruction.InternalOp0Kind = OpKind.Immediate8to16;
					instruction.InternalImmediate8 = decoder.ReadByte();
				}
			}
		}
	}

	sealed class OpCodeHandler_PushIz : OpCodeHandler {
		readonly Code code16;
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_PushIz(Code code16, Code code32, Code code64) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if (decoder.is64Mode) {
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
			else {
				if (state.operandSize == OpSize.Size32) {
					instruction.InternalCode = code32;
					instruction.InternalOp0Kind = OpKind.Immediate32;
					instruction.Immediate32 = decoder.ReadUInt32();
				}
				else {
					instruction.InternalCode = code16;
					instruction.InternalOp0Kind = OpKind.Immediate16;
					instruction.InternalImmediate16 = decoder.ReadUInt16();
				}
			}
		}
	}

	sealed class OpCodeHandler_Gv_Ma : OpCodeHandlerModRM {
		readonly Code code16;
		readonly Code code32;

		public OpCodeHandler_Gv_Ma(Code code16, Code code32) {
			this.code16 = code16;
			this.code32 = code32;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if (state.operandSize != OpSize.Size16) {
				instruction.InternalCode = code32;
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + Register.EAX;
			}
			else {
				instruction.InternalCode = code16;
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + Register.AX;
			}
			Debug.Assert(state.mod != 3);
			instruction.InternalOp1Kind = OpKind.Memory;
			decoder.ReadOpMem(ref instruction);
		}
	}

	sealed class OpCodeHandler_RvMw_Gw : OpCodeHandlerModRM {
		readonly Code code16;
		readonly Code code32;

		public OpCodeHandler_RvMw_Gw(Code code16, Code code32) {
			this.code16 = code16;
			this.code32 = code32;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			Register baseReg;
			if (state.operandSize != OpSize.Size16) {
				instruction.InternalCode = code32;
				instruction.InternalOp1Register = (int)(state.reg + state.extraRegisterBase) + Register.EAX;
				baseReg = Register.EAX;
			}
			else {
				instruction.InternalCode = code16;
				instruction.InternalOp1Register = (int)(state.reg + state.extraRegisterBase) + Register.AX;
				baseReg = Register.AX;
			}
			if (state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.rm + state.extraBaseRegisterBase) + baseReg;
			}
			else {
				instruction.InternalOp0Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
			}
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.InternalOp1Kind = OpKind.Register;
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
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
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
				decoder.ReadOpMem(ref instruction);
			}
			if (state.operandSize == OpSize.Size32) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + Register.EAX;
				instruction.InternalCode = code32;
				instruction.InternalOp2Kind = OpKind.Immediate8to32;
				instruction.InternalImmediate8 = decoder.ReadByte();
			}
			else if (state.operandSize == OpSize.Size64) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + Register.RAX;
				instruction.InternalCode = code64;
				instruction.InternalOp2Kind = OpKind.Immediate8to64;
				instruction.InternalImmediate8 = decoder.ReadByte();
			}
			else {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + Register.AX;
				instruction.InternalCode = code16;
				instruction.InternalOp2Kind = OpKind.Immediate8to16;
				instruction.InternalImmediate8 = decoder.ReadByte();
			}
		}
	}

	sealed class OpCodeHandler_Gv_Ev_Ib_REX : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_Gv_Ev_Ib_REX(Register baseReg, Code code32, Code code64) {
			this.baseReg = baseReg;
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if ((state.flags & StateFlags.W) != 0) {
				instruction.InternalCode = code64;
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + Register.RAX;
			}
			else {
				instruction.InternalCode = code32;
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + Register.EAX;
			}
			Debug.Assert(state.mod == 3);
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.InternalOp1Register = (int)(state.rm + state.extraBaseRegisterBase) + baseReg;
			instruction.InternalOp2Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadByte();
		}
	}

	sealed class OpCodeHandler_Gv_Ev_32_64 : OpCodeHandlerModRM {
		readonly Code code32;
		readonly Code code64;
		readonly uint disallowReg;
		readonly uint disallowMem;

		public OpCodeHandler_Gv_Ev_32_64(Code code32, Code code64, bool allowReg, bool allowMem) {
			this.code32 = code32;
			this.code64 = code64;
			disallowMem = allowMem ? 0 : uint.MaxValue;
			disallowReg = allowReg ? 0 : uint.MaxValue;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			Register baseReg;
			if (decoder.is64Mode) {
				instruction.InternalCode = code64;
				baseReg = Register.RAX;
			}
			else {
				instruction.InternalCode = code32;
				baseReg = Register.EAX;
			}
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + baseReg;
			if (state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.InternalOp1Register = (int)(state.rm + state.extraBaseRegisterBase) + baseReg;
				if ((disallowReg & decoder.invalidCheckMask) != 0)
					decoder.SetInvalidInstruction();
			}
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
				if ((disallowMem & decoder.invalidCheckMask) != 0)
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
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
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
				decoder.ReadOpMem(ref instruction);
			}
			if (state.operandSize == OpSize.Size32) {
				instruction.InternalCode = code32;
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + Register.EAX;
				instruction.InternalOp2Kind = OpKind.Immediate32;
				instruction.Immediate32 = decoder.ReadUInt32();
			}
			else if (state.operandSize == OpSize.Size64) {
				instruction.InternalCode = code64;
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + Register.RAX;
				instruction.InternalOp2Kind = OpKind.Immediate32to64;
				instruction.Immediate32 = decoder.ReadUInt32();
			}
			else {
				instruction.InternalCode = code16;
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
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
			else if (state.addressSize == OpSize.Size32)
				instruction.InternalOp0Kind = OpKind.MemoryESEDI;
			else
				instruction.InternalOp0Kind = OpKind.MemoryESDI;
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.InternalOp1Register = reg;
		}
	}

	sealed class OpCodeHandler_Yv_Reg : OpCodeHandler {
		readonly Code code16;
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_Yv_Reg(Code code16, Code code32, Code code64) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if (state.addressSize == OpSize.Size64)
				instruction.InternalOp0Kind = OpKind.MemoryESRDI;
			else if (state.addressSize == OpSize.Size32)
				instruction.InternalOp0Kind = OpKind.MemoryESEDI;
			else
				instruction.InternalOp0Kind = OpKind.MemoryESDI;
			if (state.operandSize == OpSize.Size32) {
				instruction.InternalCode = code32;
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.InternalOp1Register = Register.EAX;
			}
			else if (state.operandSize == OpSize.Size64) {
				instruction.InternalCode = code64;
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.InternalOp1Register = Register.RAX;
			}
			else {
				instruction.InternalCode = code16;
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.InternalOp1Register = Register.AX;
			}
		}
	}

	sealed class OpCodeHandler_Yv_Reg2 : OpCodeHandler {
		readonly Code code16;
		readonly Code code32;

		public OpCodeHandler_Yv_Reg2(Code code16, Code code32) {
			this.code16 = code16;
			this.code32 = code32;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if (state.addressSize == OpSize.Size64)
				instruction.InternalOp0Kind = OpKind.MemoryESRDI;
			else if (state.addressSize == OpSize.Size32)
				instruction.InternalOp0Kind = OpKind.MemoryESEDI;
			else
				instruction.InternalOp0Kind = OpKind.MemoryESDI;
			if (state.operandSize != OpSize.Size16) {
				instruction.InternalCode = code32;
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.InternalOp1Register = Register.DX;
			}
			else {
				instruction.InternalCode = code16;
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.InternalOp1Register = Register.DX;
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
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = reg;
			if (state.addressSize == OpSize.Size64)
				instruction.InternalOp1Kind = OpKind.MemorySegRSI;
			else if (state.addressSize == OpSize.Size32)
				instruction.InternalOp1Kind = OpKind.MemorySegESI;
			else
				instruction.InternalOp1Kind = OpKind.MemorySegSI;
		}
	}

	sealed class OpCodeHandler_Reg_Xv : OpCodeHandler {
		readonly Code code16;
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_Reg_Xv(Code code16, Code code32, Code code64) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if (state.addressSize == OpSize.Size64)
				instruction.InternalOp1Kind = OpKind.MemorySegRSI;
			else if (state.addressSize == OpSize.Size32)
				instruction.InternalOp1Kind = OpKind.MemorySegESI;
			else
				instruction.InternalOp1Kind = OpKind.MemorySegSI;
			if (state.operandSize == OpSize.Size32) {
				instruction.InternalCode = code32;
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = Register.EAX;
			}
			else if (state.operandSize == OpSize.Size64) {
				instruction.InternalCode = code64;
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = Register.RAX;
			}
			else {
				instruction.InternalCode = code16;
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = Register.AX;
			}
		}
	}

	sealed class OpCodeHandler_Reg_Xv2 : OpCodeHandler {
		readonly Code code16;
		readonly Code code32;

		public OpCodeHandler_Reg_Xv2(Code code16, Code code32) {
			this.code16 = code16;
			this.code32 = code32;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if (state.addressSize == OpSize.Size64)
				instruction.InternalOp1Kind = OpKind.MemorySegRSI;
			else if (state.addressSize == OpSize.Size32)
				instruction.InternalOp1Kind = OpKind.MemorySegESI;
			else
				instruction.InternalOp1Kind = OpKind.MemorySegSI;
			if (state.operandSize != OpSize.Size16) {
				instruction.InternalCode = code32;
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = Register.DX;
			}
			else {
				instruction.InternalCode = code16;
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = Register.DX;
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
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = reg;
			if (state.addressSize == OpSize.Size64)
				instruction.InternalOp1Kind = OpKind.MemoryESRDI;
			else if (state.addressSize == OpSize.Size32)
				instruction.InternalOp1Kind = OpKind.MemoryESEDI;
			else
				instruction.InternalOp1Kind = OpKind.MemoryESDI;
		}
	}

	sealed class OpCodeHandler_Reg_Yv : OpCodeHandler {
		readonly Code code16;
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_Reg_Yv(Code code16, Code code32, Code code64) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if (state.addressSize == OpSize.Size64)
				instruction.InternalOp1Kind = OpKind.MemoryESRDI;
			else if (state.addressSize == OpSize.Size32)
				instruction.InternalOp1Kind = OpKind.MemoryESEDI;
			else
				instruction.InternalOp1Kind = OpKind.MemoryESDI;
			if (state.operandSize == OpSize.Size32) {
				instruction.InternalCode = code32;
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = Register.EAX;
			}
			else if (state.operandSize == OpSize.Size64) {
				instruction.InternalCode = code64;
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = Register.RAX;
			}
			else {
				instruction.InternalCode = code16;
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = Register.AX;
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
			else if (state.addressSize == OpSize.Size32) {
				instruction.InternalOp0Kind = OpKind.MemoryESEDI;
				instruction.InternalOp1Kind = OpKind.MemorySegESI;
			}
			else {
				instruction.InternalOp0Kind = OpKind.MemoryESDI;
				instruction.InternalOp1Kind = OpKind.MemorySegSI;
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
			else if (state.addressSize == OpSize.Size32) {
				instruction.InternalOp0Kind = OpKind.MemoryESEDI;
				instruction.InternalOp1Kind = OpKind.MemorySegESI;
			}
			else {
				instruction.InternalOp0Kind = OpKind.MemoryESDI;
				instruction.InternalOp1Kind = OpKind.MemorySegSI;
			}
			if (state.operandSize == OpSize.Size32)
				instruction.InternalCode = code32;
			else if (state.operandSize == OpSize.Size64)
				instruction.InternalCode = code64;
			else
				instruction.InternalCode = code16;
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
			else if (state.addressSize == OpSize.Size32) {
				instruction.InternalOp0Kind = OpKind.MemorySegESI;
				instruction.InternalOp1Kind = OpKind.MemoryESEDI;
			}
			else {
				instruction.InternalOp0Kind = OpKind.MemorySegSI;
				instruction.InternalOp1Kind = OpKind.MemoryESDI;
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
			else if (state.addressSize == OpSize.Size32) {
				instruction.InternalOp0Kind = OpKind.MemorySegESI;
				instruction.InternalOp1Kind = OpKind.MemoryESEDI;
			}
			else {
				instruction.InternalOp0Kind = OpKind.MemorySegSI;
				instruction.InternalOp1Kind = OpKind.MemoryESDI;
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
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
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
				decoder.ReadOpMem(ref instruction);
			}
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.InternalOp1Register = decoder.ReadOpSegReg();
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
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + Register.EAX;
			}
			else if (state.operandSize == OpSize.Size64) {
				instruction.InternalCode = code64;
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + Register.RAX;
			}
			else {
				instruction.InternalCode = code16;
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + Register.AX;
			}
			if (state.mod == 3)
				decoder.SetInvalidInstruction();
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
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
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.InternalOp0Kind = OpKind.Register;
			var sreg = decoder.ReadOpSegReg();
			if (decoder.invalidCheckMask != 0 && sreg == Register.CS)
				decoder.SetInvalidInstruction();
			instruction.InternalOp0Register = sreg;
			if (state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
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
				decoder.ReadOpMem(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_Ap : OpCodeHandler {
		readonly Code code16;
		readonly Code code32;

		public OpCodeHandler_Ap(Code code16, Code code32) {
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
			if (state.operandSize != OpSize.Size16) {
				instruction.InternalOp0Kind = OpKind.FarBranch32;
				instruction.FarBranch32 = decoder.ReadUInt32();
			}
			else {
				instruction.InternalOp0Kind = OpKind.FarBranch16;
				instruction.InternalFarBranch16 = decoder.ReadUInt16();
			}
			instruction.InternalFarBranchSelector = decoder.ReadUInt16();
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
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = reg;
			decoder.displIndex = state.instructionLength;
			if (state.addressSize == OpSize.Size64) {
				instruction.InternalSetMemoryDisplSize(4);
				state.flags |= StateFlags.Addr64;
				instruction.InternalMemoryAddress64_lo = decoder.ReadUInt32();
				instruction.InternalMemoryAddress64_hi = decoder.ReadUInt32();
				instruction.InternalOp1Kind = OpKind.Memory64;
			}
			else if (state.addressSize == OpSize.Size32) {
				instruction.InternalSetMemoryDisplSize(3);
				instruction.MemoryDisplacement = decoder.ReadUInt32();
				//instruction.InternalMemoryIndexScale = 0;
				//instruction.InternalMemoryBase = Register.None;
				//instruction.InternalMemoryIndex = Register.None;
				instruction.InternalOp1Kind = OpKind.Memory;
			}
			else {
				instruction.InternalSetMemoryDisplSize(2);
				instruction.MemoryDisplacement = decoder.ReadUInt16();
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
			decoder.displIndex = state.instructionLength;
			if (state.addressSize == OpSize.Size64) {
				instruction.InternalSetMemoryDisplSize(4);
				state.flags |= StateFlags.Addr64;
				instruction.InternalMemoryAddress64_lo = decoder.ReadUInt32();
				instruction.InternalMemoryAddress64_hi = decoder.ReadUInt32();
				instruction.InternalOp0Kind = OpKind.Memory64;
			}
			else if (state.addressSize == OpSize.Size32) {
				instruction.InternalSetMemoryDisplSize(3);
				instruction.MemoryDisplacement = decoder.ReadUInt32();
				//instruction.InternalMemoryIndexScale = 0;
				//instruction.InternalMemoryBase = Register.None;
				//instruction.InternalMemoryIndex = Register.None;
				instruction.InternalOp0Kind = OpKind.Memory;
			}
			else {
				instruction.InternalSetMemoryDisplSize(2);
				instruction.MemoryDisplacement = decoder.ReadUInt16();
				//instruction.InternalMemoryIndexScale = 0;
				//instruction.InternalMemoryBase = Register.None;
				//instruction.InternalMemoryIndex = Register.None;
				instruction.InternalOp0Kind = OpKind.Memory;
			}
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.InternalOp1Register = reg;
		}
	}

	sealed class OpCodeHandler_Reg_Ov : OpCodeHandler {
		readonly Code code16;
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_Reg_Ov(Code code16, Code code32, Code code64) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
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
				instruction.InternalOp1Kind = OpKind.Memory64;
			}
			else if (state.addressSize == OpSize.Size32) {
				instruction.InternalSetMemoryDisplSize(3);
				instruction.MemoryDisplacement = decoder.ReadUInt32();
				//instruction.InternalMemoryIndexScale = 0;
				//instruction.InternalMemoryBase = Register.None;
				//instruction.InternalMemoryIndex = Register.None;
				instruction.InternalOp1Kind = OpKind.Memory;
			}
			else {
				instruction.InternalSetMemoryDisplSize(2);
				instruction.MemoryDisplacement = decoder.ReadUInt16();
				//instruction.InternalMemoryIndexScale = 0;
				//instruction.InternalMemoryBase = Register.None;
				//instruction.InternalMemoryIndex = Register.None;
				instruction.InternalOp1Kind = OpKind.Memory;
			}
			if (state.operandSize == OpSize.Size32) {
				instruction.InternalCode = code32;
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = Register.EAX;
			}
			else if (state.operandSize == OpSize.Size64) {
				instruction.InternalCode = code64;
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = Register.RAX;
			}
			else {
				instruction.InternalCode = code16;
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = Register.AX;
			}
		}
	}

	sealed class OpCodeHandler_Ov_Reg : OpCodeHandler {
		readonly Code code16;
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_Ov_Reg(Code code16, Code code32, Code code64) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
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
			else if (state.addressSize == OpSize.Size32) {
				instruction.InternalSetMemoryDisplSize(3);
				instruction.MemoryDisplacement = decoder.ReadUInt32();
				//instruction.InternalMemoryIndexScale = 0;
				//instruction.InternalMemoryBase = Register.None;
				//instruction.InternalMemoryIndex = Register.None;
				instruction.InternalOp0Kind = OpKind.Memory;
			}
			else {
				instruction.InternalSetMemoryDisplSize(2);
				instruction.MemoryDisplacement = decoder.ReadUInt16();
				//instruction.InternalMemoryIndexScale = 0;
				//instruction.InternalMemoryBase = Register.None;
				//instruction.InternalMemoryIndex = Register.None;
				instruction.InternalOp0Kind = OpKind.Memory;
			}
			if (state.operandSize == OpSize.Size32) {
				instruction.InternalCode = code32;
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.InternalOp1Register = Register.EAX;
			}
			else if (state.operandSize == OpSize.Size64) {
				instruction.InternalCode = code64;
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.InternalOp1Register = Register.RAX;
			}
			else {
				instruction.InternalCode = code16;
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.InternalOp1Register = Register.AX;
			}
		}
	}

	sealed class OpCodeHandler_BranchIw : OpCodeHandler {
		readonly Code code16;
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_BranchIw(Code code16, Code code32, Code code64) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if (decoder.is64Mode) {
				if ((decoder.options & DecoderOptions.AMD) == 0 || state.operandSize == OpSize.Size32)
					instruction.InternalCode = code64;
				else
					instruction.InternalCode = code16;
			}
			else {
				if (state.operandSize == OpSize.Size32)
					instruction.InternalCode = code32;
				else
					instruction.InternalCode = code16;
			}
			instruction.InternalOp0Kind = OpKind.Immediate16;
			instruction.InternalImmediate16 = decoder.ReadUInt16();
		}
	}

	sealed class OpCodeHandler_BranchSimple : OpCodeHandler {
		readonly Code code16;
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_BranchSimple(Code code16, Code code32, Code code64) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			if (decoder.is64Mode) {
				if ((decoder.options & DecoderOptions.AMD) == 0 || decoder.state.operandSize == OpSize.Size32)
					instruction.InternalCode = code64;
				else
					instruction.InternalCode = code16;
			}
			else {
				if (decoder.state.operandSize == OpSize.Size32)
					instruction.InternalCode = code32;
				else
					instruction.InternalCode = code16;
			}
		}
	}

	sealed class OpCodeHandler_Iw_Ib : OpCodeHandler {
		readonly Code code16;
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_Iw_Ib(Code code16, Code code32, Code code64) {
			this.code16 = code16;
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if (decoder.is64Mode) {
				if (state.operandSize != OpSize.Size16)
					instruction.InternalCode = code64;
				else
					instruction.InternalCode = code16;
			}
			else {
				if (state.operandSize == OpSize.Size32)
					instruction.InternalCode = code32;
				else
					instruction.InternalCode = code16;
			}
			instruction.InternalOp0Kind = OpKind.Immediate16;
			instruction.InternalImmediate16 = decoder.ReadUInt16();
			instruction.InternalOp1Kind = OpKind.Immediate8_2nd;
			instruction.InternalImmediate8_2nd = decoder.ReadByte();
		}
	}

	sealed class OpCodeHandler_Reg_Ib2 : OpCodeHandler {
		readonly Code code16;
		readonly Code code32;

		public OpCodeHandler_Reg_Ib2(Code code16, Code code32) {
			this.code16 = code16;
			this.code32 = code32;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.InternalOp0Kind = OpKind.Register;
			if (state.operandSize != OpSize.Size16) {
				instruction.InternalCode = code32;
				instruction.InternalOp0Register = Register.EAX;
			}
			else {
				instruction.InternalCode = code16;
				instruction.InternalOp0Register = Register.AX;
			}
			instruction.InternalOp1Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadByte();
		}
	}

	sealed class OpCodeHandler_IbReg2 : OpCodeHandler {
		readonly Code code16;
		readonly Code code32;

		public OpCodeHandler_IbReg2(Code code16, Code code32) {
			this.code16 = code16;
			this.code32 = code32;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalOp0Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadByte();
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.InternalOp1Kind = OpKind.Register;
			if (state.operandSize != OpSize.Size16) {
				instruction.InternalCode = code32;
				instruction.InternalOp1Register = Register.EAX;
			}
			else {
				instruction.InternalCode = code16;
				instruction.InternalOp1Register = Register.AX;
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
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = Register.EAX;
			}
			else {
				instruction.InternalCode = code16;
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = Register.AX;
			}
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
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
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = Register.DX;
			if (state.operandSize != OpSize.Size16) {
				instruction.InternalCode = code32;
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.InternalOp1Register = Register.EAX;
			}
			else {
				instruction.InternalCode = code16;
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
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
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)index + Register.AL;
			}
			else {
				instruction.InternalOp0Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
				if ((flags & (HandlerFlags.Xacquire | HandlerFlags.Xrelease)) != 0)
					decoder.SetXacquireXrelease(ref instruction, flags);
				Static.Assert((int)HandlerFlags.Lock == 8 ? 0 : -1);
				Static.Assert((int)StateFlags.AllowLock == 0x00002000 ? 0 : -1);
				state.flags |= (StateFlags)((uint)(flags & HandlerFlags.Lock) << (13 - 3));
			}
			instruction.InternalOp1Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadByte();
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
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)index + Register.AL;
			}
			else {
				instruction.InternalOp0Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
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
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)index + Register.AL;
			}
			else {
				instruction.InternalOp0Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
			}
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
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
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)index + Register.AL;
			}
			else {
				instruction.InternalOp0Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
				if ((flags & (HandlerFlags.Xacquire | HandlerFlags.Xrelease)) != 0)
					decoder.SetXacquireXrelease(ref instruction, flags);
				Static.Assert((int)HandlerFlags.Lock == 8 ? 0 : -1);
				Static.Assert((int)StateFlags.AllowLock == 0x00002000 ? 0 : -1);
				state.flags |= (StateFlags)((uint)(flags & HandlerFlags.Lock) << (13 - 3));
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
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)index + Register.AL;
			}
			else {
				instruction.InternalOp0Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
				if ((flags & (HandlerFlags.Xacquire | HandlerFlags.Xrelease)) != 0)
					decoder.SetXacquireXrelease(ref instruction, flags);
				Static.Assert((int)HandlerFlags.Lock == 8 ? 0 : -1);
				Static.Assert((int)StateFlags.AllowLock == 0x00002000 ? 0 : -1);
				state.flags |= (StateFlags)((uint)(flags & HandlerFlags.Lock) << (13 - 3));
			}
			index = state.reg + state.extraRegisterBase;
			if ((state.flags & StateFlags.HasRex) != 0 && index >= 4)
				index += 4;
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
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
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = (int)index + Register.AL;

			if (state.mod == 3) {
				index = state.rm + state.extraBaseRegisterBase;
				if ((state.flags & StateFlags.HasRex) != 0 && index >= 4)
					index += 4;
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.InternalOp1Register = (int)index + Register.AL;
			}
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
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
				decoder.ReadOpMem(ref instruction);
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
				decoder.ReadOpMem(ref instruction);
				if ((flags & (HandlerFlags.Xacquire | HandlerFlags.Xrelease)) != 0)
					decoder.SetXacquireXrelease(ref instruction, flags);
				Static.Assert((int)HandlerFlags.Lock == 8 ? 0 : -1);
				Static.Assert((int)StateFlags.AllowLock == 0x00002000 ? 0 : -1);
				state.flags |= (StateFlags)((uint)(flags & HandlerFlags.Lock) << (13 - 3));
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
			else if (state.addressSize == OpSize.Size32)
				instruction.InternalMemoryBase = Register.EBX;
			else
				instruction.InternalMemoryBase = Register.BX;
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
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + baseReg;
			if (state.mod == 3) {
				instruction.InternalCode = codeR;
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.InternalOp1Register = (int)(state.rm + state.extraBaseRegisterBase) + baseReg;
			}
			else {
				instruction.InternalCode = codeM;
				instruction.InternalOp1Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
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
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.rm + state.extraBaseRegisterBase) + baseReg;
			}
			else {
				instruction.InternalOp0Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
			}
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
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
			if (state.addressSize == OpSize.Size64)
				instruction.InternalOp0Kind = OpKind.MemorySegRDI;
			else if (state.addressSize == OpSize.Size32)
				instruction.InternalOp0Kind = OpKind.MemorySegEDI;
			else
				instruction.InternalOp0Kind = OpKind.MemorySegDI;
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.InternalOp1Register = (int)(state.reg + state.extraRegisterBase) + baseReg;
			if (state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp2Kind = OpKind.Register;
				instruction.InternalOp2Register = (int)(state.rm + state.extraBaseRegisterBase) + baseReg;
			}
			else
				decoder.SetInvalidInstruction();
		}
	}

	sealed class OpCodeHandler_rDI_P_N : OpCodeHandlerModRM {
		readonly Code code;

		public OpCodeHandler_rDI_P_N(Code code) => this.code = code;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code;
			if (state.addressSize == OpSize.Size64)
				instruction.InternalOp0Kind = OpKind.MemorySegRDI;
			else if (state.addressSize == OpSize.Size32)
				instruction.InternalOp0Kind = OpKind.MemorySegEDI;
			else
				instruction.InternalOp0Kind = OpKind.MemorySegDI;
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.InternalOp1Register = (int)state.reg + Register.MM0;
			if (state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
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
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + baseReg;
			if (state.mod == 3)
				decoder.SetInvalidInstruction();
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
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
				decoder.ReadOpMem(ref instruction);
			}
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
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
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + baseReg;
			if (state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.InternalOp1Register = (int)state.rm + Register.MM0;
			}
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_P_Q : OpCodeHandlerModRM {
		readonly Code code;

		public OpCodeHandler_P_Q(Code code) => this.code = code;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code;
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = (int)state.reg + Register.MM0;
			if (state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.InternalOp1Register = (int)state.rm + Register.MM0;
			}
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_Q_P : OpCodeHandlerModRM {
		readonly Code code;

		public OpCodeHandler_Q_P(Code code) => this.code = code;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code;
			if (state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)state.rm + Register.MM0;
			}
			else {
				instruction.InternalOp0Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
			}
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.InternalOp1Register = (int)state.reg + Register.MM0;
		}
	}

	sealed class OpCodeHandler_MP : OpCodeHandlerModRM {
		readonly Code code;

		public OpCodeHandler_MP(Code code) => this.code = code;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code;
			if (state.mod == 3)
				decoder.SetInvalidInstruction();
			else {
				instruction.InternalOp0Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
			}
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.InternalOp1Register = (int)state.reg + Register.MM0;
		}
	}

	sealed class OpCodeHandler_P_Q_Ib : OpCodeHandlerModRM {
		readonly Code code;

		public OpCodeHandler_P_Q_Ib(Code code) => this.code = code;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code;
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = (int)state.reg + Register.MM0;
			if (state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.InternalOp1Register = (int)state.rm + Register.MM0;
			}
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
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
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = (int)state.reg + Register.MM0;
			if (state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.InternalOp1Register = (int)(state.rm + state.extraBaseRegisterBase) + baseReg;
			}
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
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
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = (int)state.reg + Register.MM0;
			if (state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
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
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = (int)state.reg + Register.MM0;
			if (state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.InternalOp1Register = (int)(state.rm + state.extraBaseRegisterBase) + gpr;
			}
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
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
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = (int)state.reg + Register.MM0;
			if (state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.InternalOp1Register = (int)(state.rm + state.extraBaseRegisterBase) + gpr;
			}
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
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
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.rm + state.extraBaseRegisterBase) + gpr;
			}
			else {
				instruction.InternalOp0Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
			}
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
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
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + Register.RAX;
			}
			else {
				instruction.InternalCode = codeW0;
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + Register.EAX;
			}
			if (state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.InternalOp1Register = (int)(state.rm + state.extraBaseRegisterBase) + baseReg;
			}
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
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
			Register gpr;
			if (state.operandSize != OpSize.Size64) {
				instruction.InternalCode = codeW0;
				gpr = Register.EAX;
			}
			else {
				instruction.InternalCode = codeW1;
				gpr = Register.RAX;
			}
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + baseReg;
			if (state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.InternalOp1Register = (int)(state.rm + state.extraBaseRegisterBase) + gpr;
			}
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
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
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + baseReg;
			if (state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.InternalOp1Register = (int)(state.rm + state.extraBaseRegisterBase) + baseReg;
			}
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
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
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + baseReg;
			if (state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
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
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
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
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
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
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.rm + state.extraBaseRegisterBase) + gpr;
			}
			else {
				instruction.InternalOp0Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
			}
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
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
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + Register.XMM0;
			if (state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.InternalOp1Register = (int)(state.rm + state.extraBaseRegisterBase) + gpr;
			}
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
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
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.rm + state.extraBaseRegisterBase) + gpr;
			}
			else {
				instruction.InternalOp0Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
			}
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
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
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + baseReg;
			if (state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.InternalOp1Register = (int)(state.rm + state.extraBaseRegisterBase) + gpr;
			}
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
			}
			instruction.InternalOp2Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadByte();
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
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.InternalOp0Kind = OpKind.Register;
			if ((state.flags & StateFlags.W) != 0)
				instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + Register.RAX;
			else
				instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + Register.EAX;
			if (state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
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
			if (state.reg > 3 || ((state.extraRegisterBase & decoder.invalidCheckMask) != 0))
				decoder.SetInvalidInstruction();
			instruction.InternalCode = code;
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = (int)state.reg + Register.BND0;
			Debug.Assert(state.mod != 3);
			instruction.InternalOp1Kind = OpKind.Memory;
			decoder.ReadOpMem_MPX(ref instruction);
			// It can't be EIP since if it's MPX + 64-bit, the address size is always 64-bit
			if (decoder.invalidCheckMask != 0 && instruction.MemoryBase == Register.RIP)
				decoder.SetInvalidInstruction();
		}
	}

	sealed class OpCodeHandler_MIB_B : OpCodeHandlerModRM {
		readonly Code code;

		public OpCodeHandler_MIB_B(Code code) => this.code = code;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if (state.reg > 3 || ((state.extraRegisterBase & decoder.invalidCheckMask) != 0))
				decoder.SetInvalidInstruction();
			instruction.InternalCode = code;
			Debug.Assert(state.mod != 3);
			instruction.InternalOp0Kind = OpKind.Memory;
			decoder.ReadOpMem_MPX(ref instruction);
			// It can't be EIP since if it's MPX + 64-bit, the address size is always 64-bit
			if (decoder.invalidCheckMask != 0 && instruction.MemoryBase == Register.RIP)
				decoder.SetInvalidInstruction();
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.InternalOp1Register = (int)state.reg + Register.BND0;
		}
	}

	sealed class OpCodeHandler_B_BM : OpCodeHandlerModRM {
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_B_BM(Code code32, Code code64) {
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if ((state.reg | state.rm) > 3 || (((state.extraRegisterBase | state.extraBaseRegisterBase) & decoder.invalidCheckMask) != 0))
				decoder.SetInvalidInstruction();
			if (decoder.is64Mode)
				instruction.InternalCode = code64;
			else
				instruction.InternalCode = code32;
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = (int)state.reg + Register.BND0;
			if (state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.InternalOp1Register = (int)state.rm + Register.BND0;
			}
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				decoder.ReadOpMem_MPX(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_BM_B : OpCodeHandlerModRM {
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_BM_B(Code code32, Code code64) {
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if ((state.reg | state.rm) > 3 || (((state.extraRegisterBase | state.extraBaseRegisterBase) & decoder.invalidCheckMask) != 0))
				decoder.SetInvalidInstruction();
			if (decoder.is64Mode)
				instruction.InternalCode = code64;
			else
				instruction.InternalCode = code32;
			if (state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)state.rm + Register.BND0;
			}
			else {
				instruction.InternalOp0Kind = OpKind.Memory;
				decoder.ReadOpMem_MPX(ref instruction);
			}
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.InternalOp1Register = (int)state.reg + Register.BND0;
		}
	}

	sealed class OpCodeHandler_B_Ev : OpCodeHandlerModRM {
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_B_Ev(Code code32, Code code64) {
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			if (state.reg > 3 || ((state.extraRegisterBase & decoder.invalidCheckMask) != 0))
				decoder.SetInvalidInstruction();
			Register baseReg;
			if (decoder.is64Mode) {
				instruction.InternalCode = code64;
				baseReg = Register.RAX;
			}
			else {
				instruction.InternalCode = code32;
				baseReg = Register.EAX;
			}
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = (int)state.reg + Register.BND0;
			if (state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.InternalOp1Register = (int)(state.rm + state.extraBaseRegisterBase) + baseReg;
			}
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				decoder.ReadOpMem_MPX(ref instruction);
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
				decoder.ReadOpMem(ref instruction);
			}
			if ((state.flags & StateFlags.W) != 0) {
				instruction.InternalCode = code64;
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.InternalOp1Register = (int)(state.reg + state.extraRegisterBase) + Register.RAX;
			}
			else {
				instruction.InternalCode = code32;
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
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
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.InternalOp0Kind = OpKind.Register;
			if ((state.flags & StateFlags.W) != 0)
				instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + Register.RAX;
			else
				instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + Register.EAX;
			if (state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.InternalOp1Register = (int)state.rm + Register.MM0;
			}
			else
				decoder.SetInvalidInstruction();
			instruction.InternalOp2Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadByte();
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
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.InternalOp0Kind = OpKind.Register;
			if ((state.flags & StateFlags.W) != 0)
				instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + Register.RAX;
			else
				instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + Register.EAX;
			if (state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
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
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + baseReg;
			if (state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
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
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + Register.EAX;
			}
			else if (state.operandSize == OpSize.Size64) {
				instruction.InternalCode = code64;
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + Register.RAX;
			}
			else {
				instruction.InternalCode = code16;
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + Register.AX;
			}
			if (state.mod == 3)
				decoder.SetInvalidInstruction();
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
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
				decoder.ReadOpMem(ref instruction);
			}
			if (state.operandSize == OpSize.Size32) {
				instruction.InternalCode = code32;
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.InternalOp1Register = (int)(state.reg + state.extraRegisterBase) + Register.EAX;
			}
			else if (state.operandSize == OpSize.Size64) {
				instruction.InternalCode = code64;
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.InternalOp1Register = (int)(state.reg + state.extraRegisterBase) + Register.RAX;
			}
			else {
				instruction.InternalCode = code16;
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
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
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + Register.RAX;
			}
			else {
				instruction.InternalCode = code32;
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + Register.EAX;
			}
			if (state.mod == 3) {
				uint index = state.rm + state.extraBaseRegisterBase;
				if ((state.flags & StateFlags.HasRex) != 0 && index >= 4)
					index += 4;
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.InternalOp1Register = (int)index + Register.AL;
			}
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
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
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + Register.RAX;
			}
			else {
				instruction.InternalCode = code32;
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.reg + state.extraRegisterBase) + Register.EAX;
			}
			if (state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp1Kind = OpKind.Register;
				if ((state.flags & StateFlags.W) != 0)
					instruction.InternalOp1Register = (int)(state.rm + state.extraBaseRegisterBase) + Register.RAX;
				else
					instruction.InternalOp1Register = (int)(state.rm + state.extraBaseRegisterBase) + Register.EAX;
			}
			else {
				instruction.InternalOp1Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
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
			Debug.Assert(state.mod != 3);
			instruction.InternalOp0Kind = OpKind.Memory;
			decoder.ReadOpMem(ref instruction);
			if ((state.flags & StateFlags.W) != 0) {
				instruction.InternalCode = code64;
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp1Kind = OpKind.Register;
				instruction.InternalOp1Register = (int)(state.reg + state.extraRegisterBase) + Register.RAX;
			}
			else {
				instruction.InternalCode = code32;
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
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
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)(state.rm + state.extraBaseRegisterBase) + gpr;
			}
			else {
				instruction.InternalOp0Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
			}
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.InternalOp1Register = (int)(state.reg + state.extraRegisterBase) + baseReg;
			instruction.InternalOp2Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadByte();
		}
	}

	sealed class OpCodeHandler_Wbinvd : OpCodeHandler {
		public OpCodeHandler_Wbinvd() { }

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			if ((decoder.options & DecoderOptions.NoWbnoinvd) != 0 || decoder.state.mandatoryPrefix != MandatoryPrefixByte.PF3)
				instruction.InternalCode = Code.Wbinvd;
			else {
				decoder.ClearMandatoryPrefixF3(ref instruction);
				instruction.InternalCode = Code.Wbnoinvd;
			}
		}
	}
}
#endif
