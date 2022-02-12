// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if DECODER
using System;
using System.Diagnostics;

namespace Iced.Intel.DecoderInternal {
	// fixed fields must be in structs and can't be inlined
	struct Code3 {
		public unsafe fixed ushort codes[3];
		public Code3(Code code16, Code code32, Code code64) {
			unsafe {
				codes[0] = (ushort)code16;
				codes[1] = (ushort)code32;
				codes[2] = (ushort)code64;
			}
		}
	}

	sealed class OpCodeHandler_VEX2 : OpCodeHandlerModRM {
		readonly OpCodeHandler handlerMem;

		public OpCodeHandler_VEX2(OpCodeHandler handlerMem) => this.handlerMem = handlerMem ?? throw new ArgumentNullException(nameof(handlerMem));

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			if (decoder.is64bMode)
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
			if (decoder.is64bMode)
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
			if (decoder.is64bMode)
				decoder.EVEX_MVEX(ref instruction);
			else if (decoder.state.mod == 3)
				decoder.EVEX_MVEX(ref instruction);
			else
				handlerMem.Decode(decoder, ref instruction);
		}
	}

	sealed class OpCodeHandler_PrefixEsCsSsDs : OpCodeHandler {
		readonly Register seg;

		public OpCodeHandler_PrefixEsCsSsDs(Register seg) {
			Debug.Assert(seg == Register.ES || seg == Register.CS || seg == Register.SS || seg == Register.DS);
			this.seg = seg;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);

			if (!decoder.is64bMode || decoder.state.zs.segmentPrio <= 0)
				instruction.SegmentPrefix = seg;

			decoder.ResetRexPrefixState();
			decoder.CallOpCodeHandlerXXTable(ref instruction);
		}
	}

	sealed class OpCodeHandler_PrefixFsGs : OpCodeHandler {
		readonly Register seg;

		public OpCodeHandler_PrefixFsGs(Register seg) {
			Debug.Assert(seg == Register.FS || seg == Register.GS);
			this.seg = seg;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);

			instruction.SegmentPrefix = seg;
			decoder.state.zs.segmentPrio = 1;

			decoder.ResetRexPrefixState();
			decoder.CallOpCodeHandlerXXTable(ref instruction);
		}
	}

	sealed class OpCodeHandler_Prefix66 : OpCodeHandler {
		public OpCodeHandler_Prefix66() { }

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);

			decoder.state.zs.flags |= StateFlags.Has66;
			decoder.state.operandSize = decoder.defaultInvertedOperandSize;
			if (decoder.state.zs.mandatoryPrefix == MandatoryPrefixByte.None)
				decoder.state.zs.mandatoryPrefix = MandatoryPrefixByte.P66;

			decoder.ResetRexPrefixState();
			decoder.CallOpCodeHandlerXXTable(ref instruction);
		}
	}

	sealed class OpCodeHandler_Prefix67 : OpCodeHandler {
		public OpCodeHandler_Prefix67() { }

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);

			decoder.state.addressSize = decoder.defaultInvertedAddressSize;

			decoder.ResetRexPrefixState();
			decoder.CallOpCodeHandlerXXTable(ref instruction);
		}
	}

	sealed class OpCodeHandler_PrefixF0 : OpCodeHandler {
		public OpCodeHandler_PrefixF0() { }

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);

			instruction.InternalSetHasLockPrefix();
			decoder.state.zs.flags |= StateFlags.Lock;

			decoder.ResetRexPrefixState();
			decoder.CallOpCodeHandlerXXTable(ref instruction);
		}
	}

	sealed class OpCodeHandler_PrefixF2 : OpCodeHandler {
		public OpCodeHandler_PrefixF2() { }

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);

			instruction.InternalSetHasRepnePrefix();
			decoder.state.zs.mandatoryPrefix = MandatoryPrefixByte.PF2;

			decoder.ResetRexPrefixState();
			decoder.CallOpCodeHandlerXXTable(ref instruction);
		}
	}

	sealed class OpCodeHandler_PrefixF3 : OpCodeHandler {
		public OpCodeHandler_PrefixF3() { }

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);

			instruction.InternalSetHasRepePrefix();
			decoder.state.zs.mandatoryPrefix = MandatoryPrefixByte.PF3;

			decoder.ResetRexPrefixState();
			decoder.CallOpCodeHandlerXXTable(ref instruction);
		}
	}

	sealed class OpCodeHandler_PrefixREX : OpCodeHandler {
		readonly OpCodeHandler handler;
		readonly uint rex;

		public OpCodeHandler_PrefixREX(OpCodeHandler handler, uint rex) {
			Debug.Assert(rex <= 0x0F);
			this.handler = handler ?? throw new InvalidOperationException();
			this.rex = rex;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);

			if (decoder.is64bMode) {
				if ((rex & 8) != 0) {
					decoder.state.operandSize = OpSize.Size64;
					decoder.state.zs.flags |= StateFlags.HasRex | StateFlags.W;
				}
				else {
					decoder.state.zs.flags |= StateFlags.HasRex;
					decoder.state.zs.flags &= ~StateFlags.W;
					if ((decoder.state.zs.flags & StateFlags.Has66) == 0)
						decoder.state.operandSize = OpSize.Size32;
					else
						decoder.state.operandSize = OpSize.Size16;
				}
				decoder.state.zs.extraRegisterBase = (rex << 1) & 8;
				decoder.state.zs.extraIndexRegisterBase = (rex << 2) & 8;
				decoder.state.zs.extraBaseRegisterBase = (rex << 3) & 8;

				decoder.CallOpCodeHandlerXXTable(ref instruction);
			}
			else
				handler.Decode(decoder, ref instruction);
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
			instruction.InternalSetCodeNoCheck(code);
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = reg;
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
			instruction.InternalSetCodeNoCheck(code);
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = reg;
			instruction.Op1Kind = OpKind.Immediate8;
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
			instruction.InternalSetCodeNoCheck(code);
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op1Kind = OpKind.Register;
			instruction.Op1Register = reg;
			instruction.Op0Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadByte();
		}
	}

	sealed class OpCodeHandler_AL_DX : OpCodeHandler {
		readonly Code code;

		public OpCodeHandler_AL_DX(Code code) => this.code = code;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			instruction.InternalSetCodeNoCheck(code);
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = Register.AL;
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op1Kind = OpKind.Register;
			instruction.Op1Register = Register.DX;
		}
	}

	sealed class OpCodeHandler_DX_AL : OpCodeHandler {
		readonly Code code;

		public OpCodeHandler_DX_AL(Code code) => this.code = code;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			instruction.InternalSetCodeNoCheck(code);
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = Register.DX;
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op1Kind = OpKind.Register;
			instruction.Op1Register = Register.AL;
		}
	}

	sealed class OpCodeHandler_Ib : OpCodeHandler {
		readonly Code code;

		public OpCodeHandler_Ib(Code code) => this.code = code;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			instruction.InternalSetCodeNoCheck(code);
			instruction.Op0Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadByte();
		}
	}

	sealed class OpCodeHandler_Ib3 : OpCodeHandlerModRM {
		readonly Code code;

		public OpCodeHandler_Ib3(Code code) => this.code = code;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			instruction.InternalSetCodeNoCheck(code);
			instruction.Op0Kind = OpKind.Immediate8;
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
			handlers[(int)decoder.state.zs.mandatoryPrefix].Decode(decoder, ref instruction);
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
			var info = handlers[(int)decoder.state.zs.mandatoryPrefix];
			if (info.mandatoryPrefix)
				decoder.ClearMandatoryPrefix(ref instruction);
			info.handler.Decode(decoder, ref instruction);
		}
	}

	sealed class OpCodeHandler_MandatoryPrefix4 : OpCodeHandler {
		readonly OpCodeHandler handlerNP;
		readonly OpCodeHandler handler66;
		readonly OpCodeHandler handlerF3;
		readonly OpCodeHandler handlerF2;
		readonly uint flags;

		public OpCodeHandler_MandatoryPrefix4(OpCodeHandler handlerNP, OpCodeHandler handler66, OpCodeHandler handlerF3, OpCodeHandler handlerF2, uint flags) {
			this.handlerNP = handlerNP ?? throw new ArgumentNullException(nameof(handlerNP));
			this.handler66 = handler66 ?? throw new ArgumentNullException(nameof(handler66));
			this.handlerF3 = handlerF3 ?? throw new ArgumentNullException(nameof(handlerF3));
			this.handlerF2 = handlerF2 ?? throw new ArgumentNullException(nameof(handlerF2));
			this.flags = flags;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			OpCodeHandler handler;
			var prefix = decoder.state.zs.mandatoryPrefix;
			switch (prefix) {
			case MandatoryPrefixByte.None:
				handler = handlerNP;
				break;
			case MandatoryPrefixByte.P66:
				handler = handler66;
				break;
			case MandatoryPrefixByte.PF3:
				if ((flags & 4) != 0)
					decoder.ClearMandatoryPrefixF3(ref instruction);
				handler = handlerF3;
				break;
			case MandatoryPrefixByte.PF2:
				if ((flags & 8) != 0)
					decoder.ClearMandatoryPrefixF2(ref instruction);
				handler = handlerF2;
				break;
			default:
				throw new InvalidOperationException();
			}
			if (handler.HasModRM && (flags & 0x10) != 0)
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
			handlers[(int)decoder.state.zs.mandatoryPrefix].Decode(decoder, ref instruction);
		}
	}

	sealed class OpCodeHandler_NIb : OpCodeHandlerModRM {
		readonly Code code;

		public OpCodeHandler_NIb(Code code) => this.code = code;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			instruction.InternalSetCodeNoCheck(code);
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op0Kind = OpKind.Register;
				instruction.Op0Register = (int)decoder.state.rm + Register.MM0;
			}
			else
				decoder.SetInvalidInstruction();
			instruction.Op1Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadByte();
		}
	}

	sealed class OpCodeHandler_Reservednop : OpCodeHandlerModRM {
		readonly OpCodeHandler reservedNopHandler;
		readonly OpCodeHandler otherHandler;

		public OpCodeHandler_Reservednop(OpCodeHandler reservedNopHandler, OpCodeHandler otherHandler) {
			this.reservedNopHandler = reservedNopHandler ?? throw new ArgumentNullException(nameof(reservedNopHandler));
			this.otherHandler = otherHandler ?? throw new ArgumentNullException(nameof(otherHandler));
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			((decoder.options & DecoderOptions.ForceReservedNop) != 0 ? reservedNopHandler : otherHandler).Decode(decoder, ref instruction);
		}
	}

	sealed class OpCodeHandler_Ev_Iz : OpCodeHandlerModRM {
		readonly Code3 codes;
		readonly HandlerFlags flags;

		public OpCodeHandler_Ev_Iz(Code code16, Code code32, Code code64) =>
			codes = new Code3(code16, code32, code64);

		public OpCodeHandler_Ev_Iz(Code code16, Code code32, Code code64, HandlerFlags flags) {
			codes = new Code3(code16, code32, code64);
			this.flags = flags;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			nuint operandSize = (nuint)decoder.state.operandSize;
			unsafe { instruction.InternalSetCodeNoCheck((Code)codes.codes[operandSize]); }
			if (decoder.state.mod < 3) {
				Static.Assert((int)HandlerFlags.Lock == 8 ? 0 : -1);
				Static.Assert((int)StateFlags.AllowLock == 0x00002000 ? 0 : -1);
				decoder.state.zs.flags |= (StateFlags)((uint)(flags & HandlerFlags.Lock) << (13 - 3));
				instruction.Op0Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
			}
			else {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op0Kind = OpKind.Register;
				instruction.Op0Register = ((int)operandSize << 4) + (int)(decoder.state.rm + decoder.state.zs.extraBaseRegisterBase) + Register.AX;
			}
			if ((uint)operandSize == (uint)OpSize.Size32) {
				instruction.Op1Kind = OpKind.Immediate32;
				instruction.Immediate32 = decoder.ReadUInt32();
			}
			else if ((uint)operandSize == (uint)OpSize.Size64) {
				instruction.Op1Kind = OpKind.Immediate32to64;
				instruction.Immediate32 = decoder.ReadUInt32();
			}
			else {
				instruction.Op1Kind = OpKind.Immediate16;
				instruction.InternalImmediate16 = decoder.ReadUInt16();
			}
		}
	}

	sealed class OpCodeHandler_Ev_Ib : OpCodeHandlerModRM {
		readonly Code3 codes;
		readonly HandlerFlags flags;

		public OpCodeHandler_Ev_Ib(Code code16, Code code32, Code code64) =>
			codes = new Code3(code16, code32, code64);

		public OpCodeHandler_Ev_Ib(Code code16, Code code32, Code code64, HandlerFlags flags) {
			codes = new Code3(code16, code32, code64);
			this.flags = flags;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			nuint operandSize = (nuint)decoder.state.operandSize;
			unsafe { instruction.InternalSetCodeNoCheck((Code)codes.codes[operandSize]); }
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op0Kind = OpKind.Register;
				instruction.Op0Register = ((int)operandSize << 4) + (int)(decoder.state.rm + decoder.state.zs.extraBaseRegisterBase) + Register.AX;
			}
			else {
				Static.Assert((int)HandlerFlags.Lock == 8 ? 0 : -1);
				Static.Assert((int)StateFlags.AllowLock == 0x00002000 ? 0 : -1);
				decoder.state.zs.flags |= (StateFlags)((uint)(flags & HandlerFlags.Lock) << (13 - 3));
				instruction.Op0Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
			}
			if ((uint)operandSize == (uint)OpSize.Size32)
				instruction.Op1Kind = OpKind.Immediate8to32;
			else if ((uint)operandSize == (uint)OpSize.Size64)
				instruction.Op1Kind = OpKind.Immediate8to64;
			else
				instruction.Op1Kind = OpKind.Immediate8to16;
			instruction.InternalImmediate8 = decoder.ReadByte();
		}
	}

	sealed class OpCodeHandler_Ev_Ib2 : OpCodeHandlerModRM {
		readonly Code3 codes;
		readonly HandlerFlags flags;

		public OpCodeHandler_Ev_Ib2(Code code16, Code code32, Code code64) =>
			codes = new Code3(code16, code32, code64);

		public OpCodeHandler_Ev_Ib2(Code code16, Code code32, Code code64, HandlerFlags flags) {
			codes = new Code3(code16, code32, code64);
			this.flags = flags;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			nuint operandSize = (nuint)decoder.state.operandSize;
			unsafe { instruction.InternalSetCodeNoCheck((Code)codes.codes[operandSize]); }
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op0Kind = OpKind.Register;
				instruction.Op0Register = ((int)operandSize << 4) + (int)(decoder.state.rm + decoder.state.zs.extraBaseRegisterBase) + Register.AX;
			}
			else {
				Static.Assert((int)HandlerFlags.Lock == 8 ? 0 : -1);
				Static.Assert((int)StateFlags.AllowLock == 0x00002000 ? 0 : -1);
				decoder.state.zs.flags |= (StateFlags)((uint)(flags & HandlerFlags.Lock) << (13 - 3));
				instruction.Op0Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
			}
			instruction.Op1Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadByte();
		}
	}

	sealed class OpCodeHandler_Ev_1 : OpCodeHandlerModRM {
		readonly Code3 codes;

		public OpCodeHandler_Ev_1(Code code16, Code code32, Code code64) =>
			codes = new Code3(code16, code32, code64);

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			nuint operandSize = (nuint)decoder.state.operandSize;
			unsafe { instruction.InternalSetCodeNoCheck((Code)codes.codes[operandSize]); }
			instruction.Op1Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = 1;
			decoder.state.zs.flags |= StateFlags.NoImm;
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op0Kind = OpKind.Register;
				instruction.Op0Register = ((int)operandSize << 4) + (int)(decoder.state.rm + decoder.state.zs.extraBaseRegisterBase) + Register.AX;
			}
			else {
				instruction.Op0Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_Ev_CL : OpCodeHandlerModRM {
		readonly Code3 codes;

		public OpCodeHandler_Ev_CL(Code code16, Code code32, Code code64) =>
			codes = new Code3(code16, code32, code64);

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			nuint operandSize = (nuint)decoder.state.operandSize;
			unsafe { instruction.InternalSetCodeNoCheck((Code)codes.codes[operandSize]); }
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op1Kind = OpKind.Register;
			instruction.Op1Register = Register.CL;
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op0Kind = OpKind.Register;
				instruction.Op0Register = ((int)operandSize << 4) + (int)(decoder.state.rm + decoder.state.zs.extraBaseRegisterBase) + Register.AX;
			}
			else {
				instruction.Op0Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_Ev : OpCodeHandlerModRM {
		readonly Code3 codes;
		readonly HandlerFlags flags;

		public OpCodeHandler_Ev(Code code16, Code code32, Code code64) =>
			codes = new Code3(code16, code32, code64);

		public OpCodeHandler_Ev(Code code16, Code code32, Code code64, HandlerFlags flags) {
			codes = new Code3(code16, code32, code64);
			this.flags = flags;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			nuint operandSize = (nuint)decoder.state.operandSize;
			unsafe { instruction.InternalSetCodeNoCheck((Code)codes.codes[operandSize]); }
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op0Kind = OpKind.Register;
				instruction.Op0Register = ((int)operandSize << 4) + (int)(decoder.state.rm + decoder.state.zs.extraBaseRegisterBase) + Register.AX;
			}
			else {
				Static.Assert((int)HandlerFlags.Lock == 8 ? 0 : -1);
				Static.Assert((int)StateFlags.AllowLock == 0x00002000 ? 0 : -1);
				decoder.state.zs.flags |= (StateFlags)((uint)(flags & HandlerFlags.Lock) << (13 - 3));
				instruction.Op0Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_Rv : OpCodeHandlerModRM {
		readonly Code3 codes;

		public OpCodeHandler_Rv(Code code16, Code code32, Code code64) =>
			codes = new Code3(code16, code32, code64);

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			nuint operandSize = (nuint)decoder.state.operandSize;
			unsafe { instruction.InternalSetCodeNoCheck((Code)codes.codes[operandSize]); }
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = ((int)operandSize << 4) + (int)(decoder.state.rm + decoder.state.zs.extraBaseRegisterBase) + Register.AX;
			Debug.Assert(decoder.state.mod == 3);
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
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			Register baseReg;
			if (decoder.is64bMode) {
				instruction.InternalSetCodeNoCheck(code64);
				baseReg = Register.RAX;
			}
			else {
				instruction.InternalSetCodeNoCheck(code32);
				baseReg = Register.EAX;
			}
			Debug.Assert(decoder.state.mod == 3);
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = (int)(decoder.state.rm + decoder.state.zs.extraBaseRegisterBase) + baseReg;
		}
	}

	sealed class OpCodeHandler_Rq : OpCodeHandlerModRM {
		readonly Code code;

		public OpCodeHandler_Rq(Code code) =>
			this.code = code;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			instruction.InternalSetCodeNoCheck(code);
			Debug.Assert(decoder.state.mod == 3);
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = (int)(decoder.state.rm + decoder.state.zs.extraBaseRegisterBase) + Register.RAX;
		}
	}

	sealed class OpCodeHandler_Ev_REXW : OpCodeHandlerModRM {
		readonly Code code32;
		readonly Code code64;
		readonly uint flags;
		readonly uint disallowReg;
		readonly uint disallowMem;

		public OpCodeHandler_Ev_REXW(Code code32, Code code64, uint flags) {
			this.code32 = code32;
			this.code64 = code64;
			this.flags = flags;
			disallowReg = (flags & 1) != 0 ? 0 : uint.MaxValue;
			disallowMem = (flags & 2) != 0 ? 0 : uint.MaxValue;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			if ((decoder.state.zs.flags & StateFlags.W) != 0)
				instruction.InternalSetCodeNoCheck(code64);
			else
				instruction.InternalSetCodeNoCheck(code32);
			Static.Assert((uint)StateFlags.Has66 != 4 ? 0 : -1);
			if ((((flags & 4) | (uint)(decoder.state.zs.flags & StateFlags.Has66)) & decoder.invalidCheckMask) == (4 | (uint)StateFlags.Has66))
				decoder.SetInvalidInstruction();
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op0Kind = OpKind.Register;
				if ((decoder.state.zs.flags & StateFlags.W) != 0)
					instruction.Op0Register = (int)(decoder.state.rm + decoder.state.zs.extraBaseRegisterBase) + Register.RAX;
				else
					instruction.Op0Register = (int)(decoder.state.rm + decoder.state.zs.extraBaseRegisterBase) + Register.EAX;
				if ((disallowReg & decoder.invalidCheckMask) != 0)
					decoder.SetInvalidInstruction();
			}
			else {
				if ((disallowMem & decoder.invalidCheckMask) != 0)
					decoder.SetInvalidInstruction();
				instruction.Op0Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
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
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			if (decoder.is64bMode) {
				if ((decoder.options & DecoderOptions.AMD) == 0 || decoder.state.operandSize != OpSize.Size16)
					instruction.InternalSetCodeNoCheck(code64);
				else
					instruction.InternalSetCodeNoCheck(code16);
				if (decoder.state.mod < 3) {
					instruction.Op0Kind = OpKind.Memory;
					decoder.ReadOpMem(ref instruction);
				}
				else {
					Static.Assert(OpKind.Register == 0 ? 0 : -1);
					//instruction.Op0Kind = OpKind.Register;
					if ((decoder.options & DecoderOptions.AMD) == 0 || decoder.state.operandSize != OpSize.Size16)
						instruction.Op0Register = (int)(decoder.state.rm + decoder.state.zs.extraBaseRegisterBase) + Register.RAX;
					else
						instruction.Op0Register = (int)(decoder.state.rm + decoder.state.zs.extraBaseRegisterBase) + Register.AX;
				}
			}
			else {
				if (decoder.state.operandSize == OpSize.Size32)
					instruction.InternalSetCodeNoCheck(code32);
				else
					instruction.InternalSetCodeNoCheck(code16);
				if (decoder.state.mod < 3) {
					instruction.Op0Kind = OpKind.Memory;
					decoder.ReadOpMem(ref instruction);
				}
				else {
					Static.Assert(OpKind.Register == 0 ? 0 : -1);
					//instruction.Op0Kind = OpKind.Register;
					if (decoder.state.operandSize == OpSize.Size32)
						instruction.Op0Register = (int)decoder.state.rm + Register.EAX;
					else
						instruction.Op0Register = (int)decoder.state.rm + Register.AX;
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
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			if (decoder.state.operandSize == OpSize.Size64 && (decoder.options & DecoderOptions.AMD) == 0)
				instruction.InternalSetCodeNoCheck(code64);
			else if (decoder.state.operandSize == OpSize.Size16)
				instruction.InternalSetCodeNoCheck(code16);
			else
				instruction.InternalSetCodeNoCheck(code32);
			if (decoder.state.mod == 3)
				decoder.SetInvalidInstruction();
			else {
				instruction.Op0Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_Evw : OpCodeHandlerModRM {
		readonly Code3 codes;

		public OpCodeHandler_Evw(Code code16, Code code32, Code code64) =>
			codes = new Code3(code16, code32, code64);

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			nuint operandSize = (nuint)decoder.state.operandSize;
			unsafe { instruction.InternalSetCodeNoCheck((Code)codes.codes[operandSize]); }
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op0Kind = OpKind.Register;
				instruction.Op0Register = ((int)operandSize << 4) + (int)(decoder.state.rm + decoder.state.zs.extraBaseRegisterBase) + Register.AX;
			}
			else {
				instruction.Op0Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_Ew : OpCodeHandlerModRM {
		readonly Code3 codes;

		public OpCodeHandler_Ew(Code code16, Code code32, Code code64) =>
			codes = new Code3(code16, code32, code64);

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			nuint operandSize = (nuint)decoder.state.operandSize;
			unsafe { instruction.InternalSetCodeNoCheck((Code)codes.codes[operandSize]); }
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op0Kind = OpKind.Register;
				instruction.Op0Register = ((int)operandSize << 4) + (int)(decoder.state.rm + decoder.state.zs.extraBaseRegisterBase) + Register.AX;
			}
			else {
				instruction.Op0Kind = OpKind.Memory;
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
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			if (decoder.is64bMode)
				instruction.InternalSetCodeNoCheck(code64);
			else if (decoder.state.operandSize == OpSize.Size32)
				instruction.InternalSetCodeNoCheck(code32);
			else
				instruction.InternalSetCodeNoCheck(code16);
			Debug.Assert(decoder.state.mod != 3);
			instruction.Op0Kind = OpKind.Memory;
			decoder.ReadOpMem(ref instruction);
		}
	}

	sealed class OpCodeHandler_Gv_Ev : OpCodeHandlerModRM {
		readonly Code3 codes;

		public OpCodeHandler_Gv_Ev(Code code16, Code code32, Code code64) =>
			codes = new Code3(code16, code32, code64);

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			nuint operandSize = (nuint)decoder.state.operandSize;
			unsafe { instruction.InternalSetCodeNoCheck((Code)codes.codes[operandSize]); }
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = ((int)operandSize << 4) + (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase) + Register.AX;
			if (decoder.state.mod < 3) {
				instruction.Op1Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
			}
			else {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op1Kind = OpKind.Register;
				instruction.Op1Register = ((int)operandSize << 4) + (int)(decoder.state.rm + decoder.state.zs.extraBaseRegisterBase) + Register.AX;
			}
		}
	}

	sealed class OpCodeHandler_Gd_Rd : OpCodeHandlerModRM {
		readonly Code code;

		public OpCodeHandler_Gd_Rd(Code code) => this.code = code;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			instruction.InternalSetCodeNoCheck(code);
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase) + Register.EAX;
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op1Kind = OpKind.Register;
				instruction.Op1Register = (int)(decoder.state.rm + decoder.state.zs.extraBaseRegisterBase) + Register.EAX;
			}
			else
				decoder.SetInvalidInstruction();
		}
	}

	sealed class OpCodeHandler_Gv_M_as : OpCodeHandlerModRM {
		readonly Code3 codes;

		public OpCodeHandler_Gv_M_as(Code code16, Code code32, Code code64) =>
			codes = new Code3(code16, code32, code64);

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			nuint addressSize = (nuint)decoder.state.addressSize;
			unsafe { instruction.InternalSetCodeNoCheck((Code)codes.codes[addressSize]); }
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = ((int)addressSize << 4) + (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase) + Register.AX;
			if (decoder.state.mod == 3)
				decoder.SetInvalidInstruction();
			else {
				instruction.Op1Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_Gdq_Ev : OpCodeHandlerModRM {
		readonly Code3 codes;

		public OpCodeHandler_Gdq_Ev(Code code16, Code code32, Code code64) =>
			codes = new Code3(code16, code32, code64);

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			nuint operandSize = (nuint)decoder.state.operandSize;
			unsafe { instruction.InternalSetCodeNoCheck((Code)codes.codes[operandSize]); }
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			if ((uint)operandSize != (uint)OpSize.Size64)
				instruction.Op0Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase) + Register.EAX;
			else
				instruction.Op0Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase) + Register.RAX;
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op1Kind = OpKind.Register;
				instruction.Op1Register = ((int)operandSize << 4) + (int)(decoder.state.rm + decoder.state.zs.extraBaseRegisterBase) + Register.AX;
			}
			else {
				instruction.Op1Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_Gv_Ev3 : OpCodeHandlerModRM {
		readonly Code3 codes;

		public OpCodeHandler_Gv_Ev3(Code code16, Code code32, Code code64) =>
			codes = new Code3(code16, code32, code64);

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			nuint operandSize = (nuint)decoder.state.operandSize;
			unsafe { instruction.InternalSetCodeNoCheck((Code)codes.codes[operandSize]); }
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = ((int)operandSize << 4) + (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase) + Register.AX;
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op1Kind = OpKind.Register;
				instruction.Op1Register = ((int)operandSize << 4) + (int)(decoder.state.rm + decoder.state.zs.extraBaseRegisterBase) + Register.AX;
			}
			else {
				instruction.Op1Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_Gv_Ev2 : OpCodeHandlerModRM {
		readonly Code3 codes;

		public OpCodeHandler_Gv_Ev2(Code code16, Code code32, Code code64) =>
			codes = new Code3(code16, code32, code64);

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			nuint operandSize = (nuint)decoder.state.operandSize;
			unsafe { instruction.InternalSetCodeNoCheck((Code)codes.codes[operandSize]); }
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = ((int)operandSize << 4) + (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase) + Register.AX;
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op1Kind = OpKind.Register;
				uint index = decoder.state.rm + decoder.state.zs.extraBaseRegisterBase;
				if (decoder.state.operandSize != OpSize.Size16)
					instruction.Op1Register = (int)index + Register.EAX;
				else
					instruction.Op1Register = (int)index + Register.AX;
			}
			else {
				instruction.Op1Kind = OpKind.Memory;
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
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			if (decoder.is64bMode) {
				instruction.InternalSetCodeNoCheck(code64);
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op0Kind = OpKind.Register;
				instruction.Op0Register = (int)(decoder.state.rm + decoder.state.zs.extraBaseRegisterBase) + Register.RAX;
			}
			else {
				instruction.InternalSetCodeNoCheck(code32);
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op0Kind = OpKind.Register;
				instruction.Op0Register = (int)(decoder.state.rm + decoder.state.zs.extraBaseRegisterBase) + Register.EAX;
			}
			var extraRegisterBase = decoder.state.zs.extraRegisterBase;
			// LOCK MOV CR0 is supported by some AMD CPUs
			if (baseReg == Register.CR0 && instruction.HasLockPrefix && (decoder.options & DecoderOptions.AMD) != 0) {
				if ((extraRegisterBase & decoder.invalidCheckMask) != 0)
					decoder.SetInvalidInstruction();
				extraRegisterBase = 8;
				instruction.InternalClearHasLockPrefix();
				decoder.state.zs.flags &= ~StateFlags.Lock;
			}
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op1Kind = OpKind.Register;
			var reg = (int)(decoder.state.reg + extraRegisterBase);
			if (decoder.invalidCheckMask != 0) {
				if (baseReg == Register.CR0) {
					if (reg == 1 || (reg != 8 && reg >= 5))
						decoder.SetInvalidInstruction();
				}
				else if (baseReg == Register.DR0) {
					if (reg > 7)
						decoder.SetInvalidInstruction();
				}
				else {
					Debug.Assert(!decoder.is64bMode);
					Debug.Assert(baseReg == Register.TR0);
				}
			}
			instruction.Op1Register = reg + baseReg;
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
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			if (decoder.is64bMode) {
				instruction.InternalSetCodeNoCheck(code64);
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op1Kind = OpKind.Register;
				instruction.Op1Register = (int)(decoder.state.rm + decoder.state.zs.extraBaseRegisterBase) + Register.RAX;
			}
			else {
				instruction.InternalSetCodeNoCheck(code32);
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op1Kind = OpKind.Register;
				instruction.Op1Register = (int)(decoder.state.rm + decoder.state.zs.extraBaseRegisterBase) + Register.EAX;
			}
			var extraRegisterBase = decoder.state.zs.extraRegisterBase;
			// LOCK MOV CR0 is supported by some AMD CPUs
			if (baseReg == Register.CR0 && instruction.HasLockPrefix && (decoder.options & DecoderOptions.AMD) != 0) {
				if ((extraRegisterBase & decoder.invalidCheckMask) != 0)
					decoder.SetInvalidInstruction();
				extraRegisterBase = 8;
				instruction.InternalClearHasLockPrefix();
				decoder.state.zs.flags &= ~StateFlags.Lock;
			}
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			var reg = (int)(decoder.state.reg + extraRegisterBase);
			if (decoder.invalidCheckMask != 0) {
				if (baseReg == Register.CR0) {
					if (reg == 1 || (reg != 8 && reg >= 5))
						decoder.SetInvalidInstruction();
				}
				else if (baseReg == Register.DR0) {
					if (reg > 7)
						decoder.SetInvalidInstruction();
				}
				else {
					Debug.Assert(!decoder.is64bMode);
					Debug.Assert(baseReg == Register.TR0);
				}
			}
			instruction.Op0Register = reg + baseReg;
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
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			decoder.state.zs.flags |= StateFlags.BranchImm8;
			if (decoder.is64bMode) {
				if ((decoder.options & DecoderOptions.AMD) == 0 || decoder.state.operandSize != OpSize.Size16) {
					instruction.InternalSetCodeNoCheck(code64);
					instruction.Op0Kind = OpKind.NearBranch64;
					instruction.NearBranch64 = (ulong)(sbyte)decoder.ReadByte() + decoder.GetCurrentInstructionPointer64();
				}
				else {
					instruction.InternalSetCodeNoCheck(code16);
					instruction.Op0Kind = OpKind.NearBranch16;
					instruction.InternalNearBranch16 = (ushort)((uint)(sbyte)decoder.ReadByte() + decoder.GetCurrentInstructionPointer32());
				}
			}
			else {
				if (decoder.state.operandSize != OpSize.Size16) {
					instruction.InternalSetCodeNoCheck(code32);
					instruction.Op0Kind = OpKind.NearBranch32;
					instruction.NearBranch32 = (uint)(sbyte)decoder.ReadByte() + decoder.GetCurrentInstructionPointer32();
				}
				else {
					instruction.InternalSetCodeNoCheck(code16);
					instruction.Op0Kind = OpKind.NearBranch16;
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
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			decoder.state.zs.flags |= StateFlags.Xbegin;
			if (decoder.is64bMode) {
				if (decoder.state.operandSize == OpSize.Size32) {
					instruction.InternalSetCodeNoCheck(code32);
					instruction.Op0Kind = OpKind.NearBranch64;
					instruction.NearBranch64 = (ulong)(int)decoder.ReadUInt32() + decoder.GetCurrentInstructionPointer64();
				}
				else if (decoder.state.operandSize == OpSize.Size64) {
					instruction.InternalSetCodeNoCheck(code64);
					instruction.Op0Kind = OpKind.NearBranch64;
					instruction.NearBranch64 = (ulong)(int)decoder.ReadUInt32() + decoder.GetCurrentInstructionPointer64();
				}
				else {
					instruction.InternalSetCodeNoCheck(code16);
					instruction.Op0Kind = OpKind.NearBranch64;
					instruction.NearBranch64 = (ulong)(short)decoder.ReadUInt16() + decoder.GetCurrentInstructionPointer64();
				}
			}
			else {
				Debug.Assert(decoder.defaultCodeSize == CodeSize.Code16 || decoder.defaultCodeSize == CodeSize.Code32);
				if (decoder.state.operandSize == OpSize.Size32) {
					instruction.InternalSetCodeNoCheck(code32);
					instruction.Op0Kind = OpKind.NearBranch32;
					instruction.NearBranch32 = decoder.ReadUInt32() + decoder.GetCurrentInstructionPointer32();
				}
				else {
					Debug.Assert(decoder.state.operandSize == OpSize.Size16);
					instruction.InternalSetCodeNoCheck(code16);
					instruction.Op0Kind = OpKind.NearBranch32;
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
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			if (decoder.is64bMode) {
				if ((decoder.options & DecoderOptions.AMD) == 0 || decoder.state.operandSize != OpSize.Size16) {
					instruction.InternalSetCodeNoCheck(code64);
					instruction.Op0Kind = OpKind.NearBranch64;
					instruction.NearBranch64 = (ulong)(int)decoder.ReadUInt32() + decoder.GetCurrentInstructionPointer64();
				}
				else {
					instruction.InternalSetCodeNoCheck(code16);
					instruction.Op0Kind = OpKind.NearBranch16;
					instruction.InternalNearBranch16 = (ushort)(decoder.ReadUInt16() + decoder.GetCurrentInstructionPointer32());
				}
			}
			else {
				if (decoder.state.operandSize != OpSize.Size16) {
					instruction.InternalSetCodeNoCheck(code32);
					instruction.Op0Kind = OpKind.NearBranch32;
					instruction.NearBranch32 = decoder.ReadUInt32() + decoder.GetCurrentInstructionPointer32();
				}
				else {
					instruction.InternalSetCodeNoCheck(code16);
					instruction.Op0Kind = OpKind.NearBranch16;
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
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			decoder.state.zs.flags |= StateFlags.BranchImm8;
			if (decoder.is64bMode) {
				if ((decoder.options & DecoderOptions.AMD) == 0 || decoder.state.operandSize != OpSize.Size16) {
					if (decoder.state.addressSize == OpSize.Size64)
						instruction.InternalSetCodeNoCheck(code64_64);
					else
						instruction.InternalSetCodeNoCheck(code64_32);
					instruction.Op0Kind = OpKind.NearBranch64;
					instruction.NearBranch64 = (ulong)(sbyte)decoder.ReadByte() + decoder.GetCurrentInstructionPointer64();
				}
				else {
					if (decoder.state.addressSize == OpSize.Size64)
						instruction.InternalSetCodeNoCheck(code16_64);
					else
						instruction.InternalSetCodeNoCheck(code16_32);
					instruction.Op0Kind = OpKind.NearBranch16;
					instruction.InternalNearBranch16 = (ushort)((uint)(sbyte)decoder.ReadByte() + decoder.GetCurrentInstructionPointer32());
				}
			}
			else {
				if (decoder.state.operandSize == OpSize.Size32) {
					if (decoder.state.addressSize == OpSize.Size32)
						instruction.InternalSetCodeNoCheck(code32_32);
					else
						instruction.InternalSetCodeNoCheck(code32_16);
					instruction.Op0Kind = OpKind.NearBranch32;
					instruction.NearBranch32 = (uint)(sbyte)decoder.ReadByte() + decoder.GetCurrentInstructionPointer32();
				}
				else {
					if (decoder.state.addressSize == OpSize.Size32)
						instruction.InternalSetCodeNoCheck(code16_32);
					else
						instruction.InternalSetCodeNoCheck(code16_16);
					instruction.Op0Kind = OpKind.NearBranch16;
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
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			Debug.Assert(!decoder.is64bMode);
			if (decoder.state.operandSize != OpSize.Size16) {
				instruction.InternalSetCodeNoCheck(code32);
				instruction.Op0Kind = OpKind.NearBranch32;
				instruction.NearBranch32 = decoder.ReadUInt32();
			}
			else {
				instruction.InternalSetCodeNoCheck(code16);
				instruction.Op0Kind = OpKind.NearBranch16;
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
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			if (decoder.is64bMode) {
				if (decoder.state.operandSize != OpSize.Size16)
					instruction.InternalSetCodeNoCheck(code64);
				else
					instruction.InternalSetCodeNoCheck(code16);
			}
			else {
				if (decoder.state.operandSize == OpSize.Size32)
					instruction.InternalSetCodeNoCheck(code32);
				else
					instruction.InternalSetCodeNoCheck(code16);
			}
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = reg;
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
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			if (decoder.is64bMode) {
				if (decoder.state.operandSize != OpSize.Size16)
					instruction.InternalSetCodeNoCheck(code64);
				else
					instruction.InternalSetCodeNoCheck(code16);
			}
			else {
				if (decoder.state.operandSize == OpSize.Size32)
					instruction.InternalSetCodeNoCheck(code32);
				else
					instruction.InternalSetCodeNoCheck(code16);
			}
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op0Kind = OpKind.Register;
				uint index = decoder.state.rm + decoder.state.zs.extraBaseRegisterBase;
				if (decoder.is64bMode) {
					if (decoder.state.operandSize != OpSize.Size16)
						instruction.Op0Register = (int)index + Register.RAX;
					else
						instruction.Op0Register = (int)index + Register.AX;
				}
				else {
					if (decoder.state.operandSize == OpSize.Size32)
						instruction.Op0Register = (int)index + Register.EAX;
					else
						instruction.Op0Register = (int)index + Register.AX;
				}
			}
			else {
				instruction.Op0Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_Ev_Gv : OpCodeHandlerModRM {
		readonly Code3 codes;
		readonly HandlerFlags flags;

		public OpCodeHandler_Ev_Gv(Code code16, Code code32, Code code64) =>
			codes = new Code3(code16, code32, code64);

		public OpCodeHandler_Ev_Gv(Code code16, Code code32, Code code64, HandlerFlags flags) {
			codes = new Code3(code16, code32, code64);
			this.flags = flags;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			nuint operandSize = (nuint)decoder.state.operandSize;
			unsafe { instruction.InternalSetCodeNoCheck((Code)codes.codes[operandSize]); }
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op1Kind = OpKind.Register;
			instruction.Op1Register = ((int)operandSize << 4) + (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase) + Register.AX;
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op0Kind = OpKind.Register;
				instruction.Op0Register = ((int)operandSize << 4) + (int)(decoder.state.rm + decoder.state.zs.extraBaseRegisterBase) + Register.AX;
			}
			else {
				Static.Assert((int)HandlerFlags.Lock == 8 ? 0 : -1);
				Static.Assert((int)StateFlags.AllowLock == 0x00002000 ? 0 : -1);
				decoder.state.zs.flags |= (StateFlags)((uint)(flags & HandlerFlags.Lock) << (13 - 3));
				instruction.Op0Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
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
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			Register baseReg;
			if (decoder.is64bMode) {
				instruction.InternalSetCodeNoCheck(code64);
				baseReg = Register.RAX;
			}
			else {
				instruction.InternalSetCodeNoCheck(code32);
				baseReg = Register.EAX;
			}
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op1Kind = OpKind.Register;
			instruction.Op1Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase) + baseReg;
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op0Kind = OpKind.Register;
				instruction.Op0Register = (int)(decoder.state.rm + decoder.state.zs.extraBaseRegisterBase) + baseReg;
			}
			else {
				instruction.Op0Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_Ev_Gv_Ib : OpCodeHandlerModRM {
		readonly Code3 codes;

		public OpCodeHandler_Ev_Gv_Ib(Code code16, Code code32, Code code64) =>
			codes = new Code3(code16, code32, code64);

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			nuint operandSize = (nuint)decoder.state.operandSize;
			unsafe { instruction.InternalSetCodeNoCheck((Code)codes.codes[operandSize]); }
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op1Kind = OpKind.Register;
			instruction.Op1Register = ((int)operandSize << 4) + (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase) + Register.AX;
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op0Kind = OpKind.Register;
				instruction.Op0Register = ((int)operandSize << 4) + (int)(decoder.state.rm + decoder.state.zs.extraBaseRegisterBase) + Register.AX;
			}
			else {
				instruction.Op0Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
			}
			instruction.Op2Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadByte();
		}
	}

	sealed class OpCodeHandler_Ev_Gv_CL : OpCodeHandlerModRM {
		readonly Code3 codes;

		public OpCodeHandler_Ev_Gv_CL(Code code16, Code code32, Code code64) =>
			codes = new Code3(code16, code32, code64);

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op2Kind = OpKind.Register;
			instruction.Op2Register = Register.CL;
			nuint operandSize = (nuint)decoder.state.operandSize;
			unsafe { instruction.InternalSetCodeNoCheck((Code)codes.codes[operandSize]); }
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op1Kind = OpKind.Register;
			instruction.Op1Register = ((int)operandSize << 4) + (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase) + Register.AX;
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op0Kind = OpKind.Register;
				instruction.Op0Register = ((int)operandSize << 4) + (int)(decoder.state.rm + decoder.state.zs.extraBaseRegisterBase) + Register.AX;
			}
			else {
				instruction.Op0Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
			}
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
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			if (decoder.state.operandSize == OpSize.Size64 && (decoder.options & DecoderOptions.AMD) == 0) {
				instruction.InternalSetCodeNoCheck(code64);
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op0Kind = OpKind.Register;
				instruction.Op0Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase) + Register.RAX;
			}
			else if (decoder.state.operandSize == OpSize.Size16) {
				instruction.InternalSetCodeNoCheck(code16);
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op0Kind = OpKind.Register;
				instruction.Op0Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase) + Register.AX;
			}
			else {
				instruction.InternalSetCodeNoCheck(code32);
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op0Kind = OpKind.Register;
				instruction.Op0Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase) + Register.EAX;
			}
			if (decoder.state.mod == 3)
				decoder.SetInvalidInstruction();
			else {
				instruction.Op1Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_Gv_Eb : OpCodeHandlerModRM {
		readonly Code3 codes;

		public OpCodeHandler_Gv_Eb(Code code16, Code code32, Code code64) =>
			codes = new Code3(code16, code32, code64);

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			nuint operandSize = (nuint)decoder.state.operandSize;
			unsafe { instruction.InternalSetCodeNoCheck((Code)codes.codes[operandSize]); }
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = ((int)operandSize << 4) + (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase) + Register.AX;
			if (decoder.state.mod == 3) {
				uint index = decoder.state.rm + decoder.state.zs.extraBaseRegisterBase;
				if ((decoder.state.zs.flags & StateFlags.HasRex) != 0 && index >= 4)
					index += 4;
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op1Kind = OpKind.Register;
				instruction.Op1Register = (int)index + Register.AL;
			}
			else {
				instruction.Op1Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_Gv_Ew : OpCodeHandlerModRM {
		readonly Code3 codes;

		public OpCodeHandler_Gv_Ew(Code code16, Code code32, Code code64) =>
			codes = new Code3(code16, code32, code64);

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			nuint operandSize = (nuint)decoder.state.operandSize;
			unsafe { instruction.InternalSetCodeNoCheck((Code)codes.codes[operandSize]); }
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = ((int)operandSize << 4) + (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase) + Register.AX;
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op1Kind = OpKind.Register;
				instruction.Op1Register = (int)(decoder.state.rm + decoder.state.zs.extraBaseRegisterBase) + Register.AX;
			}
			else {
				instruction.Op1Kind = OpKind.Memory;
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
			if (decoder.is64bMode) {
				if (decoder.state.operandSize != OpSize.Size16)
					instruction.InternalSetCodeNoCheck(code64);
				else
					instruction.InternalSetCodeNoCheck(code16);
			}
			else {
				if (decoder.state.operandSize == OpSize.Size32)
					instruction.InternalSetCodeNoCheck(code32);
				else
					instruction.InternalSetCodeNoCheck(code16);
			}
		}
	}

	sealed class OpCodeHandler_Simple2 : OpCodeHandler {
		readonly Code3 codes;

		public OpCodeHandler_Simple2(Code code16, Code code32, Code code64) =>
			codes = new Code3(code16, code32, code64);

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			nuint operandSize = (nuint)decoder.state.operandSize;
			unsafe { instruction.InternalSetCodeNoCheck((Code)codes.codes[operandSize]); }
		}
	}

	sealed class OpCodeHandler_Simple2Iw : OpCodeHandler {
		readonly Code3 codes;

		public OpCodeHandler_Simple2Iw(Code code16, Code code32, Code code64) =>
			codes = new Code3(code16, code32, code64);

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			nuint operandSize = (nuint)decoder.state.operandSize;
			unsafe { instruction.InternalSetCodeNoCheck((Code)codes.codes[operandSize]); }
			instruction.Op0Kind = OpKind.Immediate16;
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
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			if (decoder.is64bMode) {
				if (decoder.state.operandSize != OpSize.Size16)
					instruction.InternalSetCodeNoCheck(code64);
				else
					instruction.InternalSetCodeNoCheck(code16);
			}
			else {
				if (decoder.state.operandSize == OpSize.Size32)
					instruction.InternalSetCodeNoCheck(code32);
				else
					instruction.InternalSetCodeNoCheck(code16);
			}
		}
	}

	sealed class OpCodeHandler_Simple5 : OpCodeHandler {
		readonly Code3 codes;

		public OpCodeHandler_Simple5(Code code16, Code code32, Code code64) =>
			codes = new Code3(code16, code32, code64);

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			nuint addressSize = (nuint)decoder.state.addressSize;
			unsafe { instruction.InternalSetCodeNoCheck((Code)codes.codes[addressSize]); }
		}
	}

	sealed class OpCodeHandler_Simple5_a32 : OpCodeHandler {
		readonly Code3 codes;

		public OpCodeHandler_Simple5_a32(Code code16, Code code32, Code code64) =>
			codes = new Code3(code16, code32, code64);

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			if (decoder.state.addressSize != OpSize.Size32 && decoder.invalidCheckMask != 0)
				decoder.SetInvalidInstruction();
			nuint addressSize = (nuint)decoder.state.addressSize;
			unsafe { instruction.InternalSetCodeNoCheck((Code)codes.codes[addressSize]); }
		}
	}

	sealed class OpCodeHandler_Simple5_ModRM_as : OpCodeHandlerModRM {
		readonly Code3 codes;

		public OpCodeHandler_Simple5_ModRM_as(Code code16, Code code32, Code code64) =>
			codes = new Code3(code16, code32, code64);

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			nuint addressSize = (nuint)decoder.state.addressSize;
			unsafe { instruction.InternalSetCodeNoCheck((Code)codes.codes[addressSize]); }
			instruction.Op0Register = ((int)addressSize << 4) + (int)(decoder.state.rm + decoder.state.zs.extraBaseRegisterBase) + Register.AX;
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
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			if ((decoder.state.zs.flags & StateFlags.W) != 0)
				instruction.InternalSetCodeNoCheck(code64);
			else
				instruction.InternalSetCodeNoCheck(code32);
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
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			if (decoder.is64bMode) {
				if (decoder.state.operandSize != OpSize.Size16) {
					instruction.InternalSetCodeNoCheck(code64);
					Static.Assert(OpKind.Register == 0 ? 0 : -1);
					//instruction.Op0Kind = OpKind.Register;
					instruction.Op0Register = index + (int)decoder.state.zs.extraBaseRegisterBase + Register.RAX;
				}
				else {
					instruction.InternalSetCodeNoCheck(code16);
					Static.Assert(OpKind.Register == 0 ? 0 : -1);
					//instruction.Op0Kind = OpKind.Register;
					instruction.Op0Register = index + (int)decoder.state.zs.extraBaseRegisterBase + Register.AX;
				}
			}
			else {
				if (decoder.state.operandSize == OpSize.Size32) {
					instruction.InternalSetCodeNoCheck(code32);
					Static.Assert(OpKind.Register == 0 ? 0 : -1);
					//instruction.Op0Kind = OpKind.Register;
					instruction.Op0Register = index + (int)decoder.state.zs.extraBaseRegisterBase + Register.EAX;
				}
				else {
					instruction.InternalSetCodeNoCheck(code16);
					Static.Assert(OpKind.Register == 0 ? 0 : -1);
					//instruction.Op0Kind = OpKind.Register;
					instruction.Op0Register = index + (int)decoder.state.zs.extraBaseRegisterBase + Register.AX;
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
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			Static.Assert((int)OpSize.Size16 == 0 ? 0 : -1);
			Static.Assert((int)OpSize.Size32 == 1 ? 0 : -1);
			Static.Assert((int)OpSize.Size64 == 2 ? 0 : -1);
			int sizeIndex = (int)decoder.state.operandSize;

			instruction.InternalSetCodeNoCheck(sizeIndex + code);
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			Static.Assert(Register.AX + 16 == Register.EAX ? 0 : -1);
			Static.Assert(Register.AX + 32 == Register.RAX ? 0 : -1);
			instruction.Op0Register = sizeIndex * 16 + index + (int)decoder.state.zs.extraBaseRegisterBase + Register.AX;
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
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);

			if (index == 0 && decoder.state.zs.mandatoryPrefix == MandatoryPrefixByte.PF3 && (decoder.options & DecoderOptions.NoPause) == 0) {
				decoder.ClearMandatoryPrefixF3(ref instruction);
				instruction.InternalSetCodeNoCheck(Code.Pause);
			}
			else {
				Static.Assert((int)OpSize.Size16 == 0 ? 0 : -1);
				Static.Assert((int)OpSize.Size32 == 1 ? 0 : -1);
				Static.Assert((int)OpSize.Size64 == 2 ? 0 : -1);
				int sizeIndex = (int)decoder.state.operandSize;
				int codeIndex = index + (int)decoder.state.zs.extraBaseRegisterBase;

				instruction.InternalSetCodeNoCheck(codes[sizeIndex * 16 + codeIndex]);
				if (codeIndex != 0) {
					Static.Assert(Register.AX + 16 == Register.EAX ? 0 : -1);
					Static.Assert(Register.AX + 32 == Register.RAX ? 0 : -1);
					var reg = sizeIndex * 16 + codeIndex + Register.AX;
					Static.Assert(OpKind.Register == 0 ? 0 : -1);
					//instruction.Op0Kind = OpKind.Register;
					instruction.Op0Register = reg;
					Static.Assert(OpKind.Register == 0 ? 0 : -1);
					//instruction.Op1Kind = OpKind.Register;
					instruction.Op1Register = sizeIndex * 16 + Register.AX;
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
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			if (decoder.state.operandSize == OpSize.Size32) {
				instruction.InternalSetCodeNoCheck(code32);
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op0Kind = OpKind.Register;
				instruction.Op0Register = Register.EAX;
				instruction.Op1Kind = OpKind.Immediate32;
				instruction.Immediate32 = decoder.ReadUInt32();
			}
			else if (decoder.state.operandSize == OpSize.Size64) {
				instruction.InternalSetCodeNoCheck(code64);
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op0Kind = OpKind.Register;
				instruction.Op0Register = Register.RAX;
				instruction.Op1Kind = OpKind.Immediate32to64;
				instruction.Immediate32 = decoder.ReadUInt32();
			}
			else {
				instruction.InternalSetCodeNoCheck(code16);
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op0Kind = OpKind.Register;
				instruction.Op0Register = Register.AX;
				instruction.Op1Kind = OpKind.Immediate16;
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
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			Register register;
			if ((decoder.state.zs.flags & StateFlags.HasRex) != 0)
				register = withRexPrefix[index + (int)decoder.state.zs.extraBaseRegisterBase];
			else
				register = index + Register.AL;
			instruction.InternalSetCodeNoCheck(Code.Mov_r8_imm8);
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = register;
			instruction.Op1Kind = OpKind.Immediate8;
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
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			if (decoder.state.operandSize == OpSize.Size32) {
				instruction.InternalSetCodeNoCheck(Code.Mov_r32_imm32);
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op0Kind = OpKind.Register;
				instruction.Op0Register = index + (int)decoder.state.zs.extraBaseRegisterBase + Register.EAX;
				instruction.Op1Kind = OpKind.Immediate32;
				instruction.Immediate32 = decoder.ReadUInt32();
			}
			else if (decoder.state.operandSize == OpSize.Size64) {
				instruction.InternalSetCodeNoCheck(Code.Mov_r64_imm64);
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op0Kind = OpKind.Register;
				instruction.Op0Register = index + (int)decoder.state.zs.extraBaseRegisterBase + Register.RAX;
				instruction.Op1Kind = OpKind.Immediate64;
				instruction.InternalImmediate64_lo = decoder.ReadUInt32();
				instruction.InternalImmediate64_hi = decoder.ReadUInt32();
			}
			else {
				instruction.InternalSetCodeNoCheck(Code.Mov_r16_imm16);
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op0Kind = OpKind.Register;
				instruction.Op0Register = index + (int)decoder.state.zs.extraBaseRegisterBase + Register.AX;
				instruction.Op1Kind = OpKind.Immediate16;
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
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			if (decoder.is64bMode) {
				if (decoder.state.operandSize != OpSize.Size16) {
					instruction.InternalSetCodeNoCheck(code64);
					instruction.Op0Kind = OpKind.Immediate8to64;
					instruction.InternalImmediate8 = decoder.ReadByte();
				}
				else {
					instruction.InternalSetCodeNoCheck(code16);
					instruction.Op0Kind = OpKind.Immediate8to16;
					instruction.InternalImmediate8 = decoder.ReadByte();
				}
			}
			else {
				if (decoder.state.operandSize == OpSize.Size32) {
					instruction.InternalSetCodeNoCheck(code32);
					instruction.Op0Kind = OpKind.Immediate8to32;
					instruction.InternalImmediate8 = decoder.ReadByte();
				}
				else {
					instruction.InternalSetCodeNoCheck(code16);
					instruction.Op0Kind = OpKind.Immediate8to16;
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
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			if (decoder.is64bMode) {
				if (decoder.state.operandSize != OpSize.Size16) {
					instruction.InternalSetCodeNoCheck(code64);
					instruction.Op0Kind = OpKind.Immediate32to64;
					instruction.Immediate32 = decoder.ReadUInt32();
				}
				else {
					instruction.InternalSetCodeNoCheck(code16);
					instruction.Op0Kind = OpKind.Immediate16;
					instruction.InternalImmediate16 = decoder.ReadUInt16();
				}
			}
			else {
				if (decoder.state.operandSize == OpSize.Size32) {
					instruction.InternalSetCodeNoCheck(code32);
					instruction.Op0Kind = OpKind.Immediate32;
					instruction.Immediate32 = decoder.ReadUInt32();
				}
				else {
					instruction.InternalSetCodeNoCheck(code16);
					instruction.Op0Kind = OpKind.Immediate16;
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
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			if (decoder.state.operandSize != OpSize.Size16) {
				instruction.InternalSetCodeNoCheck(code32);
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op0Kind = OpKind.Register;
				instruction.Op0Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase) + Register.EAX;
			}
			else {
				instruction.InternalSetCodeNoCheck(code16);
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op0Kind = OpKind.Register;
				instruction.Op0Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase) + Register.AX;
			}
			Debug.Assert(decoder.state.mod != 3);
			instruction.Op1Kind = OpKind.Memory;
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
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			Register baseReg;
			if (decoder.state.operandSize != OpSize.Size16) {
				instruction.InternalSetCodeNoCheck(code32);
				instruction.Op1Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase) + Register.EAX;
				baseReg = Register.EAX;
			}
			else {
				instruction.InternalSetCodeNoCheck(code16);
				instruction.Op1Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase) + Register.AX;
				baseReg = Register.AX;
			}
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op1Kind = OpKind.Register;
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op0Kind = OpKind.Register;
				instruction.Op0Register = (int)(decoder.state.rm + decoder.state.zs.extraBaseRegisterBase) + baseReg;
			}
			else {
				instruction.Op0Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_Gv_Ev_Ib : OpCodeHandlerModRM {
		readonly Code3 codes;

		public OpCodeHandler_Gv_Ev_Ib(Code code16, Code code32, Code code64) =>
			codes = new Code3(code16, code32, code64);

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			nuint operandSize = (nuint)decoder.state.operandSize;
			unsafe { instruction.InternalSetCodeNoCheck((Code)codes.codes[operandSize]); }
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = ((int)operandSize << 4) + (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase) + Register.AX;
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op1Kind = OpKind.Register;
				instruction.Op1Register = ((int)operandSize << 4) + (int)(decoder.state.rm + decoder.state.zs.extraBaseRegisterBase) + Register.AX;
			}
			else {
				instruction.Op1Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
			}
			if ((uint)operandSize == (uint)OpSize.Size32) {
				instruction.Op2Kind = OpKind.Immediate8to32;
				instruction.InternalImmediate8 = decoder.ReadByte();
			}
			else if ((uint)operandSize == (uint)OpSize.Size64) {
				instruction.Op2Kind = OpKind.Immediate8to64;
				instruction.InternalImmediate8 = decoder.ReadByte();
			}
			else {
				instruction.Op2Kind = OpKind.Immediate8to16;
				instruction.InternalImmediate8 = decoder.ReadByte();
			}
		}
	}

	sealed class OpCodeHandler_Gv_Ev_Ib_REX : OpCodeHandlerModRM {
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_Gv_Ev_Ib_REX(Code code32, Code code64) {
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			if ((decoder.state.zs.flags & StateFlags.W) != 0) {
				instruction.InternalSetCodeNoCheck(code64);
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op0Kind = OpKind.Register;
				instruction.Op0Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase) + Register.RAX;
			}
			else {
				instruction.InternalSetCodeNoCheck(code32);
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op0Kind = OpKind.Register;
				instruction.Op0Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase) + Register.EAX;
			}
			Debug.Assert(decoder.state.mod == 3);
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op1Kind = OpKind.Register;
			instruction.Op1Register = (int)(decoder.state.rm + decoder.state.zs.extraBaseRegisterBase) + Register.XMM0;
			instruction.Op2Kind = OpKind.Immediate8;
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
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			Register baseReg;
			if (decoder.is64bMode) {
				instruction.InternalSetCodeNoCheck(code64);
				baseReg = Register.RAX;
			}
			else {
				instruction.InternalSetCodeNoCheck(code32);
				baseReg = Register.EAX;
			}
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase) + baseReg;
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op1Kind = OpKind.Register;
				instruction.Op1Register = (int)(decoder.state.rm + decoder.state.zs.extraBaseRegisterBase) + baseReg;
				if ((disallowReg & decoder.invalidCheckMask) != 0)
					decoder.SetInvalidInstruction();
			}
			else {
				if ((disallowMem & decoder.invalidCheckMask) != 0)
					decoder.SetInvalidInstruction();
				instruction.Op1Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_Gv_Ev_Iz : OpCodeHandlerModRM {
		readonly Code3 codes;

		public OpCodeHandler_Gv_Ev_Iz(Code code16, Code code32, Code code64) =>
			codes = new Code3(code16, code32, code64);

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			nuint operandSize = (nuint)decoder.state.operandSize;
			unsafe { instruction.InternalSetCodeNoCheck((Code)codes.codes[operandSize]); }
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = ((int)operandSize << 4) + (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase) + Register.AX;
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op1Kind = OpKind.Register;
				instruction.Op1Register = ((int)operandSize << 4) + (int)(decoder.state.rm + decoder.state.zs.extraBaseRegisterBase) + Register.AX;
			}
			else {
				instruction.Op1Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
			}
			if ((uint)operandSize == (uint)OpSize.Size32) {
				instruction.Op2Kind = OpKind.Immediate32;
				instruction.Immediate32 = decoder.ReadUInt32();
			}
			else if ((uint)operandSize == (uint)OpSize.Size64) {
				instruction.Op2Kind = OpKind.Immediate32to64;
				instruction.Immediate32 = decoder.ReadUInt32();
			}
			else {
				instruction.Op2Kind = OpKind.Immediate16;
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
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			instruction.InternalSetCodeNoCheck(code);
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op1Kind = OpKind.Register;
			instruction.Op1Register = reg;
			if (decoder.state.addressSize == OpSize.Size64)
				instruction.Op0Kind = OpKind.MemoryESRDI;
			else if (decoder.state.addressSize == OpSize.Size32)
				instruction.Op0Kind = OpKind.MemoryESEDI;
			else
				instruction.Op0Kind = OpKind.MemoryESDI;
		}
	}

	sealed class OpCodeHandler_Yv_Reg : OpCodeHandler {
		readonly Code3 codes;

		public OpCodeHandler_Yv_Reg(Code code16, Code code32, Code code64) =>
			codes = new Code3(code16, code32, code64);

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			nuint operandSize = (nuint)decoder.state.operandSize;
			unsafe { instruction.InternalSetCodeNoCheck((Code)codes.codes[operandSize]); }
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op1Kind = OpKind.Register;
			instruction.Op1Register = ((int)operandSize << 4) + Register.AX;
			if (decoder.state.addressSize == OpSize.Size64)
				instruction.Op0Kind = OpKind.MemoryESRDI;
			else if (decoder.state.addressSize == OpSize.Size32)
				instruction.Op0Kind = OpKind.MemoryESEDI;
			else
				instruction.Op0Kind = OpKind.MemoryESDI;
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
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			if (decoder.state.operandSize != OpSize.Size16) {
				instruction.InternalSetCodeNoCheck(code32);
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op1Kind = OpKind.Register;
				instruction.Op1Register = Register.DX;
			}
			else {
				instruction.InternalSetCodeNoCheck(code16);
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op1Kind = OpKind.Register;
				instruction.Op1Register = Register.DX;
			}
			if (decoder.state.addressSize == OpSize.Size64)
				instruction.Op0Kind = OpKind.MemoryESRDI;
			else if (decoder.state.addressSize == OpSize.Size32)
				instruction.Op0Kind = OpKind.MemoryESEDI;
			else
				instruction.Op0Kind = OpKind.MemoryESDI;
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
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			instruction.InternalSetCodeNoCheck(code);
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = reg;
			if (decoder.state.addressSize == OpSize.Size64)
				instruction.Op1Kind = OpKind.MemorySegRSI;
			else if (decoder.state.addressSize == OpSize.Size32)
				instruction.Op1Kind = OpKind.MemorySegESI;
			else
				instruction.Op1Kind = OpKind.MemorySegSI;
		}
	}

	sealed class OpCodeHandler_Reg_Xv : OpCodeHandler {
		readonly Code3 codes;

		public OpCodeHandler_Reg_Xv(Code code16, Code code32, Code code64) =>
			codes = new Code3(code16, code32, code64);

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			nuint operandSize = (nuint)decoder.state.operandSize;
			unsafe { instruction.InternalSetCodeNoCheck((Code)codes.codes[operandSize]); }
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = ((int)operandSize << 4) + Register.AX;
			if (decoder.state.addressSize == OpSize.Size64)
				instruction.Op1Kind = OpKind.MemorySegRSI;
			else if (decoder.state.addressSize == OpSize.Size32)
				instruction.Op1Kind = OpKind.MemorySegESI;
			else
				instruction.Op1Kind = OpKind.MemorySegSI;
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
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			if (decoder.state.operandSize != OpSize.Size16) {
				instruction.InternalSetCodeNoCheck(code32);
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op0Kind = OpKind.Register;
				instruction.Op0Register = Register.DX;
			}
			else {
				instruction.InternalSetCodeNoCheck(code16);
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op0Kind = OpKind.Register;
				instruction.Op0Register = Register.DX;
			}
			if (decoder.state.addressSize == OpSize.Size64)
				instruction.Op1Kind = OpKind.MemorySegRSI;
			else if (decoder.state.addressSize == OpSize.Size32)
				instruction.Op1Kind = OpKind.MemorySegESI;
			else
				instruction.Op1Kind = OpKind.MemorySegSI;
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
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			instruction.InternalSetCodeNoCheck(code);
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = reg;
			if (decoder.state.addressSize == OpSize.Size64)
				instruction.Op1Kind = OpKind.MemoryESRDI;
			else if (decoder.state.addressSize == OpSize.Size32)
				instruction.Op1Kind = OpKind.MemoryESEDI;
			else
				instruction.Op1Kind = OpKind.MemoryESDI;
		}
	}

	sealed class OpCodeHandler_Reg_Yv : OpCodeHandler {
		readonly Code3 codes;

		public OpCodeHandler_Reg_Yv(Code code16, Code code32, Code code64) =>
			codes = new Code3(code16, code32, code64);

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			nuint operandSize = (nuint)decoder.state.operandSize;
			unsafe { instruction.InternalSetCodeNoCheck((Code)codes.codes[operandSize]); }
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = ((int)operandSize << 4) + Register.AX;
			if (decoder.state.addressSize == OpSize.Size64)
				instruction.Op1Kind = OpKind.MemoryESRDI;
			else if (decoder.state.addressSize == OpSize.Size32)
				instruction.Op1Kind = OpKind.MemoryESEDI;
			else
				instruction.Op1Kind = OpKind.MemoryESDI;
		}
	}

	sealed class OpCodeHandler_Yb_Xb : OpCodeHandler {
		readonly Code code;

		public OpCodeHandler_Yb_Xb(Code code) => this.code = code;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			instruction.InternalSetCodeNoCheck(code);
			if (decoder.state.addressSize == OpSize.Size64) {
				instruction.Op0Kind = OpKind.MemoryESRDI;
				instruction.Op1Kind = OpKind.MemorySegRSI;
			}
			else if (decoder.state.addressSize == OpSize.Size32) {
				instruction.Op0Kind = OpKind.MemoryESEDI;
				instruction.Op1Kind = OpKind.MemorySegESI;
			}
			else {
				instruction.Op0Kind = OpKind.MemoryESDI;
				instruction.Op1Kind = OpKind.MemorySegSI;
			}
		}
	}

	sealed class OpCodeHandler_Yv_Xv : OpCodeHandler {
		readonly Code3 codes;

		public OpCodeHandler_Yv_Xv(Code code16, Code code32, Code code64) =>
			codes = new Code3(code16, code32, code64);

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			nuint operandSize = (nuint)decoder.state.operandSize;
			unsafe { instruction.InternalSetCodeNoCheck((Code)codes.codes[operandSize]); }
			if (decoder.state.addressSize == OpSize.Size64) {
				instruction.Op0Kind = OpKind.MemoryESRDI;
				instruction.Op1Kind = OpKind.MemorySegRSI;
			}
			else if (decoder.state.addressSize == OpSize.Size32) {
				instruction.Op0Kind = OpKind.MemoryESEDI;
				instruction.Op1Kind = OpKind.MemorySegESI;
			}
			else {
				instruction.Op0Kind = OpKind.MemoryESDI;
				instruction.Op1Kind = OpKind.MemorySegSI;
			}
		}
	}

	sealed class OpCodeHandler_Xb_Yb : OpCodeHandler {
		readonly Code code;

		public OpCodeHandler_Xb_Yb(Code code) => this.code = code;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			instruction.InternalSetCodeNoCheck(code);
			if (decoder.state.addressSize == OpSize.Size64) {
				instruction.Op0Kind = OpKind.MemorySegRSI;
				instruction.Op1Kind = OpKind.MemoryESRDI;
			}
			else if (decoder.state.addressSize == OpSize.Size32) {
				instruction.Op0Kind = OpKind.MemorySegESI;
				instruction.Op1Kind = OpKind.MemoryESEDI;
			}
			else {
				instruction.Op0Kind = OpKind.MemorySegSI;
				instruction.Op1Kind = OpKind.MemoryESDI;
			}
		}
	}

	sealed class OpCodeHandler_Xv_Yv : OpCodeHandler {
		readonly Code3 codes;

		public OpCodeHandler_Xv_Yv(Code code16, Code code32, Code code64) =>
			codes = new Code3(code16, code32, code64);

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			nuint operandSize = (nuint)decoder.state.operandSize;
			unsafe { instruction.InternalSetCodeNoCheck((Code)codes.codes[operandSize]); }
			if (decoder.state.addressSize == OpSize.Size64) {
				instruction.Op0Kind = OpKind.MemorySegRSI;
				instruction.Op1Kind = OpKind.MemoryESRDI;
			}
			else if (decoder.state.addressSize == OpSize.Size32) {
				instruction.Op0Kind = OpKind.MemorySegESI;
				instruction.Op1Kind = OpKind.MemoryESEDI;
			}
			else {
				instruction.Op0Kind = OpKind.MemorySegSI;
				instruction.Op1Kind = OpKind.MemoryESDI;
			}
		}
	}

	sealed class OpCodeHandler_Ev_Sw : OpCodeHandlerModRM {
		readonly Code3 codes;

		public OpCodeHandler_Ev_Sw(Code code16, Code code32, Code code64) =>
			codes = new Code3(code16, code32, code64);

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op1Kind = OpKind.Register;
			instruction.Op1Register = decoder.ReadOpSegReg();
			nuint operandSize = (nuint)decoder.state.operandSize;
			unsafe { instruction.InternalSetCodeNoCheck((Code)codes.codes[operandSize]); }
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op0Kind = OpKind.Register;
				instruction.Op0Register = ((int)operandSize << 4) + (int)(decoder.state.rm + decoder.state.zs.extraBaseRegisterBase) + Register.AX;
			}
			else {
				instruction.Op0Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_M_Sw : OpCodeHandlerModRM {
		readonly Code code;

		public OpCodeHandler_M_Sw(Code code) => this.code = code;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			instruction.InternalSetCodeNoCheck(code);
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op1Kind = OpKind.Register;
			instruction.Op1Register = decoder.ReadOpSegReg();
			if (decoder.state.mod == 3)
				decoder.SetInvalidInstruction();
			else {
				instruction.Op0Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_Gv_M : OpCodeHandlerModRM {
		readonly Code3 codes;

		public OpCodeHandler_Gv_M(Code code16, Code code32, Code code64) =>
			codes = new Code3(code16, code32, code64);

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			nuint operandSize = (nuint)decoder.state.operandSize;
			unsafe { instruction.InternalSetCodeNoCheck((Code)codes.codes[operandSize]); }
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = ((int)operandSize << 4) + (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase) + Register.AX;
			if (decoder.state.mod < 3) {
				instruction.Op1Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
			}
			else
				decoder.SetInvalidInstruction();
		}
	}

	sealed class OpCodeHandler_Sw_Ev : OpCodeHandlerModRM {
		readonly Code3 codes;

		public OpCodeHandler_Sw_Ev(Code code16, Code code32, Code code64) =>
			codes = new Code3(code16, code32, code64);

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			nuint operandSize = (nuint)decoder.state.operandSize;
			unsafe { instruction.InternalSetCodeNoCheck((Code)codes.codes[operandSize]); }
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			var sreg = decoder.ReadOpSegReg();
			if (decoder.invalidCheckMask != 0 && sreg == Register.CS)
				decoder.SetInvalidInstruction();
			instruction.Op0Register = sreg;
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op1Kind = OpKind.Register;
				instruction.Op1Register = ((int)operandSize << 4) + (int)(decoder.state.rm + decoder.state.zs.extraBaseRegisterBase) + Register.AX;
			}
			else {
				instruction.Op1Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_Sw_M : OpCodeHandlerModRM {
		readonly Code code;

		public OpCodeHandler_Sw_M(Code code) => this.code = code;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			instruction.InternalSetCodeNoCheck(code);
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = decoder.ReadOpSegReg();
			if (decoder.state.mod == 3)
				decoder.SetInvalidInstruction();
			else {
				instruction.Op1Kind = OpKind.Memory;
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
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			if (decoder.state.operandSize != OpSize.Size16)
				instruction.InternalSetCodeNoCheck(code32);
			else
				instruction.InternalSetCodeNoCheck(code16);
			if (decoder.state.operandSize != OpSize.Size16) {
				instruction.Op0Kind = OpKind.FarBranch32;
				instruction.FarBranch32 = decoder.ReadUInt32();
			}
			else {
				instruction.Op0Kind = OpKind.FarBranch16;
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
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			instruction.InternalSetCodeNoCheck(code);
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = reg;
			decoder.displIndex = decoder.state.zs.instructionLength;
			//instruction.InternalMemoryIndexScale = 0;
			//instruction.InternalMemoryBase = Register.None;
			//instruction.InternalMemoryIndex = Register.None;
			instruction.Op1Kind = OpKind.Memory;
			if (decoder.state.addressSize == OpSize.Size64) {
				instruction.InternalSetMemoryDisplSize(4);
				decoder.state.zs.flags |= StateFlags.Addr64;
				instruction.MemoryDisplacement64 = decoder.ReadUInt64();
			}
			else if (decoder.state.addressSize == OpSize.Size32) {
				instruction.InternalSetMemoryDisplSize(3);
				instruction.MemoryDisplacement64 = decoder.ReadUInt32();
			}
			else {
				instruction.InternalSetMemoryDisplSize(2);
				instruction.MemoryDisplacement64 = decoder.ReadUInt16();
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
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			instruction.InternalSetCodeNoCheck(code);
			decoder.displIndex = decoder.state.zs.instructionLength;
			//instruction.InternalMemoryIndexScale = 0;
			//instruction.InternalMemoryBase = Register.None;
			//instruction.InternalMemoryIndex = Register.None;
			instruction.Op0Kind = OpKind.Memory;
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op1Kind = OpKind.Register;
			instruction.Op1Register = reg;
			if (decoder.state.addressSize == OpSize.Size64) {
				instruction.InternalSetMemoryDisplSize(4);
				decoder.state.zs.flags |= StateFlags.Addr64;
				instruction.MemoryDisplacement64 = decoder.ReadUInt64();
			}
			else if (decoder.state.addressSize == OpSize.Size32) {
				instruction.InternalSetMemoryDisplSize(3);
				instruction.MemoryDisplacement64 = decoder.ReadUInt32();
			}
			else {
				instruction.InternalSetMemoryDisplSize(2);
				instruction.MemoryDisplacement64 = decoder.ReadUInt16();
			}
		}
	}

	sealed class OpCodeHandler_Reg_Ov : OpCodeHandler {
		readonly Code3 codes;

		public OpCodeHandler_Reg_Ov(Code code16, Code code32, Code code64) =>
			codes = new Code3(code16, code32, code64);

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			decoder.displIndex = decoder.state.zs.instructionLength;
			//instruction.InternalMemoryIndexScale = 0;
			//instruction.InternalMemoryBase = Register.None;
			//instruction.InternalMemoryIndex = Register.None;
			instruction.Op1Kind = OpKind.Memory;
			nuint operandSize = (nuint)decoder.state.operandSize;
			unsafe { instruction.InternalSetCodeNoCheck((Code)codes.codes[operandSize]); }
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = ((int)operandSize << 4) + Register.AX;
			if (decoder.state.addressSize == OpSize.Size64) {
				instruction.InternalSetMemoryDisplSize(4);
				decoder.state.zs.flags |= StateFlags.Addr64;
				instruction.MemoryDisplacement64 = decoder.ReadUInt64();
			}
			else if (decoder.state.addressSize == OpSize.Size32) {
				instruction.InternalSetMemoryDisplSize(3);
				instruction.MemoryDisplacement64 = decoder.ReadUInt32();
			}
			else {
				instruction.InternalSetMemoryDisplSize(2);
				instruction.MemoryDisplacement64 = decoder.ReadUInt16();
			}
		}
	}

	sealed class OpCodeHandler_Ov_Reg : OpCodeHandler {
		readonly Code3 codes;

		public OpCodeHandler_Ov_Reg(Code code16, Code code32, Code code64) =>
			codes = new Code3(code16, code32, code64);

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			decoder.displIndex = decoder.state.zs.instructionLength;
			//instruction.InternalMemoryIndexScale = 0;
			//instruction.InternalMemoryBase = Register.None;
			//instruction.InternalMemoryIndex = Register.None;
			instruction.Op0Kind = OpKind.Memory;
			nuint operandSize = (nuint)decoder.state.operandSize;
			unsafe { instruction.InternalSetCodeNoCheck((Code)codes.codes[operandSize]); }
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op1Kind = OpKind.Register;
			instruction.Op1Register = ((int)operandSize << 4) + Register.AX;
			if (decoder.state.addressSize == OpSize.Size64) {
				instruction.InternalSetMemoryDisplSize(4);
				decoder.state.zs.flags |= StateFlags.Addr64;
				instruction.MemoryDisplacement64 = decoder.ReadUInt64();
			}
			else if (decoder.state.addressSize == OpSize.Size32) {
				instruction.InternalSetMemoryDisplSize(3);
				instruction.MemoryDisplacement64 = decoder.ReadUInt32();
			}
			else {
				instruction.InternalSetMemoryDisplSize(2);
				instruction.MemoryDisplacement64 = decoder.ReadUInt16();
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
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			if (decoder.is64bMode) {
				if ((decoder.options & DecoderOptions.AMD) == 0 || decoder.state.operandSize != OpSize.Size16)
					instruction.InternalSetCodeNoCheck(code64);
				else
					instruction.InternalSetCodeNoCheck(code16);
			}
			else {
				if (decoder.state.operandSize == OpSize.Size32)
					instruction.InternalSetCodeNoCheck(code32);
				else
					instruction.InternalSetCodeNoCheck(code16);
			}
			instruction.Op0Kind = OpKind.Immediate16;
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
			if (decoder.is64bMode) {
				if ((decoder.options & DecoderOptions.AMD) == 0 || decoder.state.operandSize != OpSize.Size16)
					instruction.InternalSetCodeNoCheck(code64);
				else
					instruction.InternalSetCodeNoCheck(code16);
			}
			else {
				if (decoder.state.operandSize == OpSize.Size32)
					instruction.InternalSetCodeNoCheck(code32);
				else
					instruction.InternalSetCodeNoCheck(code16);
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
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			instruction.Op0Kind = OpKind.Immediate16;
			instruction.InternalImmediate16 = decoder.ReadUInt16();
			instruction.Op1Kind = OpKind.Immediate8_2nd;
			instruction.InternalImmediate8_2nd = decoder.ReadByte();
			if (decoder.is64bMode) {
				if (decoder.state.operandSize != OpSize.Size16)
					instruction.InternalSetCodeNoCheck(code64);
				else
					instruction.InternalSetCodeNoCheck(code16);
			}
			else {
				if (decoder.state.operandSize == OpSize.Size32)
					instruction.InternalSetCodeNoCheck(code32);
				else
					instruction.InternalSetCodeNoCheck(code16);
			}
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
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			instruction.Op1Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadByte();
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			if (decoder.state.operandSize != OpSize.Size16) {
				instruction.InternalSetCodeNoCheck(code32);
				instruction.Op0Register = Register.EAX;
			}
			else {
				instruction.InternalSetCodeNoCheck(code16);
				instruction.Op0Register = Register.AX;
			}
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
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			instruction.Op0Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadByte();
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op1Kind = OpKind.Register;
			if (decoder.state.operandSize != OpSize.Size16) {
				instruction.InternalSetCodeNoCheck(code32);
				instruction.Op1Register = Register.EAX;
			}
			else {
				instruction.InternalSetCodeNoCheck(code16);
				instruction.Op1Register = Register.AX;
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
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op1Kind = OpKind.Register;
			instruction.Op1Register = Register.DX;
			if (decoder.state.operandSize != OpSize.Size16) {
				instruction.InternalSetCodeNoCheck(code32);
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op0Kind = OpKind.Register;
				instruction.Op0Register = Register.EAX;
			}
			else {
				instruction.InternalSetCodeNoCheck(code16);
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op0Kind = OpKind.Register;
				instruction.Op0Register = Register.AX;
			}
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
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = Register.DX;
			if (decoder.state.operandSize != OpSize.Size16) {
				instruction.InternalSetCodeNoCheck(code32);
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op1Kind = OpKind.Register;
				instruction.Op1Register = Register.EAX;
			}
			else {
				instruction.InternalSetCodeNoCheck(code16);
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op1Kind = OpKind.Register;
				instruction.Op1Register = Register.AX;
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
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			instruction.InternalSetCodeNoCheck(code);
			if (decoder.state.mod < 3) {
				Static.Assert((int)HandlerFlags.Lock == 8 ? 0 : -1);
				Static.Assert((int)StateFlags.AllowLock == 0x00002000 ? 0 : -1);
				decoder.state.zs.flags |= (StateFlags)((uint)(flags & HandlerFlags.Lock) << (13 - 3));
				instruction.Op0Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
			}
			else {
				uint index = decoder.state.rm + decoder.state.zs.extraBaseRegisterBase;
				if ((decoder.state.zs.flags & StateFlags.HasRex) != 0 && index >= 4)
					index += 4;
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op0Kind = OpKind.Register;
				instruction.Op0Register = (int)index + Register.AL;
			}
			instruction.Op1Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadByte();
		}
	}

	sealed class OpCodeHandler_Eb_1 : OpCodeHandlerModRM {
		readonly Code code;

		public OpCodeHandler_Eb_1(Code code) => this.code = code;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			instruction.InternalSetCodeNoCheck(code);
			instruction.Op1Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = 1;
			decoder.state.zs.flags |= StateFlags.NoImm;
			if (decoder.state.mod == 3) {
				uint index = decoder.state.rm + decoder.state.zs.extraBaseRegisterBase;
				if ((decoder.state.zs.flags & StateFlags.HasRex) != 0 && index >= 4)
					index += 4;
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op0Kind = OpKind.Register;
				instruction.Op0Register = (int)index + Register.AL;
			}
			else {
				instruction.Op0Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_Eb_CL : OpCodeHandlerModRM {
		readonly Code code;

		public OpCodeHandler_Eb_CL(Code code) => this.code = code;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			instruction.InternalSetCodeNoCheck(code);
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op1Kind = OpKind.Register;
			instruction.Op1Register = Register.CL;
			if (decoder.state.mod == 3) {
				uint index = decoder.state.rm + decoder.state.zs.extraBaseRegisterBase;
				if ((decoder.state.zs.flags & StateFlags.HasRex) != 0 && index >= 4)
					index += 4;
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op0Kind = OpKind.Register;
				instruction.Op0Register = (int)index + Register.AL;
			}
			else {
				instruction.Op0Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
			}
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
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			instruction.InternalSetCodeNoCheck(code);
			if (decoder.state.mod == 3) {
				uint index = decoder.state.rm + decoder.state.zs.extraBaseRegisterBase;
				if ((decoder.state.zs.flags & StateFlags.HasRex) != 0 && index >= 4)
					index += 4;
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op0Kind = OpKind.Register;
				instruction.Op0Register = (int)index + Register.AL;
			}
			else {
				Static.Assert((int)HandlerFlags.Lock == 8 ? 0 : -1);
				Static.Assert((int)StateFlags.AllowLock == 0x00002000 ? 0 : -1);
				decoder.state.zs.flags |= (StateFlags)((uint)(flags & HandlerFlags.Lock) << (13 - 3));
				instruction.Op0Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
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
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			instruction.InternalSetCodeNoCheck(code);
			uint index;
			index = decoder.state.reg + decoder.state.zs.extraRegisterBase;
			if ((decoder.state.zs.flags & StateFlags.HasRex) != 0 && index >= 4)
				index += 4;
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op1Kind = OpKind.Register;
			instruction.Op1Register = (int)index + Register.AL;
			if (decoder.state.mod == 3) {
				index = decoder.state.rm + decoder.state.zs.extraBaseRegisterBase;
				if ((decoder.state.zs.flags & StateFlags.HasRex) != 0 && index >= 4)
					index += 4;
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op0Kind = OpKind.Register;
				instruction.Op0Register = (int)index + Register.AL;
			}
			else {
				Static.Assert((int)HandlerFlags.Lock == 8 ? 0 : -1);
				Static.Assert((int)StateFlags.AllowLock == 0x00002000 ? 0 : -1);
				decoder.state.zs.flags |= (StateFlags)((uint)(flags & HandlerFlags.Lock) << (13 - 3));
				instruction.Op0Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_Gb_Eb : OpCodeHandlerModRM {
		readonly Code code;

		public OpCodeHandler_Gb_Eb(Code code) => this.code = code;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			instruction.InternalSetCodeNoCheck(code);
			uint index = decoder.state.reg + decoder.state.zs.extraRegisterBase;
			if ((decoder.state.zs.flags & StateFlags.HasRex) != 0 && index >= 4)
				index += 4;
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = (int)index + Register.AL;

			if (decoder.state.mod == 3) {
				index = decoder.state.rm + decoder.state.zs.extraBaseRegisterBase;
				if ((decoder.state.zs.flags & StateFlags.HasRex) != 0 && index >= 4)
					index += 4;
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op1Kind = OpKind.Register;
				instruction.Op1Register = (int)index + Register.AL;
			}
			else {
				instruction.Op1Kind = OpKind.Memory;
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
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			if ((decoder.state.zs.flags & StateFlags.W) != 0)
				instruction.InternalSetCodeNoCheck(codeW1);
			else
				instruction.InternalSetCodeNoCheck(codeW0);
			if (decoder.state.mod == 3)
				decoder.SetInvalidInstruction();
			else {
				instruction.Op0Kind = OpKind.Memory;
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
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			if ((decoder.state.zs.flags & StateFlags.W) != 0)
				instruction.InternalSetCodeNoCheck(code64);
			else
				instruction.InternalSetCodeNoCheck(code32);
			if (decoder.state.mod == 3)
				decoder.SetInvalidInstruction();
			else {
				instruction.Op0Kind = OpKind.Memory;
				var flags = (decoder.state.zs.flags & StateFlags.W) != 0 ? flags64 : flags32;
				if ((flags & (HandlerFlags.Xacquire | HandlerFlags.Xrelease)) != 0)
					decoder.SetXacquireXrelease(ref instruction);
				Static.Assert((int)HandlerFlags.Lock == 8 ? 0 : -1);
				Static.Assert((int)StateFlags.AllowLock == 0x00002000 ? 0 : -1);
				decoder.state.zs.flags |= (StateFlags)((uint)(flags & HandlerFlags.Lock) << (13 - 3));
				decoder.ReadOpMem(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_MemBx : OpCodeHandler {
		readonly Code code;

		public OpCodeHandler_MemBx(Code code) => this.code = code;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			instruction.InternalSetCodeNoCheck(code);
			instruction.InternalMemoryIndex = Register.AL;
			instruction.Op0Kind = OpKind.Memory;
			//instruction.MemoryDisplacement64 = 0;
			//instruction.InternalMemoryIndexScale = 0;
			//instruction.InternalSetMemoryDisplSize(0);
			if (decoder.state.addressSize == OpSize.Size64)
				instruction.InternalMemoryBase = Register.RBX;
			else if (decoder.state.addressSize == OpSize.Size32)
				instruction.InternalMemoryBase = Register.EBX;
			else
				instruction.InternalMemoryBase = Register.BX;
		}
	}

	sealed class OpCodeHandler_VW : OpCodeHandlerModRM {
		readonly Code codeR;
		readonly Code codeM;

		public OpCodeHandler_VW(Code codeR, Code codeM) {
			this.codeR = codeR;
			this.codeM = codeM;
		}

		public OpCodeHandler_VW(Code code) {
			codeR = code;
			codeM = code;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase) + Register.XMM0;
			if (decoder.state.mod == 3) {
				instruction.InternalSetCodeNoCheck(codeR);
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op1Kind = OpKind.Register;
				instruction.Op1Register = (int)(decoder.state.rm + decoder.state.zs.extraBaseRegisterBase) + Register.XMM0;
			}
			else {
				instruction.InternalSetCodeNoCheck(codeM);
				instruction.Op1Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_WV : OpCodeHandlerModRM {
		readonly Code code;

		public OpCodeHandler_WV(Code code) => this.code = code;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			instruction.InternalSetCodeNoCheck(code);
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op1Kind = OpKind.Register;
			instruction.Op1Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase) + Register.XMM0;
			if (decoder.state.mod < 3) {
				instruction.Op0Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
			}
			else {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op0Kind = OpKind.Register;
				instruction.Op0Register = (int)(decoder.state.rm + decoder.state.zs.extraBaseRegisterBase) + Register.XMM0;
			}
		}
	}

	sealed class OpCodeHandler_rDI_VX_RX : OpCodeHandlerModRM {
		readonly Code code;

		public OpCodeHandler_rDI_VX_RX(Code code) => this.code = code;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			instruction.InternalSetCodeNoCheck(code);
			if (decoder.state.addressSize == OpSize.Size64)
				instruction.Op0Kind = OpKind.MemorySegRDI;
			else if (decoder.state.addressSize == OpSize.Size32)
				instruction.Op0Kind = OpKind.MemorySegEDI;
			else
				instruction.Op0Kind = OpKind.MemorySegDI;
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op1Kind = OpKind.Register;
			instruction.Op1Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase) + Register.XMM0;
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op2Kind = OpKind.Register;
				instruction.Op2Register = (int)(decoder.state.rm + decoder.state.zs.extraBaseRegisterBase) + Register.XMM0;
			}
			else
				decoder.SetInvalidInstruction();
		}
	}

	sealed class OpCodeHandler_rDI_P_N : OpCodeHandlerModRM {
		readonly Code code;

		public OpCodeHandler_rDI_P_N(Code code) => this.code = code;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			instruction.InternalSetCodeNoCheck(code);
			if (decoder.state.addressSize == OpSize.Size64)
				instruction.Op0Kind = OpKind.MemorySegRDI;
			else if (decoder.state.addressSize == OpSize.Size32)
				instruction.Op0Kind = OpKind.MemorySegEDI;
			else
				instruction.Op0Kind = OpKind.MemorySegDI;
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op1Kind = OpKind.Register;
			instruction.Op1Register = (int)decoder.state.reg + Register.MM0;
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op2Kind = OpKind.Register;
				instruction.Op2Register = (int)decoder.state.rm + Register.MM0;
			}
			else
				decoder.SetInvalidInstruction();
		}
	}

	sealed class OpCodeHandler_VM : OpCodeHandlerModRM {
		readonly Code code;

		public OpCodeHandler_VM(Code code) => this.code = code;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			instruction.InternalSetCodeNoCheck(code);
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase) + Register.XMM0;
			if (decoder.state.mod == 3)
				decoder.SetInvalidInstruction();
			else {
				instruction.Op1Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_MV : OpCodeHandlerModRM {
		readonly Code code;

		public OpCodeHandler_MV(Code code) => this.code = code;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			instruction.InternalSetCodeNoCheck(code);
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op1Kind = OpKind.Register;
			instruction.Op1Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase) + Register.XMM0;
			if (decoder.state.mod == 3)
				decoder.SetInvalidInstruction();
			else {
				instruction.Op0Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_VQ : OpCodeHandlerModRM {
		readonly Code code;

		public OpCodeHandler_VQ(Code code) => this.code = code;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			instruction.InternalSetCodeNoCheck(code);
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase) + Register.XMM0;
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op1Kind = OpKind.Register;
				instruction.Op1Register = (int)decoder.state.rm + Register.MM0;
			}
			else {
				instruction.Op1Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_P_Q : OpCodeHandlerModRM {
		readonly Code code;

		public OpCodeHandler_P_Q(Code code) => this.code = code;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			instruction.InternalSetCodeNoCheck(code);
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = (int)decoder.state.reg + Register.MM0;
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op1Kind = OpKind.Register;
				instruction.Op1Register = (int)decoder.state.rm + Register.MM0;
			}
			else {
				instruction.Op1Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_Q_P : OpCodeHandlerModRM {
		readonly Code code;

		public OpCodeHandler_Q_P(Code code) => this.code = code;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			instruction.InternalSetCodeNoCheck(code);
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op1Kind = OpKind.Register;
			instruction.Op1Register = (int)decoder.state.reg + Register.MM0;
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op0Kind = OpKind.Register;
				instruction.Op0Register = (int)decoder.state.rm + Register.MM0;
			}
			else {
				instruction.Op0Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_MP : OpCodeHandlerModRM {
		readonly Code code;

		public OpCodeHandler_MP(Code code) => this.code = code;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			instruction.InternalSetCodeNoCheck(code);
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op1Kind = OpKind.Register;
			instruction.Op1Register = (int)decoder.state.reg + Register.MM0;
			if (decoder.state.mod == 3)
				decoder.SetInvalidInstruction();
			else {
				instruction.Op0Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_P_Q_Ib : OpCodeHandlerModRM {
		readonly Code code;

		public OpCodeHandler_P_Q_Ib(Code code) => this.code = code;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			instruction.InternalSetCodeNoCheck(code);
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = (int)decoder.state.reg + Register.MM0;
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op1Kind = OpKind.Register;
				instruction.Op1Register = (int)decoder.state.rm + Register.MM0;
			}
			else {
				instruction.Op1Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
			}
			instruction.Op2Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadByte();
		}
	}

	sealed class OpCodeHandler_P_W : OpCodeHandlerModRM {
		readonly Code code;

		public OpCodeHandler_P_W(Code code) => this.code = code;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			instruction.InternalSetCodeNoCheck(code);
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = (int)decoder.state.reg + Register.MM0;
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op1Kind = OpKind.Register;
				instruction.Op1Register = (int)(decoder.state.rm + decoder.state.zs.extraBaseRegisterBase) + Register.XMM0;
			}
			else {
				instruction.Op1Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_P_R : OpCodeHandlerModRM {
		readonly Code code;

		public OpCodeHandler_P_R(Code code) => this.code = code;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			instruction.InternalSetCodeNoCheck(code);
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = (int)decoder.state.reg + Register.MM0;
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op1Kind = OpKind.Register;
				instruction.Op1Register = (int)(decoder.state.rm + decoder.state.zs.extraBaseRegisterBase) + Register.XMM0;
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
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			Register gpr;
			if ((decoder.state.zs.flags & StateFlags.W) != 0) {
				instruction.InternalSetCodeNoCheck(code64);
				gpr = Register.RAX;
			}
			else {
				instruction.InternalSetCodeNoCheck(code32);
				gpr = Register.EAX;
			}
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = (int)decoder.state.reg + Register.MM0;
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op1Kind = OpKind.Register;
				instruction.Op1Register = (int)(decoder.state.rm + decoder.state.zs.extraBaseRegisterBase) + gpr;
			}
			else {
				instruction.Op1Kind = OpKind.Memory;
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
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			Register gpr;
			if ((decoder.state.zs.flags & StateFlags.W) != 0) {
				instruction.InternalSetCodeNoCheck(code64);
				gpr = Register.RAX;
			}
			else {
				instruction.InternalSetCodeNoCheck(code32);
				gpr = Register.EAX;
			}
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = (int)decoder.state.reg + Register.MM0;
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op1Kind = OpKind.Register;
				instruction.Op1Register = (int)(decoder.state.rm + decoder.state.zs.extraBaseRegisterBase) + gpr;
			}
			else {
				instruction.Op1Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
			}
			instruction.Op2Kind = OpKind.Immediate8;
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
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op1Kind = OpKind.Register;
			instruction.Op1Register = (int)decoder.state.reg + Register.MM0;
			Register gpr;
			if ((decoder.state.zs.flags & StateFlags.W) != 0) {
				instruction.InternalSetCodeNoCheck(code64);
				gpr = Register.RAX;
			}
			else {
				instruction.InternalSetCodeNoCheck(code32);
				gpr = Register.EAX;
			}
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op0Kind = OpKind.Register;
				instruction.Op0Register = (int)(decoder.state.rm + decoder.state.zs.extraBaseRegisterBase) + gpr;
			}
			else {
				instruction.Op0Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_Gv_W : OpCodeHandlerModRM {
		readonly Code codeW0;
		readonly Code codeW1;

		public OpCodeHandler_Gv_W(Code codeW0, Code codeW1) {
			this.codeW0 = codeW0;
			this.codeW1 = codeW1;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			if ((decoder.state.zs.flags & StateFlags.W) != 0) {
				instruction.InternalSetCodeNoCheck(codeW1);
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op0Kind = OpKind.Register;
				instruction.Op0Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase) + Register.RAX;
			}
			else {
				instruction.InternalSetCodeNoCheck(codeW0);
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op0Kind = OpKind.Register;
				instruction.Op0Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase) + Register.EAX;
			}
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op1Kind = OpKind.Register;
				instruction.Op1Register = (int)(decoder.state.rm + decoder.state.zs.extraBaseRegisterBase) + Register.XMM0;
			}
			else {
				instruction.Op1Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_V_Ev : OpCodeHandlerModRM {
		readonly Code codeW0;
		readonly Code codeW1;

		public OpCodeHandler_V_Ev(Code codeW0, Code codeW1) {
			this.codeW0 = codeW0;
			this.codeW1 = codeW1;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			Register gpr;
			if (decoder.state.operandSize != OpSize.Size64) {
				instruction.InternalSetCodeNoCheck(codeW0);
				gpr = Register.EAX;
			}
			else {
				instruction.InternalSetCodeNoCheck(codeW1);
				gpr = Register.RAX;
			}
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase) + Register.XMM0;
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op1Kind = OpKind.Register;
				instruction.Op1Register = (int)(decoder.state.rm + decoder.state.zs.extraBaseRegisterBase) + gpr;
			}
			else {
				instruction.Op1Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_VWIb : OpCodeHandlerModRM {
		readonly Code codeW0;
		readonly Code codeW1;

		public OpCodeHandler_VWIb(Code code) {
			codeW0 = code;
			codeW1 = code;
		}

		public OpCodeHandler_VWIb(Code codeW0, Code codeW1) {
			this.codeW0 = codeW0;
			this.codeW1 = codeW1;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			if ((decoder.state.zs.flags & StateFlags.W) != 0)
				instruction.InternalSetCodeNoCheck(codeW1);
			else
				instruction.InternalSetCodeNoCheck(codeW0);
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase) + Register.XMM0;
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op1Kind = OpKind.Register;
				instruction.Op1Register = (int)(decoder.state.rm + decoder.state.zs.extraBaseRegisterBase) + Register.XMM0;
			}
			else {
				instruction.Op1Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
			}
			instruction.Op2Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadByte();
		}
	}

	sealed class OpCodeHandler_VRIbIb : OpCodeHandlerModRM {
		readonly Code code;

		public OpCodeHandler_VRIbIb(Code code) => this.code = code;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			instruction.InternalSetCodeNoCheck(code);
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase) + Register.XMM0;
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op1Kind = OpKind.Register;
				instruction.Op1Register = (int)(decoder.state.rm + decoder.state.zs.extraBaseRegisterBase) + Register.XMM0;
			}
			else
				decoder.SetInvalidInstruction();
			instruction.Op2Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadByte();
			instruction.Op3Kind = OpKind.Immediate8_2nd;
			instruction.InternalImmediate8_2nd = decoder.ReadByte();
		}
	}

	sealed class OpCodeHandler_RIbIb : OpCodeHandlerModRM {
		readonly Code code;

		public OpCodeHandler_RIbIb(Code code) => this.code = code;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			instruction.InternalSetCodeNoCheck(code);
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op0Kind = OpKind.Register;
				instruction.Op0Register = (int)(decoder.state.rm + decoder.state.zs.extraBaseRegisterBase) + Register.XMM0;
			}
			else
				decoder.SetInvalidInstruction();
			instruction.Op1Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadByte();
			instruction.Op2Kind = OpKind.Immediate8_2nd;
			instruction.InternalImmediate8_2nd = decoder.ReadByte();
		}
	}

	sealed class OpCodeHandler_RIb : OpCodeHandlerModRM {
		readonly Code code;

		public OpCodeHandler_RIb(Code code) => this.code = code;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			instruction.InternalSetCodeNoCheck(code);
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op0Kind = OpKind.Register;
				instruction.Op0Register = (int)(decoder.state.rm + decoder.state.zs.extraBaseRegisterBase) + Register.XMM0;
			}
			else
				decoder.SetInvalidInstruction();
			instruction.Op1Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadByte();
		}
	}

	sealed class OpCodeHandler_Ed_V_Ib : OpCodeHandlerModRM {
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_Ed_V_Ib(Code code32, Code code64) {
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op1Kind = OpKind.Register;
			instruction.Op1Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase) + Register.XMM0;
			Register gpr;
			if ((decoder.state.zs.flags & StateFlags.W) != 0) {
				instruction.InternalSetCodeNoCheck(code64);
				gpr = Register.RAX;
			}
			else {
				instruction.InternalSetCodeNoCheck(code32);
				gpr = Register.EAX;
			}
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op0Kind = OpKind.Register;
				instruction.Op0Register = (int)(decoder.state.rm + decoder.state.zs.extraBaseRegisterBase) + gpr;
			}
			else {
				instruction.Op0Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
			}
			instruction.Op2Kind = OpKind.Immediate8;
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
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			Register gpr;
			if ((decoder.state.zs.flags & StateFlags.W) != 0) {
				instruction.InternalSetCodeNoCheck(code64);
				gpr = Register.RAX;
			}
			else {
				instruction.InternalSetCodeNoCheck(code32);
				gpr = Register.EAX;
			}
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase) + Register.XMM0;
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op1Kind = OpKind.Register;
				instruction.Op1Register = (int)(decoder.state.rm + decoder.state.zs.extraBaseRegisterBase) + gpr;
			}
			else {
				instruction.Op1Kind = OpKind.Memory;
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
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op1Kind = OpKind.Register;
			instruction.Op1Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase) + Register.XMM0;
			Register gpr;
			if ((decoder.state.zs.flags & StateFlags.W) != 0) {
				instruction.InternalSetCodeNoCheck(code64);
				gpr = Register.RAX;
			}
			else {
				instruction.InternalSetCodeNoCheck(code32);
				gpr = Register.EAX;
			}
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op0Kind = OpKind.Register;
				instruction.Op0Register = (int)(decoder.state.rm + decoder.state.zs.extraBaseRegisterBase) + gpr;
			}
			else {
				instruction.Op0Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_VX_E_Ib : OpCodeHandlerModRM {
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_VX_E_Ib(Code code32, Code code64) {
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			Register gpr;
			if ((decoder.state.zs.flags & StateFlags.W) != 0) {
				instruction.InternalSetCodeNoCheck(code64);
				gpr = Register.RAX;
			}
			else {
				instruction.InternalSetCodeNoCheck(code32);
				gpr = Register.EAX;
			}
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase) + Register.XMM0;
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op1Kind = OpKind.Register;
				instruction.Op1Register = (int)(decoder.state.rm + decoder.state.zs.extraBaseRegisterBase) + gpr;
			}
			else {
				instruction.Op1Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
			}
			instruction.Op2Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadByte();
		}
	}

	sealed class OpCodeHandler_Gv_RX : OpCodeHandlerModRM {
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_Gv_RX(Code code32, Code code64) {
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			if ((decoder.state.zs.flags & StateFlags.W) != 0)
				instruction.InternalSetCodeNoCheck(code64);
			else
				instruction.InternalSetCodeNoCheck(code32);
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			if ((decoder.state.zs.flags & StateFlags.W) != 0)
				instruction.Op0Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase) + Register.RAX;
			else
				instruction.Op0Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase) + Register.EAX;
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op1Kind = OpKind.Register;
				instruction.Op1Register = (int)(decoder.state.rm + decoder.state.zs.extraBaseRegisterBase) + Register.XMM0;
			}
			else
				decoder.SetInvalidInstruction();
		}
	}

	sealed class OpCodeHandler_B_MIB : OpCodeHandlerModRM {
		readonly Code code;

		public OpCodeHandler_B_MIB(Code code) => this.code = code;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			if (decoder.state.reg > 3 || (decoder.state.zs.extraRegisterBase & decoder.invalidCheckMask) != 0)
				decoder.SetInvalidInstruction();
			instruction.InternalSetCodeNoCheck(code);
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = (int)decoder.state.reg + Register.BND0;
			Debug.Assert(decoder.state.mod != 3);
			instruction.Op1Kind = OpKind.Memory;
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
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			if (decoder.state.reg > 3 || (decoder.state.zs.extraRegisterBase & decoder.invalidCheckMask) != 0)
				decoder.SetInvalidInstruction();
			instruction.InternalSetCodeNoCheck(code);
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op1Kind = OpKind.Register;
			instruction.Op1Register = (int)decoder.state.reg + Register.BND0;
			Debug.Assert(decoder.state.mod != 3);
			instruction.Op0Kind = OpKind.Memory;
			decoder.ReadOpMem_MPX(ref instruction);
			// It can't be EIP since if it's MPX + 64-bit, the address size is always 64-bit
			if (decoder.invalidCheckMask != 0 && instruction.MemoryBase == Register.RIP)
				decoder.SetInvalidInstruction();
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
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			if (decoder.state.reg > 3 || (decoder.state.zs.extraRegisterBase & decoder.invalidCheckMask) != 0)
				decoder.SetInvalidInstruction();
			if (decoder.is64bMode)
				instruction.InternalSetCodeNoCheck(code64);
			else
				instruction.InternalSetCodeNoCheck(code32);
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = (int)decoder.state.reg + Register.BND0;
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op1Kind = OpKind.Register;
				instruction.Op1Register = (int)decoder.state.rm + Register.BND0;
				if (decoder.state.rm > 3 || (decoder.state.zs.extraBaseRegisterBase & decoder.invalidCheckMask) != 0)
					decoder.SetInvalidInstruction();
			}
			else {
				instruction.Op1Kind = OpKind.Memory;
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
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			if (decoder.state.reg > 3 || ((decoder.state.zs.extraRegisterBase & decoder.invalidCheckMask) != 0))
				decoder.SetInvalidInstruction();
			if (decoder.is64bMode)
				instruction.InternalSetCodeNoCheck(code64);
			else
				instruction.InternalSetCodeNoCheck(code32);
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op1Kind = OpKind.Register;
			instruction.Op1Register = (int)decoder.state.reg + Register.BND0;
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op0Kind = OpKind.Register;
				instruction.Op0Register = (int)decoder.state.rm + Register.BND0;
				if (decoder.state.rm > 3 || (decoder.state.zs.extraBaseRegisterBase & decoder.invalidCheckMask) != 0)
					decoder.SetInvalidInstruction();
			}
			else {
				instruction.Op0Kind = OpKind.Memory;
				decoder.ReadOpMem_MPX(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_B_Ev : OpCodeHandlerModRM {
		readonly Code code32;
		readonly Code code64;
		readonly uint ripRelMask;

		public OpCodeHandler_B_Ev(Code code32, Code code64, bool supportsRipRel) {
			this.code32 = code32;
			this.code64 = code64;
			ripRelMask = supportsRipRel ? 0 : uint.MaxValue;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			if (decoder.state.reg > 3 || (decoder.state.zs.extraRegisterBase & decoder.invalidCheckMask) != 0)
				decoder.SetInvalidInstruction();
			Register baseReg;
			if (decoder.is64bMode) {
				instruction.InternalSetCodeNoCheck(code64);
				baseReg = Register.RAX;
			}
			else {
				instruction.InternalSetCodeNoCheck(code32);
				baseReg = Register.EAX;
			}
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = (int)decoder.state.reg + Register.BND0;
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op1Kind = OpKind.Register;
				instruction.Op1Register = (int)(decoder.state.rm + decoder.state.zs.extraBaseRegisterBase) + baseReg;
			}
			else {
				instruction.Op1Kind = OpKind.Memory;
				decoder.ReadOpMem_MPX(ref instruction);
				// It can't be EIP since if it's MPX + 64-bit, the address size is always 64-bit
				if ((ripRelMask & decoder.invalidCheckMask) != 0 && instruction.MemoryBase == Register.RIP)
					decoder.SetInvalidInstruction();
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
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			if ((decoder.state.zs.flags & StateFlags.W) != 0) {
				instruction.InternalSetCodeNoCheck(code64);
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op1Kind = OpKind.Register;
				instruction.Op1Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase) + Register.RAX;
			}
			else {
				instruction.InternalSetCodeNoCheck(code32);
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op1Kind = OpKind.Register;
				instruction.Op1Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase) + Register.EAX;
			}
			if (decoder.state.mod == 3)
				decoder.SetInvalidInstruction();
			else {
				instruction.Op0Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
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
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			if ((decoder.state.zs.flags & StateFlags.W) != 0)
				instruction.InternalSetCodeNoCheck(code64);
			else
				instruction.InternalSetCodeNoCheck(code32);
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			if ((decoder.state.zs.flags & StateFlags.W) != 0)
				instruction.Op0Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase) + Register.RAX;
			else
				instruction.Op0Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase) + Register.EAX;
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op1Kind = OpKind.Register;
				instruction.Op1Register = (int)decoder.state.rm + Register.MM0;
			}
			else
				decoder.SetInvalidInstruction();
			instruction.Op2Kind = OpKind.Immediate8;
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
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			if ((decoder.state.zs.flags & StateFlags.W) != 0)
				instruction.InternalSetCodeNoCheck(code64);
			else
				instruction.InternalSetCodeNoCheck(code32);
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			if ((decoder.state.zs.flags & StateFlags.W) != 0)
				instruction.Op0Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase) + Register.RAX;
			else
				instruction.Op0Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase) + Register.EAX;
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op1Kind = OpKind.Register;
				instruction.Op1Register = (int)decoder.state.rm + Register.MM0;
			}
			else
				decoder.SetInvalidInstruction();
		}
	}

	sealed class OpCodeHandler_VN : OpCodeHandlerModRM {
		readonly Code code;

		public OpCodeHandler_VN(Code code) => this.code = code;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			instruction.InternalSetCodeNoCheck(code);
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase) + Register.XMM0;
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op1Kind = OpKind.Register;
				instruction.Op1Register = (int)decoder.state.rm + Register.MM0;
			}
			else
				decoder.SetInvalidInstruction();
		}
	}

	sealed class OpCodeHandler_Gv_Mv : OpCodeHandlerModRM {
		readonly Code3 codes;

		public OpCodeHandler_Gv_Mv(Code code16, Code code32, Code code64) =>
			codes = new Code3(code16, code32, code64);

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			nuint operandSize = (nuint)decoder.state.operandSize;
			unsafe { instruction.InternalSetCodeNoCheck((Code)codes.codes[operandSize]); }
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = ((int)operandSize << 4) + (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase) + Register.AX;
			if (decoder.state.mod == 3)
				decoder.SetInvalidInstruction();
			else {
				instruction.Op1Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_Mv_Gv : OpCodeHandlerModRM {
		readonly Code3 codes;

		public OpCodeHandler_Mv_Gv(Code code16, Code code32, Code code64) =>
			codes = new Code3(code16, code32, code64);

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			nuint operandSize = (nuint)decoder.state.operandSize;
			unsafe { instruction.InternalSetCodeNoCheck((Code)codes.codes[operandSize]); }
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op1Kind = OpKind.Register;
			instruction.Op1Register = ((int)operandSize << 4) + (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase) + Register.AX;
			if (decoder.state.mod == 3)
				decoder.SetInvalidInstruction();
			else {
				instruction.Op0Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
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
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			if ((decoder.state.zs.flags & StateFlags.W) != 0) {
				instruction.InternalSetCodeNoCheck(code64);
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op0Kind = OpKind.Register;
				instruction.Op0Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase) + Register.RAX;
			}
			else {
				instruction.InternalSetCodeNoCheck(code32);
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op0Kind = OpKind.Register;
				instruction.Op0Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase) + Register.EAX;
			}
			if (decoder.state.mod == 3) {
				uint index = decoder.state.rm + decoder.state.zs.extraBaseRegisterBase;
				if ((decoder.state.zs.flags & StateFlags.HasRex) != 0 && index >= 4)
					index += 4;
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op1Kind = OpKind.Register;
				instruction.Op1Register = (int)index + Register.AL;
			}
			else {
				instruction.Op1Kind = OpKind.Memory;
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
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			if ((decoder.state.zs.flags & StateFlags.W) != 0) {
				instruction.InternalSetCodeNoCheck(code64);
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op0Kind = OpKind.Register;
				instruction.Op0Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase) + Register.RAX;
			}
			else {
				instruction.InternalSetCodeNoCheck(code32);
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op0Kind = OpKind.Register;
				instruction.Op0Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase) + Register.EAX;
			}
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op1Kind = OpKind.Register;
				if ((decoder.state.zs.flags & StateFlags.W) != 0)
					instruction.Op1Register = (int)(decoder.state.rm + decoder.state.zs.extraBaseRegisterBase) + Register.RAX;
				else
					instruction.Op1Register = (int)(decoder.state.rm + decoder.state.zs.extraBaseRegisterBase) + Register.EAX;
			}
			else {
				instruction.Op1Kind = OpKind.Memory;
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
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			if ((decoder.state.zs.flags & StateFlags.W) != 0) {
				instruction.InternalSetCodeNoCheck(code64);
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op1Kind = OpKind.Register;
				instruction.Op1Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase) + Register.RAX;
			}
			else {
				instruction.InternalSetCodeNoCheck(code32);
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op1Kind = OpKind.Register;
				instruction.Op1Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase) + Register.EAX;
			}
			Debug.Assert(decoder.state.mod != 3);
			instruction.Op0Kind = OpKind.Memory;
			decoder.ReadOpMem(ref instruction);
		}
	}

	sealed class OpCodeHandler_GvM_VX_Ib : OpCodeHandlerModRM {
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_GvM_VX_Ib(Code code32, Code code64) {
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op1Kind = OpKind.Register;
			instruction.Op1Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase) + Register.XMM0;
			Register gpr;
			if ((decoder.state.zs.flags & StateFlags.W) != 0) {
				instruction.InternalSetCodeNoCheck(code64);
				gpr = Register.RAX;
			}
			else {
				instruction.InternalSetCodeNoCheck(code32);
				gpr = Register.EAX;
			}
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op0Kind = OpKind.Register;
				instruction.Op0Register = (int)(decoder.state.rm + decoder.state.zs.extraBaseRegisterBase) + gpr;
			}
			else {
				instruction.Op0Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
			}
			instruction.Op2Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadByte();
		}
	}

	sealed class OpCodeHandler_Wbinvd : OpCodeHandler {
		public OpCodeHandler_Wbinvd() { }

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			if ((decoder.options & DecoderOptions.NoWbnoinvd) != 0 || decoder.state.zs.mandatoryPrefix != MandatoryPrefixByte.PF3)
				instruction.InternalSetCodeNoCheck(Code.Wbinvd);
			else {
				decoder.ClearMandatoryPrefixF3(ref instruction);
				instruction.InternalSetCodeNoCheck(Code.Wbnoinvd);
			}
		}
	}
}
#endif
