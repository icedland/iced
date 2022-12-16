// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if DECODER && (!NO_VEX || !NO_XOP)
using System;
using System.Diagnostics;

namespace Iced.Intel.DecoderInternal {
	sealed class OpCodeHandler_VectorLength_VEX : OpCodeHandlerModRM {
		readonly OpCodeHandler[] handlers;

		public OpCodeHandler_VectorLength_VEX(OpCodeHandler handler128, OpCodeHandler handler256) {
			Static.Assert((int)VectorLength.L128 == 0 ? 0 : -1);
			Static.Assert((int)VectorLength.L256 == 1 ? 0 : -1);
			Static.Assert((int)VectorLength.L512 == 2 ? 0 : -1);
			Static.Assert((int)VectorLength.Unknown == 3 ? 0 : -1);
			handlers = new OpCodeHandler[4] {
				handler128 ?? throw new ArgumentNullException(nameof(handler128)),
				handler256 ?? throw new ArgumentNullException(nameof(handler256)),
				OpCodeHandler_Invalid.Instance,
				OpCodeHandler_Invalid.Instance,
			};
			Debug.Assert(handler128.HasModRM == HasModRM);
			Debug.Assert(handler256.HasModRM == HasModRM);
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.VEX || decoder.state.Encoding == EncodingKind.XOP);
			handlers[(int)decoder.state.vectorLength].Decode(decoder, ref instruction);
		}
	}

	sealed class OpCodeHandler_VectorLength_NoModRM_VEX : OpCodeHandler {
		readonly OpCodeHandler[] handlers;

		public OpCodeHandler_VectorLength_NoModRM_VEX(OpCodeHandler handler128, OpCodeHandler handler256) {
			Static.Assert((int)VectorLength.L128 == 0 ? 0 : -1);
			Static.Assert((int)VectorLength.L256 == 1 ? 0 : -1);
			Static.Assert((int)VectorLength.L512 == 2 ? 0 : -1);
			Static.Assert((int)VectorLength.Unknown == 3 ? 0 : -1);
			handlers = new OpCodeHandler[4] {
				handler128 ?? throw new ArgumentNullException(nameof(handler128)),
				handler256 ?? throw new ArgumentNullException(nameof(handler256)),
				OpCodeHandler_Invalid.Instance,
				OpCodeHandler_Invalid.Instance,
			};
			Debug.Assert(handler128.HasModRM == HasModRM);
			Debug.Assert(handler256.HasModRM == HasModRM);
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.VEX || decoder.state.Encoding == EncodingKind.XOP);
			handlers[(int)decoder.state.vectorLength].Decode(decoder, ref instruction);
		}
	}

	sealed class OpCodeHandler_VEX_Simple : OpCodeHandler {
		readonly Code code;
		public OpCodeHandler_VEX_Simple(Code code) => this.code = code;
		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.VEX || decoder.state.Encoding == EncodingKind.XOP);
			if ((decoder.state.vvvv_invalidCheck & decoder.invalidCheckMask) != 0)
				decoder.SetInvalidInstruction();
			instruction.InternalSetCodeNoCheck(code);
		}
	}

	sealed class OpCodeHandler_VEX_VHEv : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code codeW0;
		readonly Code codeW1;

		public OpCodeHandler_VEX_VHEv(Register baseReg, Code codeW0, Code codeW1) {
			this.baseReg = baseReg;
			this.codeW0 = codeW0;
			this.codeW1 = codeW1;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.VEX || decoder.state.Encoding == EncodingKind.XOP);
			Register gpr;
			if (((uint)decoder.state.zs.flags & decoder.is64bMode_and_W) != 0) {
				instruction.InternalSetCodeNoCheck(codeW1);
				gpr = Register.RAX;
			}
			else {
				instruction.InternalSetCodeNoCheck(codeW0);
				gpr = Register.EAX;
			}
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase) + baseReg;
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op1Kind = OpKind.Register;
			instruction.Op1Register = (int)decoder.state.vvvv + baseReg;
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op2Kind = OpKind.Register;
				instruction.Op2Register = (int)(decoder.state.rm + decoder.state.zs.extraBaseRegisterBase) + gpr;
			}
			else {
				instruction.Op2Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_VEX_VHEvIb : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code codeW0;
		readonly Code codeW1;

		public OpCodeHandler_VEX_VHEvIb(Register baseReg, Code codeW0, Code codeW1) {
			this.baseReg = baseReg;
			this.codeW0 = codeW0;
			this.codeW1 = codeW1;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.VEX || decoder.state.Encoding == EncodingKind.XOP);
			Register gpr;
			if (((uint)decoder.state.zs.flags & decoder.is64bMode_and_W) != 0) {
				instruction.InternalSetCodeNoCheck(codeW1);
				gpr = Register.RAX;
			}
			else {
				instruction.InternalSetCodeNoCheck(codeW0);
				gpr = Register.EAX;
			}
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase) + baseReg;
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op1Kind = OpKind.Register;
			instruction.Op1Register = (int)decoder.state.vvvv + baseReg;
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op2Kind = OpKind.Register;
				instruction.Op2Register = (int)(decoder.state.rm + decoder.state.zs.extraBaseRegisterBase) + gpr;
			}
			else {
				instruction.Op2Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
			}
			instruction.Op3Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadByte();
		}
	}

	sealed class OpCodeHandler_VEX_VW : OpCodeHandlerModRM {
		readonly Register baseReg1;
		readonly Register baseReg2;
		readonly Code code;

		public OpCodeHandler_VEX_VW(Register baseReg, Code code) {
			baseReg1 = baseReg;
			baseReg2 = baseReg;
			this.code = code;
		}

		public OpCodeHandler_VEX_VW(Register baseReg1, Register baseReg2, Code code) {
			this.baseReg1 = baseReg1;
			this.baseReg2 = baseReg2;
			this.code = code;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.VEX || decoder.state.Encoding == EncodingKind.XOP);
			if ((decoder.state.vvvv_invalidCheck & decoder.invalidCheckMask) != 0)
				decoder.SetInvalidInstruction();
			instruction.InternalSetCodeNoCheck(code);
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase) + baseReg1;
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op1Kind = OpKind.Register;
				instruction.Op1Register = (int)(decoder.state.rm + decoder.state.zs.extraBaseRegisterBase) + baseReg2;
			}
			else {
				instruction.Op1Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_VEX_VX_Ev : OpCodeHandlerModRM {
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_VEX_VX_Ev(Code code32, Code code64) {
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.VEX || decoder.state.Encoding == EncodingKind.XOP);
			if ((decoder.state.vvvv_invalidCheck & decoder.invalidCheckMask) != 0)
				decoder.SetInvalidInstruction();
			Register gpr;
			if (((uint)decoder.state.zs.flags & decoder.is64bMode_and_W) != 0) {
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

	sealed class OpCodeHandler_VEX_Ev_VX : OpCodeHandlerModRM {
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_VEX_Ev_VX(Code code32, Code code64) {
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.VEX || decoder.state.Encoding == EncodingKind.XOP);
			if ((decoder.state.vvvv_invalidCheck & decoder.invalidCheckMask) != 0)
				decoder.SetInvalidInstruction();
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op1Kind = OpKind.Register;
			instruction.Op1Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase) + Register.XMM0;
			Register gpr;
			if (((uint)decoder.state.zs.flags & decoder.is64bMode_and_W) != 0) {
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

	sealed class OpCodeHandler_VEX_WV : OpCodeHandlerModRM {
		readonly Register baseReg1;
		readonly Register baseReg2;
		readonly Code code;

		public OpCodeHandler_VEX_WV(Register baseReg, Code code) {
			baseReg1 = baseReg;
			baseReg2 = baseReg;
			this.code = code;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.VEX || decoder.state.Encoding == EncodingKind.XOP);
			if ((decoder.state.vvvv_invalidCheck & decoder.invalidCheckMask) != 0)
				decoder.SetInvalidInstruction();
			instruction.InternalSetCodeNoCheck(code);
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op1Kind = OpKind.Register;
			instruction.Op1Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase) + baseReg1;
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op0Kind = OpKind.Register;
				instruction.Op0Register = (int)(decoder.state.rm + decoder.state.zs.extraBaseRegisterBase) + baseReg2;
			}
			else {
				instruction.Op0Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_VEX_VM : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code code;

		public OpCodeHandler_VEX_VM(Register baseReg, Code code) {
			this.baseReg = baseReg;
			this.code = code;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.VEX || decoder.state.Encoding == EncodingKind.XOP);
			if ((decoder.state.vvvv_invalidCheck & decoder.invalidCheckMask) != 0)
				decoder.SetInvalidInstruction();
			instruction.InternalSetCodeNoCheck(code);
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase) + baseReg;
			if (decoder.state.mod == 3)
				decoder.SetInvalidInstruction();
			else {
				instruction.Op1Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_VEX_MV : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code code;

		public OpCodeHandler_VEX_MV(Register baseReg, Code code) {
			this.baseReg = baseReg;
			this.code = code;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.VEX || decoder.state.Encoding == EncodingKind.XOP);
			if ((decoder.state.vvvv_invalidCheck & decoder.invalidCheckMask) != 0)
				decoder.SetInvalidInstruction();
			instruction.InternalSetCodeNoCheck(code);
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op1Kind = OpKind.Register;
			instruction.Op1Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase) + baseReg;
			if (decoder.state.mod == 3)
				decoder.SetInvalidInstruction();
			else {
				instruction.Op0Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_VEX_M : OpCodeHandlerModRM {
		readonly Code code;

		public OpCodeHandler_VEX_M(Code code) => this.code = code;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.VEX || decoder.state.Encoding == EncodingKind.XOP);
			if ((decoder.state.vvvv_invalidCheck & decoder.invalidCheckMask) != 0)
				decoder.SetInvalidInstruction();
			instruction.InternalSetCodeNoCheck(code);
			if (decoder.state.mod == 3)
				decoder.SetInvalidInstruction();
			else {
				instruction.Op0Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_VEX_RdRq : OpCodeHandlerModRM {
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_VEX_RdRq(Code code32, Code code64) {
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.VEX || decoder.state.Encoding == EncodingKind.XOP);
			if ((decoder.state.vvvv_invalidCheck & decoder.invalidCheckMask) != 0)
				decoder.SetInvalidInstruction();
			if (((uint)decoder.state.zs.flags & decoder.is64bMode_and_W) != 0) {
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
			if (decoder.state.mod != 3)
				decoder.SetInvalidInstruction();
		}
	}

	sealed class OpCodeHandler_VEX_rDI_VX_RX : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code code;

		public OpCodeHandler_VEX_rDI_VX_RX(Register baseReg, Code code) {
			this.baseReg = baseReg;
			this.code = code;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.VEX || decoder.state.Encoding == EncodingKind.XOP);
			if ((decoder.state.vvvv_invalidCheck & decoder.invalidCheckMask) != 0)
				decoder.SetInvalidInstruction();
			instruction.InternalSetCodeNoCheck(code);
			if (decoder.state.addressSize == OpSize.Size64)
				instruction.Op0Kind = OpKind.MemorySegRDI;
			else if (decoder.state.addressSize == OpSize.Size32)
				instruction.Op0Kind = OpKind.MemorySegEDI;
			else
				instruction.Op0Kind = OpKind.MemorySegDI;
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op1Kind = OpKind.Register;
			instruction.Op1Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase) + baseReg;
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op2Kind = OpKind.Register;
				instruction.Op2Register = (int)(decoder.state.rm + decoder.state.zs.extraBaseRegisterBase) + baseReg;
			}
			else
				decoder.SetInvalidInstruction();
		}
	}

	sealed class OpCodeHandler_VEX_VWIb : OpCodeHandlerModRM {
		readonly Register baseReg1;
		readonly Register baseReg2;
		readonly Code codeW0;
		readonly Code codeW1;

		public OpCodeHandler_VEX_VWIb(Register baseReg, Code code) {
			baseReg1 = baseReg;
			baseReg2 = baseReg;
			codeW0 = code;
			codeW1 = code;
		}

		public OpCodeHandler_VEX_VWIb(Register baseReg, Code codeW0, Code codeW1) {
			baseReg1 = baseReg;
			baseReg2 = baseReg;
			this.codeW0 = codeW0;
			this.codeW1 = codeW1;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.VEX || decoder.state.Encoding == EncodingKind.XOP);
			if ((decoder.state.vvvv_invalidCheck & decoder.invalidCheckMask) != 0)
				decoder.SetInvalidInstruction();
			if (((uint)decoder.state.zs.flags & decoder.is64bMode_and_W) != 0)
				instruction.InternalSetCodeNoCheck(codeW1);
			else
				instruction.InternalSetCodeNoCheck(codeW0);
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase) + baseReg1;
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op1Kind = OpKind.Register;
				instruction.Op1Register = (int)(decoder.state.rm + decoder.state.zs.extraBaseRegisterBase) + baseReg2;
			}
			else {
				instruction.Op1Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
			}
			instruction.Op2Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadByte();
		}
	}

	sealed class OpCodeHandler_VEX_WVIb : OpCodeHandlerModRM {
		readonly Register baseReg1;
		readonly Register baseReg2;
		readonly Code code;

		public OpCodeHandler_VEX_WVIb(Register baseReg1, Register baseReg2, Code code) {
			this.baseReg1 = baseReg1;
			this.baseReg2 = baseReg2;
			this.code = code;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.VEX || decoder.state.Encoding == EncodingKind.XOP);
			if ((decoder.state.vvvv_invalidCheck & decoder.invalidCheckMask) != 0)
				decoder.SetInvalidInstruction();
			instruction.InternalSetCodeNoCheck(code);
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op0Kind = OpKind.Register;
				instruction.Op0Register = (int)(decoder.state.rm + decoder.state.zs.extraBaseRegisterBase) + baseReg1;
			}
			else {
				instruction.Op0Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
			}
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op1Kind = OpKind.Register;
			instruction.Op1Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase) + baseReg2;
			instruction.Op2Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadByte();
		}
	}

	sealed class OpCodeHandler_VEX_Ed_V_Ib : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_VEX_Ed_V_Ib(Register baseReg, Code code32, Code code64) {
			this.baseReg = baseReg;
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.VEX || decoder.state.Encoding == EncodingKind.XOP);
			if ((decoder.state.vvvv_invalidCheck & decoder.invalidCheckMask) != 0)
				decoder.SetInvalidInstruction();
			Register gpr;
			if (((uint)decoder.state.zs.flags & decoder.is64bMode_and_W) != 0) {
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
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op1Kind = OpKind.Register;
			instruction.Op1Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase) + baseReg;
			instruction.Op2Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadByte();
		}
	}

	sealed class OpCodeHandler_VEX_VHW : OpCodeHandlerModRM {
		readonly Register baseReg1;
		readonly Register baseReg2;
		readonly Register baseReg3;
		readonly Code codeR;
		readonly Code codeM;

		public OpCodeHandler_VEX_VHW(Register baseReg, Code codeR, Code codeM) {
			baseReg1 = baseReg;
			baseReg2 = baseReg;
			baseReg3 = baseReg;
			this.codeR = codeR;
			this.codeM = codeM;
		}

		public OpCodeHandler_VEX_VHW(Register baseReg, Code code) {
			baseReg1 = baseReg;
			baseReg2 = baseReg;
			baseReg3 = baseReg;
			codeR = code;
			codeM = code;
		}

		public OpCodeHandler_VEX_VHW(Register baseReg1, Register baseReg2, Register baseReg3, Code code) {
			this.baseReg1 = baseReg1;
			this.baseReg2 = baseReg2;
			this.baseReg3 = baseReg3;
			codeR = code;
			codeM = code;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.VEX || decoder.state.Encoding == EncodingKind.XOP);
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase) + baseReg1;
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op1Kind = OpKind.Register;
			instruction.Op1Register = (int)decoder.state.vvvv + baseReg2;
			if (decoder.state.mod == 3) {
				instruction.InternalSetCodeNoCheck(codeR);
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op2Kind = OpKind.Register;
				instruction.Op2Register = (int)(decoder.state.rm + decoder.state.zs.extraBaseRegisterBase) + baseReg3;
			}
			else {
				instruction.InternalSetCodeNoCheck(codeM);
				instruction.Op2Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_VEX_VWH : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code code;

		public OpCodeHandler_VEX_VWH(Register baseReg, Code code) {
			this.baseReg = baseReg;
			this.code = code;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.VEX || decoder.state.Encoding == EncodingKind.XOP);
			instruction.InternalSetCodeNoCheck(code);
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase) + baseReg;
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op2Kind = OpKind.Register;
			instruction.Op2Register = (int)decoder.state.vvvv + baseReg;
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op1Kind = OpKind.Register;
				instruction.Op1Register = (int)(decoder.state.rm + decoder.state.zs.extraBaseRegisterBase) + baseReg;
			}
			else {
				instruction.Op1Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_VEX_WHV : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code codeR;

		public OpCodeHandler_VEX_WHV(Register baseReg, Code code) {
			this.baseReg = baseReg;
			codeR = code;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.VEX || decoder.state.Encoding == EncodingKind.XOP);
			Debug.Assert(decoder.state.mod == 3);
			instruction.InternalSetCodeNoCheck(codeR);
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = (int)(decoder.state.rm + decoder.state.zs.extraBaseRegisterBase) + baseReg;
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op1Kind = OpKind.Register;
			instruction.Op1Register = (int)decoder.state.vvvv + baseReg;
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op2Kind = OpKind.Register;
			instruction.Op2Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase) + baseReg;
		}
	}

	sealed class OpCodeHandler_VEX_VHM : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code code;

		public OpCodeHandler_VEX_VHM(Register baseReg, Code code) {
			this.baseReg = baseReg;
			this.code = code;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.VEX || decoder.state.Encoding == EncodingKind.XOP);
			instruction.InternalSetCodeNoCheck(code);
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase) + baseReg;
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op1Kind = OpKind.Register;
			instruction.Op1Register = (int)decoder.state.vvvv + baseReg;
			if (decoder.state.mod == 3)
				decoder.SetInvalidInstruction();
			else {
				instruction.Op2Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_VEX_MHV : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code code;

		public OpCodeHandler_VEX_MHV(Register baseReg, Code code) {
			this.baseReg = baseReg;
			this.code = code;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.VEX || decoder.state.Encoding == EncodingKind.XOP);
			instruction.InternalSetCodeNoCheck(code);
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op1Kind = OpKind.Register;
			instruction.Op1Register = (int)decoder.state.vvvv + baseReg;
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op2Kind = OpKind.Register;
			instruction.Op2Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase) + baseReg;
			if (decoder.state.mod == 3)
				decoder.SetInvalidInstruction();
			else {
				instruction.Op0Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_VEX_VHWIb : OpCodeHandlerModRM {
		readonly Register baseReg1;
		readonly Register baseReg2;
		readonly Register baseReg3;
		readonly Code code;

		public OpCodeHandler_VEX_VHWIb(Register baseReg, Code code) {
			baseReg1 = baseReg;
			baseReg2 = baseReg;
			baseReg3 = baseReg;
			this.code = code;
		}

		public OpCodeHandler_VEX_VHWIb(Register baseReg1, Register baseReg2, Register baseReg3, Code code) {
			this.baseReg1 = baseReg1;
			this.baseReg2 = baseReg2;
			this.baseReg3 = baseReg3;
			this.code = code;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.VEX || decoder.state.Encoding == EncodingKind.XOP);
			instruction.InternalSetCodeNoCheck(code);
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase) + baseReg1;
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op1Kind = OpKind.Register;
			instruction.Op1Register = (int)decoder.state.vvvv + baseReg2;
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op2Kind = OpKind.Register;
				instruction.Op2Register = (int)(decoder.state.rm + decoder.state.zs.extraBaseRegisterBase) + baseReg3;
			}
			else {
				instruction.Op2Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
			}
			instruction.Op3Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadByte();
		}
	}

	sealed class OpCodeHandler_VEX_HRIb : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code code;

		public OpCodeHandler_VEX_HRIb(Register baseReg, Code code) {
			this.baseReg = baseReg;
			this.code = code;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.VEX || decoder.state.Encoding == EncodingKind.XOP);
			instruction.InternalSetCodeNoCheck(code);
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = (int)decoder.state.vvvv + baseReg;
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op1Kind = OpKind.Register;
				instruction.Op1Register = (int)(decoder.state.rm + decoder.state.zs.extraBaseRegisterBase) + baseReg;
			}
			else
				decoder.SetInvalidInstruction();
			instruction.Op2Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadByte();
		}
	}

	sealed class OpCodeHandler_VEX_VHWIs4 : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code code;

		public OpCodeHandler_VEX_VHWIs4(Register baseReg, Code code) {
			this.baseReg = baseReg;
			this.code = code;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.VEX || decoder.state.Encoding == EncodingKind.XOP);
			instruction.InternalSetCodeNoCheck(code);
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase) + baseReg;
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op1Kind = OpKind.Register;
			instruction.Op1Register = (int)decoder.state.vvvv + baseReg;
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op2Kind = OpKind.Register;
				instruction.Op2Register = (int)(decoder.state.rm + decoder.state.zs.extraBaseRegisterBase) + baseReg;
			}
			else {
				instruction.Op2Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
			}
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op3Kind = OpKind.Register;
			instruction.Op3Register = (int)((decoder.ReadByte() >> 4) & decoder.reg15Mask) + baseReg;
		}
	}

	sealed class OpCodeHandler_VEX_VHIs4W : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code code;

		public OpCodeHandler_VEX_VHIs4W(Register baseReg, Code code) {
			this.baseReg = baseReg;
			this.code = code;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.VEX || decoder.state.Encoding == EncodingKind.XOP);
			instruction.InternalSetCodeNoCheck(code);
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase) + baseReg;
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op1Kind = OpKind.Register;
			instruction.Op1Register = (int)decoder.state.vvvv + baseReg;
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op3Kind = OpKind.Register;
				instruction.Op3Register = (int)(decoder.state.rm + decoder.state.zs.extraBaseRegisterBase) + baseReg;
			}
			else {
				instruction.Op3Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
			}
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op2Kind = OpKind.Register;
			instruction.Op2Register = (int)((decoder.ReadByte() >> 4) & decoder.reg15Mask) + baseReg;
		}
	}

	sealed class OpCodeHandler_VEX_VHWIs5 : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code code;

		public OpCodeHandler_VEX_VHWIs5(Register baseReg, Code code) {
			this.baseReg = baseReg;
			this.code = code;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.VEX || decoder.state.Encoding == EncodingKind.XOP);
			instruction.InternalSetCodeNoCheck(code);
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase) + baseReg;
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op1Kind = OpKind.Register;
			instruction.Op1Register = (int)decoder.state.vvvv + baseReg;
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op2Kind = OpKind.Register;
				instruction.Op2Register = (int)(decoder.state.rm + decoder.state.zs.extraBaseRegisterBase) + baseReg;
			}
			else {
				instruction.Op2Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
			}
			uint ib = decoder.ReadByte();
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op3Kind = OpKind.Register;
			instruction.Op3Register = (int)((ib >> 4) & decoder.reg15Mask) + baseReg;
			Debug.Assert(instruction.Op4Kind == OpKind.Immediate8);// It's hard coded
			instruction.InternalImmediate8 = ib & 0xF;
		}
	}

	sealed class OpCodeHandler_VEX_VHIs5W : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code code;

		public OpCodeHandler_VEX_VHIs5W(Register baseReg, Code code) {
			this.baseReg = baseReg;
			this.code = code;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.VEX || decoder.state.Encoding == EncodingKind.XOP);
			instruction.InternalSetCodeNoCheck(code);
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase) + baseReg;
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op1Kind = OpKind.Register;
			instruction.Op1Register = (int)decoder.state.vvvv + baseReg;
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op3Kind = OpKind.Register;
				instruction.Op3Register = (int)(decoder.state.rm + decoder.state.zs.extraBaseRegisterBase) + baseReg;
			}
			else {
				instruction.Op3Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
			}
			uint ib = decoder.ReadByte();
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op2Kind = OpKind.Register;
			instruction.Op2Register = (int)((ib >> 4) & decoder.reg15Mask) + baseReg;
			Debug.Assert(instruction.Op4Kind == OpKind.Immediate8);// It's hard coded
			instruction.InternalImmediate8 = ib & 0xF;
		}
	}

	sealed class OpCodeHandler_VEX_VK_HK_RK : OpCodeHandlerModRM {
		readonly Code code;

		public OpCodeHandler_VEX_VK_HK_RK(Code code) => this.code = code;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.VEX || decoder.state.Encoding == EncodingKind.XOP);
			if (decoder.invalidCheckMask != 0 && (decoder.state.vvvv > 7 || decoder.state.zs.extraRegisterBase != 0))
				decoder.SetInvalidInstruction();
			instruction.InternalSetCodeNoCheck(code);
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = (int)decoder.state.reg + Register.K0;
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op1Kind = OpKind.Register;
			instruction.Op1Register = (int)(decoder.state.vvvv & 7) + Register.K0;
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op2Kind = OpKind.Register;
				instruction.Op2Register = (int)decoder.state.rm + Register.K0;
			}
			else
				decoder.SetInvalidInstruction();
		}
	}

	sealed class OpCodeHandler_VEX_VK_RK : OpCodeHandlerModRM {
		readonly Code code;

		public OpCodeHandler_VEX_VK_RK(Code code) => this.code = code;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.VEX || decoder.state.Encoding == EncodingKind.XOP);
			if (((decoder.state.vvvv_invalidCheck | decoder.state.zs.extraRegisterBase) & decoder.invalidCheckMask) != 0)
				decoder.SetInvalidInstruction();
			instruction.InternalSetCodeNoCheck(code);
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = (int)decoder.state.reg + Register.K0;
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op1Kind = OpKind.Register;
				instruction.Op1Register = (int)decoder.state.rm + Register.K0;
			}
			else
				decoder.SetInvalidInstruction();
		}
	}

	sealed class OpCodeHandler_VEX_VK_RK_Ib : OpCodeHandlerModRM {
		readonly Code code;

		public OpCodeHandler_VEX_VK_RK_Ib(Code code) => this.code = code;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.VEX || decoder.state.Encoding == EncodingKind.XOP);
			if (((decoder.state.vvvv_invalidCheck | decoder.state.zs.extraRegisterBase) & decoder.invalidCheckMask) != 0)
				decoder.SetInvalidInstruction();
			instruction.InternalSetCodeNoCheck(code);
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = (int)decoder.state.reg + Register.K0;
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op1Kind = OpKind.Register;
				instruction.Op1Register = (int)decoder.state.rm + Register.K0;
			}
			else
				decoder.SetInvalidInstruction();
			instruction.Op2Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadByte();
		}
	}

	sealed class OpCodeHandler_VEX_VK_WK : OpCodeHandlerModRM {
		readonly Code code;

		public OpCodeHandler_VEX_VK_WK(Code code) => this.code = code;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.VEX || decoder.state.Encoding == EncodingKind.XOP);
			if (((decoder.state.vvvv_invalidCheck | decoder.state.zs.extraRegisterBase) & decoder.invalidCheckMask) != 0)
				decoder.SetInvalidInstruction();
			instruction.InternalSetCodeNoCheck(code);
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = (int)decoder.state.reg + Register.K0;
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op1Kind = OpKind.Register;
				instruction.Op1Register = (int)decoder.state.rm + Register.K0;
			}
			else {
				instruction.Op1Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_VEX_M_VK : OpCodeHandlerModRM {
		readonly Code code;

		public OpCodeHandler_VEX_M_VK(Code code) => this.code = code;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.VEX || decoder.state.Encoding == EncodingKind.XOP);
			if (((decoder.state.vvvv_invalidCheck | decoder.state.zs.extraRegisterBase) & decoder.invalidCheckMask) != 0)
				decoder.SetInvalidInstruction();
			instruction.InternalSetCodeNoCheck(code);
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op1Kind = OpKind.Register;
			instruction.Op1Register = (int)decoder.state.reg + Register.K0;
			if (decoder.state.mod == 3)
				decoder.SetInvalidInstruction();
			else {
				instruction.Op0Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_VEX_VK_R : OpCodeHandlerModRM {
		readonly Code code;
		readonly Register gpr;

		public OpCodeHandler_VEX_VK_R(Code code, Register gpr) {
			this.code = code;
			this.gpr = gpr;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.VEX || decoder.state.Encoding == EncodingKind.XOP);
			if (((decoder.state.vvvv_invalidCheck | decoder.state.zs.extraRegisterBase) & decoder.invalidCheckMask) != 0)
				decoder.SetInvalidInstruction();
			instruction.InternalSetCodeNoCheck(code);
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = (int)decoder.state.reg + Register.K0;
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op1Kind = OpKind.Register;
				instruction.Op1Register = (int)(decoder.state.rm + decoder.state.zs.extraBaseRegisterBase) + gpr;
			}
			else
				decoder.SetInvalidInstruction();
		}
	}

	sealed class OpCodeHandler_VEX_G_VK : OpCodeHandlerModRM {
		readonly Code code;
		readonly Register gpr;

		public OpCodeHandler_VEX_G_VK(Code code, Register gpr) {
			this.code = code;
			this.gpr = gpr;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.VEX || decoder.state.Encoding == EncodingKind.XOP);
			if ((decoder.state.vvvv_invalidCheck & decoder.invalidCheckMask) != 0)
				decoder.SetInvalidInstruction();
			instruction.InternalSetCodeNoCheck(code);
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase) + gpr;
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op1Kind = OpKind.Register;
				instruction.Op1Register = (int)decoder.state.rm + Register.K0;
			}
			else
				decoder.SetInvalidInstruction();
		}
	}

	sealed class OpCodeHandler_VEX_Gv_W : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code codeW0;
		readonly Code codeW1;

		public OpCodeHandler_VEX_Gv_W(Register baseReg, Code codeW0, Code codeW1) {
			this.baseReg = baseReg;
			this.codeW0 = codeW0;
			this.codeW1 = codeW1;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.VEX || decoder.state.Encoding == EncodingKind.XOP);
			if ((decoder.state.vvvv_invalidCheck & decoder.invalidCheckMask) != 0)
				decoder.SetInvalidInstruction();
			if (((uint)decoder.state.zs.flags & decoder.is64bMode_and_W) != 0) {
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
				instruction.Op1Register = (int)(decoder.state.rm + decoder.state.zs.extraBaseRegisterBase) + baseReg;
			}
			else {
				instruction.Op1Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_VEX_Gv_RX : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_VEX_Gv_RX(Register baseReg, Code code32, Code code64) {
			this.baseReg = baseReg;
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.VEX || decoder.state.Encoding == EncodingKind.XOP);
			if ((decoder.state.vvvv_invalidCheck & decoder.invalidCheckMask) != 0)
				decoder.SetInvalidInstruction();
			Register gpr;
			if (((uint)decoder.state.zs.flags & decoder.is64bMode_and_W) != 0) {
				instruction.InternalSetCodeNoCheck(code64);
				gpr = Register.RAX;
			}
			else {
				instruction.InternalSetCodeNoCheck(code32);
				gpr = Register.EAX;
			}
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase) + gpr;
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op1Kind = OpKind.Register;
				instruction.Op1Register = (int)(decoder.state.rm + decoder.state.zs.extraBaseRegisterBase) + baseReg;
			}
			else
				decoder.SetInvalidInstruction();
		}
	}

	sealed class OpCodeHandler_VEX_Gv_GPR_Ib : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_VEX_Gv_GPR_Ib(Register baseReg, Code code32, Code code64) {
			this.baseReg = baseReg;
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.VEX || decoder.state.Encoding == EncodingKind.XOP);
			if ((decoder.state.vvvv_invalidCheck & decoder.invalidCheckMask) != 0)
				decoder.SetInvalidInstruction();
			Register gpr;
			if (((uint)decoder.state.zs.flags & decoder.is64bMode_and_W) != 0) {
				instruction.InternalSetCodeNoCheck(code64);
				gpr = Register.RAX;
			}
			else {
				instruction.InternalSetCodeNoCheck(code32);
				gpr = Register.EAX;
			}
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase) + gpr;
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op1Kind = OpKind.Register;
				instruction.Op1Register = (int)(decoder.state.rm + decoder.state.zs.extraBaseRegisterBase) + baseReg;
			}
			else
				decoder.SetInvalidInstruction();
			instruction.Op2Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadByte();
		}
	}

	sealed class OpCodeHandler_VEX_VX_VSIB_HX : OpCodeHandlerModRM {
		readonly Register baseReg1;
		readonly Register vsibIndex;
		readonly Register baseReg3;
		readonly Code code;

		public OpCodeHandler_VEX_VX_VSIB_HX(Register baseReg1, Register vsibIndex, Register baseReg3, Code code) {
			this.baseReg1 = baseReg1;
			this.vsibIndex = vsibIndex;
			this.baseReg3 = baseReg3;
			this.code = code;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.VEX || decoder.state.Encoding == EncodingKind.XOP);
			instruction.InternalSetCodeNoCheck(code);
			int regNum = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase);
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = regNum + baseReg1;
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op2Kind = OpKind.Register;
			instruction.Op2Register = (int)decoder.state.vvvv + baseReg3;
			if (decoder.state.mod == 3)
				decoder.SetInvalidInstruction();
			else {
				instruction.Op1Kind = OpKind.Memory;
				decoder.ReadOpMem_VSIB(ref instruction, vsibIndex, TupleType.N1);
				if (decoder.invalidCheckMask != 0) {
					uint indexNum = ((uint)(instruction.MemoryIndex - Register.XMM0) % (uint)IcedConstants.VMM_count);
					if ((uint)regNum == indexNum || decoder.state.vvvv == indexNum || (uint)regNum == decoder.state.vvvv)
						decoder.SetInvalidInstruction();
				}
			}
		}
	}

	sealed class OpCodeHandler_VEX_Gv_Gv_Ev : OpCodeHandlerModRM {
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_VEX_Gv_Gv_Ev(Code code32, Code code64) {
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.VEX || decoder.state.Encoding == EncodingKind.XOP);
			Register gpr;
			if (((uint)decoder.state.zs.flags & decoder.is64bMode_and_W) != 0) {
				instruction.InternalSetCodeNoCheck(code64);
				gpr = Register.RAX;
			}
			else {
				instruction.InternalSetCodeNoCheck(code32);
				gpr = Register.EAX;
			}
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase) + gpr;
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op1Kind = OpKind.Register;
			instruction.Op1Register = (int)decoder.state.vvvv + gpr;
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op2Kind = OpKind.Register;
				instruction.Op2Register = (int)(decoder.state.rm + decoder.state.zs.extraBaseRegisterBase) + gpr;
			}
			else {
				instruction.Op2Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_VEX_Gv_Ev_Gv : OpCodeHandlerModRM {
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_VEX_Gv_Ev_Gv(Code code32, Code code64) {
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.VEX || decoder.state.Encoding == EncodingKind.XOP);
			Register gpr;
			if (((uint)decoder.state.zs.flags & decoder.is64bMode_and_W) != 0) {
				instruction.InternalSetCodeNoCheck(code64);
				gpr = Register.RAX;
			}
			else {
				instruction.InternalSetCodeNoCheck(code32);
				gpr = Register.EAX;
			}
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase) + gpr;
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op2Kind = OpKind.Register;
			instruction.Op2Register = (int)decoder.state.vvvv + gpr;
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

	sealed class OpCodeHandler_VEX_Ev_Gv_Gv : OpCodeHandlerModRM {
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_VEX_Ev_Gv_Gv(Code code32, Code code64) {
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.VEX || decoder.state.Encoding == EncodingKind.XOP);
			Register gpr;
			if (((uint)decoder.state.zs.flags & decoder.is64bMode_and_W) != 0) {
				instruction.InternalSetCodeNoCheck(code64);
				gpr = Register.RAX;
			}
			else {
				instruction.InternalSetCodeNoCheck(code32);
				gpr = Register.EAX;
			}
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op1Kind = OpKind.Register;
			instruction.Op1Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase) + gpr;
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op2Kind = OpKind.Register;
			instruction.Op2Register = (int)decoder.state.vvvv + gpr;
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

	sealed class OpCodeHandler_VEX_Hv_Ev : OpCodeHandlerModRM {
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_VEX_Hv_Ev(Code code32, Code code64) {
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.VEX || decoder.state.Encoding == EncodingKind.XOP);
			Register gpr;
			if (((uint)decoder.state.zs.flags & decoder.is64bMode_and_W) != 0) {
				instruction.InternalSetCodeNoCheck(code64);
				gpr = Register.RAX;
			}
			else {
				instruction.InternalSetCodeNoCheck(code32);
				gpr = Register.EAX;
			}
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = (int)decoder.state.vvvv + gpr;
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

	sealed class OpCodeHandler_VEX_Hv_Ed_Id : OpCodeHandlerModRM {
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_VEX_Hv_Ed_Id(Code code32, Code code64) {
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.VEX || decoder.state.Encoding == EncodingKind.XOP);
			if (((uint)decoder.state.zs.flags & decoder.is64bMode_and_W) != 0) {
				instruction.InternalSetCodeNoCheck(code64);
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op0Kind = OpKind.Register;
				instruction.Op0Register = (int)decoder.state.vvvv + Register.RAX;
			}
			else {
				instruction.InternalSetCodeNoCheck(code32);
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op0Kind = OpKind.Register;
				instruction.Op0Register = (int)decoder.state.vvvv + Register.EAX;
			}
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op1Kind = OpKind.Register;
				instruction.Op1Register = (int)(decoder.state.rm + decoder.state.zs.extraBaseRegisterBase) + Register.EAX;
			}
			else {
				instruction.Op1Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
			}
			instruction.Op2Kind = OpKind.Immediate32;
			instruction.Immediate32 = decoder.ReadUInt32();
		}
	}

	sealed class OpCodeHandler_VEX_GvM_VX_Ib : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_VEX_GvM_VX_Ib(Register baseReg, Code code32, Code code64) {
			this.baseReg = baseReg;
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.VEX || decoder.state.Encoding == EncodingKind.XOP);
			if ((decoder.state.vvvv_invalidCheck & decoder.invalidCheckMask) != 0)
				decoder.SetInvalidInstruction();
			Register gpr;
			if (((uint)decoder.state.zs.flags & decoder.is64bMode_and_W) != 0) {
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
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op1Kind = OpKind.Register;
			instruction.Op1Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase) + baseReg;
			instruction.Op2Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadByte();
		}
	}

	sealed class OpCodeHandler_VEX_Gv_Ev_Ib : OpCodeHandlerModRM {
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_VEX_Gv_Ev_Ib(Code code32, Code code64) {
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.VEX || decoder.state.Encoding == EncodingKind.XOP);
			if ((decoder.state.vvvv_invalidCheck & decoder.invalidCheckMask) != 0)
				decoder.SetInvalidInstruction();
			Register gpr;
			if (((uint)decoder.state.zs.flags & decoder.is64bMode_and_W) != 0) {
				instruction.InternalSetCodeNoCheck(code64);
				gpr = Register.RAX;
			}
			else {
				instruction.InternalSetCodeNoCheck(code32);
				gpr = Register.EAX;
			}
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase) + gpr;
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

	sealed class OpCodeHandler_VEX_Gv_Ev_Id : OpCodeHandlerModRM {
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_VEX_Gv_Ev_Id(Code code32, Code code64) {
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.VEX || decoder.state.Encoding == EncodingKind.XOP);
			if ((decoder.state.vvvv_invalidCheck & decoder.invalidCheckMask) != 0)
				decoder.SetInvalidInstruction();
			Register gpr;
			if (((uint)decoder.state.zs.flags & decoder.is64bMode_and_W) != 0) {
				instruction.InternalSetCodeNoCheck(code64);
				gpr = Register.RAX;
			}
			else {
				instruction.InternalSetCodeNoCheck(code32);
				gpr = Register.EAX;
			}
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase) + gpr;
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op1Kind = OpKind.Register;
				instruction.Op1Register = (int)(decoder.state.rm + decoder.state.zs.extraBaseRegisterBase) + gpr;
			}
			else {
				instruction.Op1Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction);
			}
			instruction.Op2Kind = OpKind.Immediate32;
			instruction.Immediate32 = decoder.ReadUInt32();
		}
	}

	sealed class OpCodeHandler_VEX_VT_SIBMEM : OpCodeHandlerModRM {
		readonly Code code;

		public OpCodeHandler_VEX_VT_SIBMEM(Code code) => this.code = code;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.VEX || decoder.state.Encoding == EncodingKind.XOP);
			if (((decoder.state.vvvv_invalidCheck | decoder.state.zs.extraRegisterBase) & decoder.invalidCheckMask) != 0)
				decoder.SetInvalidInstruction();
			instruction.InternalSetCodeNoCheck(code);
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = (int)decoder.state.reg + Register.TMM0;
			if (decoder.state.mod == 3)
				decoder.SetInvalidInstruction();
			else {
				instruction.Op1Kind = OpKind.Memory;
				decoder.ReadOpMemSib(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_VEX_SIBMEM_VT : OpCodeHandlerModRM {
		readonly Code code;

		public OpCodeHandler_VEX_SIBMEM_VT(Code code) => this.code = code;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.VEX || decoder.state.Encoding == EncodingKind.XOP);
			if (((decoder.state.vvvv_invalidCheck | decoder.state.zs.extraRegisterBase) & decoder.invalidCheckMask) != 0)
				decoder.SetInvalidInstruction();
			instruction.InternalSetCodeNoCheck(code);
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op1Kind = OpKind.Register;
			instruction.Op1Register = (int)decoder.state.reg + Register.TMM0;
			if (decoder.state.mod == 3)
				decoder.SetInvalidInstruction();
			else {
				instruction.Op0Kind = OpKind.Memory;
				decoder.ReadOpMemSib(ref instruction);
			}
		}
	}

	sealed class OpCodeHandler_VEX_VT : OpCodeHandlerModRM {
		readonly Code code;

		public OpCodeHandler_VEX_VT(Code code) => this.code = code;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.VEX || decoder.state.Encoding == EncodingKind.XOP);
			if (((decoder.state.vvvv_invalidCheck | decoder.state.zs.extraRegisterBase) & decoder.invalidCheckMask) != 0)
				decoder.SetInvalidInstruction();
			instruction.InternalSetCodeNoCheck(code);
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = (int)decoder.state.reg + Register.TMM0;
		}
	}

	sealed class OpCodeHandler_VEX_VT_RT_HT : OpCodeHandlerModRM {
		readonly Code code;

		public OpCodeHandler_VEX_VT_RT_HT(Code code) => this.code = code;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.VEX || decoder.state.Encoding == EncodingKind.XOP);
			if (decoder.invalidCheckMask != 0 && (decoder.state.vvvv > 7 || decoder.state.zs.extraRegisterBase != 0))
				decoder.SetInvalidInstruction();
			instruction.InternalSetCodeNoCheck(code);
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = (int)decoder.state.reg + Register.TMM0;
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op2Kind = OpKind.Register;
			instruction.Op2Register = (int)(decoder.state.vvvv & 7) + Register.TMM0;
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op1Kind = OpKind.Register;
				instruction.Op1Register = (int)decoder.state.rm + Register.TMM0;
				if (decoder.invalidCheckMask != 0) {
					if (decoder.state.zs.extraBaseRegisterBase != 0 || decoder.state.reg == decoder.state.vvvv || decoder.state.reg == decoder.state.rm || decoder.state.rm == decoder.state.vvvv)
						decoder.SetInvalidInstruction();
				}
			}
			else
				decoder.SetInvalidInstruction();
		}
	}

	sealed class OpCodeHandler_VEX_Gq_HK_RK : OpCodeHandlerModRM {
		readonly Code code;

		public OpCodeHandler_VEX_Gq_HK_RK(Code code) => this.code = code;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.VEX || decoder.state.Encoding == EncodingKind.XOP);
			if (decoder.invalidCheckMask != 0 && decoder.state.vvvv > 7)
				decoder.SetInvalidInstruction();
			instruction.InternalSetCodeNoCheck(code);
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase) + Register.RAX;
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op1Kind = OpKind.Register;
			instruction.Op1Register = (int)(decoder.state.vvvv & 7) + Register.K0;
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op2Kind = OpKind.Register;
				instruction.Op2Register = (int)decoder.state.rm + Register.K0;
			}
			else
				decoder.SetInvalidInstruction();
		}
	}

	sealed class OpCodeHandler_VEX_VK_R_Ib : OpCodeHandlerModRM {
		readonly Code code;
		readonly Register gpr;

		public OpCodeHandler_VEX_VK_R_Ib(Code code, Register gpr) {
			this.code = code;
			this.gpr = gpr;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.VEX || decoder.state.Encoding == EncodingKind.XOP);
			if (((decoder.state.vvvv_invalidCheck | decoder.state.zs.extraRegisterBase) & decoder.invalidCheckMask) != 0)
				decoder.SetInvalidInstruction();
			instruction.InternalSetCodeNoCheck(code);
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = (int)decoder.state.reg + Register.K0;
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op1Kind = OpKind.Register;
				instruction.Op1Register = (int)(decoder.state.rm + decoder.state.zs.extraBaseRegisterBase) + gpr;
			}
			else
				decoder.SetInvalidInstruction();
			instruction.Op2Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadByte();
		}
	}

	sealed class OpCodeHandler_VEX_K_Jb : OpCodeHandlerModRM {
		readonly Code code;

		public OpCodeHandler_VEX_K_Jb(Code code) => this.code = code;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.VEX || decoder.state.Encoding == EncodingKind.XOP);
			decoder.state.zs.flags |= StateFlags.BranchImm8;
			if (decoder.invalidCheckMask != 0 && decoder.state.vvvv > 7)
				decoder.SetInvalidInstruction();
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = (int)(decoder.state.vvvv & 7) + Register.K0;
			Debug.Assert(decoder.is64bMode);
			instruction.InternalSetCodeNoCheck(code);
			instruction.Op1Kind = OpKind.NearBranch64;
			// The modrm byte has the imm8 value
			instruction.NearBranch64 = (ulong)(sbyte)decoder.state.modrm + decoder.GetCurrentInstructionPointer64();
		}
	}

	sealed class OpCodeHandler_VEX_K_Jz : OpCodeHandlerModRM {
		readonly Code code;

		public OpCodeHandler_VEX_K_Jz(Code code) => this.code = code;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.VEX || decoder.state.Encoding == EncodingKind.XOP);
			if (decoder.invalidCheckMask != 0 && decoder.state.vvvv > 7)
				decoder.SetInvalidInstruction();
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = (int)(decoder.state.vvvv & 7) + Register.K0;
			Debug.Assert(decoder.is64bMode);
			instruction.InternalSetCodeNoCheck(code);
			instruction.Op1Kind = OpKind.NearBranch64;
			// The modrm byte has the low 8 bits of imm32
			uint imm = decoder.state.modrm | (decoder.ReadByte() << 8) | (decoder.ReadByte() << 16) | (decoder.ReadByte() << 24);
			instruction.NearBranch64 = (ulong)(int)imm + decoder.GetCurrentInstructionPointer64();
		}
	}

	sealed class OpCodeHandler_VEX_Gv_Ev : OpCodeHandlerModRM {
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_VEX_Gv_Ev(Code code32, Code code64) {
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.VEX || decoder.state.Encoding == EncodingKind.XOP);
			if ((decoder.state.vvvv_invalidCheck & decoder.invalidCheckMask) != 0)
				decoder.SetInvalidInstruction();
			Register gpr;
			if (((uint)decoder.state.zs.flags & decoder.is64bMode_and_W) != 0) {
				instruction.InternalSetCodeNoCheck(code64);
				gpr = Register.RAX;
			}
			else {
				instruction.InternalSetCodeNoCheck(code32);
				gpr = Register.EAX;
			}
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase) + gpr;
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

	sealed class OpCodeHandler_VEX_Ev : OpCodeHandlerModRM {
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_VEX_Ev(Code code32, Code code64) {
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.VEX || decoder.state.Encoding == EncodingKind.XOP);
			if ((decoder.state.vvvv_invalidCheck & decoder.invalidCheckMask) != 0)
				decoder.SetInvalidInstruction();
			Register gpr;
			if (((uint)decoder.state.zs.flags & decoder.is64bMode_and_W) != 0) {
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
}
#endif
