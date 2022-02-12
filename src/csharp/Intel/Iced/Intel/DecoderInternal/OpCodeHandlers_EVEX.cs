// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if DECODER && !NO_EVEX
using System;
using System.Diagnostics;

namespace Iced.Intel.DecoderInternal {
	sealed class OpCodeHandler_VectorLength_EVEX : OpCodeHandlerModRM {
		readonly OpCodeHandler[] handlers;

		public OpCodeHandler_VectorLength_EVEX(OpCodeHandler handler128, OpCodeHandler handler256, OpCodeHandler handler512) {
			Static.Assert((int)VectorLength.L128 == 0 ? 0 : -1);
			Static.Assert((int)VectorLength.L256 == 1 ? 0 : -1);
			Static.Assert((int)VectorLength.L512 == 2 ? 0 : -1);
			Static.Assert((int)VectorLength.Unknown == 3 ? 0 : -1);
			handlers = new OpCodeHandler[4] {
				handler128 ?? throw new ArgumentNullException(nameof(handler128)),
				handler256 ?? throw new ArgumentNullException(nameof(handler256)),
				handler512 ?? throw new ArgumentNullException(nameof(handler512)),
				OpCodeHandler_Invalid.Instance,
			};
			Debug.Assert(handler128.HasModRM == HasModRM);
			Debug.Assert(handler256.HasModRM == HasModRM);
			Debug.Assert(handler512.HasModRM == HasModRM);
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.EVEX);
			handlers[(int)decoder.state.vectorLength].Decode(decoder, ref instruction);
		}
	}

	sealed class OpCodeHandler_VectorLength_EVEX_er : OpCodeHandlerModRM {
		readonly OpCodeHandler[] handlers;

		public OpCodeHandler_VectorLength_EVEX_er(OpCodeHandler handler128, OpCodeHandler handler256, OpCodeHandler handler512) {
			Static.Assert((int)VectorLength.L128 == 0 ? 0 : -1);
			Static.Assert((int)VectorLength.L256 == 1 ? 0 : -1);
			Static.Assert((int)VectorLength.L512 == 2 ? 0 : -1);
			Static.Assert((int)VectorLength.Unknown == 3 ? 0 : -1);
			handlers = new OpCodeHandler[4] {
				handler128 ?? throw new ArgumentNullException(nameof(handler128)),
				handler256 ?? throw new ArgumentNullException(nameof(handler256)),
				handler512 ?? throw new ArgumentNullException(nameof(handler512)),
				OpCodeHandler_Invalid.Instance,
			};
			Debug.Assert(handler128.HasModRM == HasModRM);
			Debug.Assert(handler256.HasModRM == HasModRM);
			Debug.Assert(handler512.HasModRM == HasModRM);
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.EVEX);
			int index = (int)decoder.state.vectorLength;
			if (decoder.state.mod == 3 && (decoder.state.zs.flags & StateFlags.b) != 0)
				index = (int)VectorLength.L512;
			handlers[index].Decode(decoder, ref instruction);
		}
	}

	sealed class OpCodeHandler_EVEX_V_H_Ev_er : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code codeW0;
		readonly Code codeW1;
		readonly TupleType tupleTypeW0;
		readonly TupleType tupleTypeW1;

		public OpCodeHandler_EVEX_V_H_Ev_er(Register baseReg, Code codeW0, Code codeW1, TupleType tupleTypeW0, TupleType tupleTypeW1) {
			this.baseReg = baseReg;
			this.codeW0 = codeW0;
			this.codeW1 = codeW1;
			this.tupleTypeW0 = tupleTypeW0;
			this.tupleTypeW1 = tupleTypeW1;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.EVEX);
			if ((((uint)(decoder.state.zs.flags & StateFlags.z) | decoder.state.aaa) & decoder.invalidCheckMask) != 0)
				decoder.SetInvalidInstruction();
			Register gpr;
			TupleType tupleType;
			if (((uint)decoder.state.zs.flags & decoder.is64bMode_and_W) != 0) {
				instruction.InternalSetCodeNoCheck(codeW1);
				tupleType = tupleTypeW1;
				gpr = Register.RAX;
			}
			else {
				instruction.InternalSetCodeNoCheck(codeW0);
				tupleType = tupleTypeW0;
				gpr = Register.EAX;
			}
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase + decoder.state.extraRegisterBaseEVEX) + baseReg;
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op1Kind = OpKind.Register;
			instruction.Op1Register = (int)decoder.state.vvvv + baseReg;
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op2Kind = OpKind.Register;
				instruction.Op2Register = (int)(decoder.state.rm + decoder.state.zs.extraBaseRegisterBase) + gpr;
				if ((decoder.state.zs.flags & StateFlags.b) != 0) {
					Static.Assert((int)RoundingControl.None == 0 ? 0 : -1);
					Static.Assert((int)RoundingControl.RoundToNearest == 1 ? 0 : -1);
					Static.Assert((int)RoundingControl.RoundDown == 2 ? 0 : -1);
					Static.Assert((int)RoundingControl.RoundUp == 3 ? 0 : -1);
					Static.Assert((int)RoundingControl.RoundTowardZero == 4 ? 0 : -1);
					instruction.InternalRoundingControl = decoder.state.vectorLength + (uint)RoundingControl.RoundToNearest;
				}
			}
			else {
				if (((uint)(decoder.state.zs.flags & StateFlags.b) & decoder.invalidCheckMask) != 0)
					decoder.SetInvalidInstruction();
				instruction.Op2Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction, tupleType);
			}
		}
	}

	sealed class OpCodeHandler_EVEX_V_H_Ev_Ib : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code codeW0;
		readonly Code codeW1;
		readonly TupleType tupleTypeW0;
		readonly TupleType tupleTypeW1;

		public OpCodeHandler_EVEX_V_H_Ev_Ib(Register baseReg, Code codeW0, Code codeW1, TupleType tupleTypeW0, TupleType tupleTypeW1) {
			this.baseReg = baseReg;
			this.codeW0 = codeW0;
			this.codeW1 = codeW1;
			this.tupleTypeW0 = tupleTypeW0;
			this.tupleTypeW1 = tupleTypeW1;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.EVEX);
			if ((((uint)(decoder.state.zs.flags & (StateFlags.b | StateFlags.z)) | decoder.state.aaa) & decoder.invalidCheckMask) != 0)
				decoder.SetInvalidInstruction();
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
			instruction.Op0Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase + decoder.state.extraRegisterBaseEVEX) + baseReg;
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
				if (((uint)decoder.state.zs.flags & decoder.is64bMode_and_W) != 0)
					decoder.ReadOpMem(ref instruction, tupleTypeW1);
				else
					decoder.ReadOpMem(ref instruction, tupleTypeW0);
			}
			instruction.Op3Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadByte();
		}
	}

	sealed class OpCodeHandler_EVEX_Ed_V_Ib : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code code32;
		readonly Code code64;
		readonly TupleType tupleType32;
		readonly TupleType tupleType64;

		public OpCodeHandler_EVEX_Ed_V_Ib(Register baseReg, Code code32, Code code64, TupleType tupleType32, TupleType tupleType64) {
			this.baseReg = baseReg;
			this.code32 = code32;
			this.code64 = code64;
			this.tupleType32 = tupleType32;
			this.tupleType64 = tupleType64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.EVEX);
			if ((((uint)(decoder.state.zs.flags & (StateFlags.b | StateFlags.z)) | decoder.state.vvvv_invalidCheck | decoder.state.aaa) & decoder.invalidCheckMask) != 0)
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
				if (((uint)decoder.state.zs.flags & decoder.is64bMode_and_W) != 0)
					decoder.ReadOpMem(ref instruction, tupleType64);
				else
					decoder.ReadOpMem(ref instruction, tupleType32);
			}
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op1Kind = OpKind.Register;
			instruction.Op1Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase + decoder.state.extraRegisterBaseEVEX) + baseReg;
			instruction.Op2Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadByte();
		}
	}

	sealed class OpCodeHandler_EVEX_VkHW_er : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code code;
		readonly TupleType tupleType;
		readonly bool onlySAE;
		readonly bool canBroadcast;

		public OpCodeHandler_EVEX_VkHW_er(Register baseReg, Code code, TupleType tupleType, bool onlySAE, bool canBroadcast) {
			this.baseReg = baseReg;
			this.code = code;
			this.tupleType = tupleType;
			this.onlySAE = onlySAE;
			this.canBroadcast = canBroadcast;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.EVEX);
			instruction.InternalSetCodeNoCheck(code);

			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase + decoder.state.extraRegisterBaseEVEX) + baseReg;
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op1Kind = OpKind.Register;
			instruction.Op1Register = (int)decoder.state.vvvv + baseReg;
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op2Kind = OpKind.Register;
				instruction.Op2Register = (int)(decoder.state.rm + decoder.state.extraBaseRegisterBaseEVEX) + baseReg;
				if ((decoder.state.zs.flags & StateFlags.b) != 0) {
					if (onlySAE)
						instruction.InternalSetSuppressAllExceptions();
					else {
						Static.Assert((int)RoundingControl.None == 0 ? 0 : -1);
						Static.Assert((int)RoundingControl.RoundToNearest == 1 ? 0 : -1);
						Static.Assert((int)RoundingControl.RoundDown == 2 ? 0 : -1);
						Static.Assert((int)RoundingControl.RoundUp == 3 ? 0 : -1);
						Static.Assert((int)RoundingControl.RoundTowardZero == 4 ? 0 : -1);
						instruction.InternalRoundingControl = decoder.state.vectorLength + (uint)RoundingControl.RoundToNearest;
					}
				}
			}
			else {
				instruction.Op2Kind = OpKind.Memory;
				if ((decoder.state.zs.flags & StateFlags.b) != 0) {
					if (canBroadcast)
						instruction.InternalSetIsBroadcast();
					else if (decoder.invalidCheckMask != 0)
						decoder.SetInvalidInstruction();
				}
				decoder.ReadOpMem(ref instruction, tupleType);
			}
		}
	}

	sealed class OpCodeHandler_EVEX_VkHW_er_ur : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code code;
		readonly TupleType tupleType;
		readonly bool canBroadcast;

		public OpCodeHandler_EVEX_VkHW_er_ur(Register baseReg, Code code, TupleType tupleType, bool canBroadcast) {
			this.baseReg = baseReg;
			this.code = code;
			this.tupleType = tupleType;
			this.canBroadcast = canBroadcast;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.EVEX);
			instruction.InternalSetCodeNoCheck(code);

			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			int regNum0 = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase + decoder.state.extraRegisterBaseEVEX);
			instruction.Op0Register = regNum0 + baseReg;
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op1Kind = OpKind.Register;
			instruction.Op1Register = (int)decoder.state.vvvv + baseReg;
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op2Kind = OpKind.Register;
				int regNum2 = (int)(decoder.state.rm + decoder.state.extraBaseRegisterBaseEVEX);
				instruction.Op2Register = regNum2 + baseReg;
				if (decoder.invalidCheckMask != 0 && (regNum0 == (int)decoder.state.vvvv || regNum0 == regNum2))
					decoder.SetInvalidInstruction();
				if ((decoder.state.zs.flags & StateFlags.b) != 0) {
					Static.Assert((int)RoundingControl.None == 0 ? 0 : -1);
					Static.Assert((int)RoundingControl.RoundToNearest == 1 ? 0 : -1);
					Static.Assert((int)RoundingControl.RoundDown == 2 ? 0 : -1);
					Static.Assert((int)RoundingControl.RoundUp == 3 ? 0 : -1);
					Static.Assert((int)RoundingControl.RoundTowardZero == 4 ? 0 : -1);
					instruction.InternalRoundingControl = decoder.state.vectorLength + (uint)RoundingControl.RoundToNearest;
				}
			}
			else {
				if (decoder.invalidCheckMask != 0 && regNum0 == (int)decoder.state.vvvv)
					decoder.SetInvalidInstruction();
				instruction.Op2Kind = OpKind.Memory;
				if ((decoder.state.zs.flags & StateFlags.b) != 0) {
					if (canBroadcast)
						instruction.InternalSetIsBroadcast();
					else if (decoder.invalidCheckMask != 0)
						decoder.SetInvalidInstruction();
				}
				decoder.ReadOpMem(ref instruction, tupleType);
			}
		}
	}

	sealed class OpCodeHandler_EVEX_VkW_er : OpCodeHandlerModRM {
		readonly Register baseReg1;
		readonly Register baseReg2;
		readonly Code code;
		readonly TupleType tupleType;
		readonly bool onlySAE;
		readonly bool canBroadcast;

		public OpCodeHandler_EVEX_VkW_er(Register baseReg, Code code, TupleType tupleType, bool onlySAE) {
			baseReg1 = baseReg;
			baseReg2 = baseReg;
			this.code = code;
			this.tupleType = tupleType;
			this.onlySAE = onlySAE;
			canBroadcast = true;
		}

		public OpCodeHandler_EVEX_VkW_er(Register baseReg1, Register baseReg2, Code code, TupleType tupleType, bool onlySAE) {
			this.baseReg1 = baseReg1;
			this.baseReg2 = baseReg2;
			this.code = code;
			this.tupleType = tupleType;
			this.onlySAE = onlySAE;
			canBroadcast = true;
		}

		public OpCodeHandler_EVEX_VkW_er(Register baseReg1, Register baseReg2, Code code, TupleType tupleType, bool onlySAE, bool canBroadcast) {
			this.baseReg1 = baseReg1;
			this.baseReg2 = baseReg2;
			this.code = code;
			this.tupleType = tupleType;
			this.onlySAE = onlySAE;
			this.canBroadcast = canBroadcast;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.EVEX);
			if ((decoder.state.vvvv_invalidCheck & decoder.invalidCheckMask) != 0)
				decoder.SetInvalidInstruction();
			instruction.InternalSetCodeNoCheck(code);

			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase + decoder.state.extraRegisterBaseEVEX) + baseReg1;
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op1Kind = OpKind.Register;
				instruction.Op1Register = (int)(decoder.state.rm + decoder.state.extraBaseRegisterBaseEVEX) + baseReg2;
				if ((decoder.state.zs.flags & StateFlags.b) != 0) {
					if (onlySAE)
						instruction.InternalSetSuppressAllExceptions();
					else {
						Static.Assert((int)RoundingControl.None == 0 ? 0 : -1);
						Static.Assert((int)RoundingControl.RoundToNearest == 1 ? 0 : -1);
						Static.Assert((int)RoundingControl.RoundDown == 2 ? 0 : -1);
						Static.Assert((int)RoundingControl.RoundUp == 3 ? 0 : -1);
						Static.Assert((int)RoundingControl.RoundTowardZero == 4 ? 0 : -1);
						instruction.InternalRoundingControl = decoder.state.vectorLength + (uint)RoundingControl.RoundToNearest;
					}
				}
			}
			else {
				instruction.Op1Kind = OpKind.Memory;
				if ((decoder.state.zs.flags & StateFlags.b) != 0) {
					if (canBroadcast)
						instruction.InternalSetIsBroadcast();
					else if (decoder.invalidCheckMask != 0)
						decoder.SetInvalidInstruction();
				}
				decoder.ReadOpMem(ref instruction, tupleType);
			}
		}
	}

	sealed class OpCodeHandler_EVEX_VkWIb_er : OpCodeHandlerModRM {
		readonly Register baseReg1;
		readonly Register baseReg2;
		readonly Code code;
		readonly TupleType tupleType;

		public OpCodeHandler_EVEX_VkWIb_er(Register baseReg, Code code, TupleType tupleType) {
			baseReg1 = baseReg;
			baseReg2 = baseReg;
			this.code = code;
			this.tupleType = tupleType;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.EVEX);
			if ((decoder.state.vvvv_invalidCheck & decoder.invalidCheckMask) != 0)
				decoder.SetInvalidInstruction();
			instruction.InternalSetCodeNoCheck(code);

			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase + decoder.state.extraRegisterBaseEVEX) + baseReg1;
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op1Kind = OpKind.Register;
				instruction.Op1Register = (int)(decoder.state.rm + decoder.state.extraBaseRegisterBaseEVEX) + baseReg2;
				if ((decoder.state.zs.flags & StateFlags.b) != 0)
					instruction.InternalSetSuppressAllExceptions();
			}
			else {
				instruction.Op1Kind = OpKind.Memory;
				if ((decoder.state.zs.flags & StateFlags.b) != 0)
					instruction.InternalSetIsBroadcast();
				decoder.ReadOpMem(ref instruction, tupleType);
			}
			instruction.Op2Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadByte();
		}
	}

	sealed class OpCodeHandler_EVEX_VkW : OpCodeHandlerModRM {
		readonly Register baseReg1;
		readonly Register baseReg2;
		readonly Code code;
		readonly TupleType tupleType;
		readonly bool canBroadcast;

		public OpCodeHandler_EVEX_VkW(Register baseReg, Code code, TupleType tupleType, bool canBroadcast) {
			baseReg1 = baseReg;
			baseReg2 = baseReg;
			this.code = code;
			this.tupleType = tupleType;
			this.canBroadcast = canBroadcast;
		}

		public OpCodeHandler_EVEX_VkW(Register baseReg1, Register baseReg2, Code code, TupleType tupleType, bool canBroadcast) {
			this.baseReg1 = baseReg1;
			this.baseReg2 = baseReg2;
			this.code = code;
			this.tupleType = tupleType;
			this.canBroadcast = canBroadcast;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.EVEX);
			if ((decoder.state.vvvv_invalidCheck & decoder.invalidCheckMask) != 0)
				decoder.SetInvalidInstruction();
			instruction.InternalSetCodeNoCheck(code);

			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase + decoder.state.extraRegisterBaseEVEX) + baseReg1;
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op1Kind = OpKind.Register;
				instruction.Op1Register = (int)(decoder.state.rm + decoder.state.extraBaseRegisterBaseEVEX) + baseReg2;
				if (((uint)(decoder.state.zs.flags & StateFlags.b) & decoder.invalidCheckMask) != 0)
					decoder.SetInvalidInstruction();
			}
			else {
				instruction.Op1Kind = OpKind.Memory;
				if ((decoder.state.zs.flags & StateFlags.b) != 0) {
					if (canBroadcast)
						instruction.InternalSetIsBroadcast();
					else if (decoder.invalidCheckMask != 0)
						decoder.SetInvalidInstruction();
				}
				decoder.ReadOpMem(ref instruction, tupleType);
			}
		}
	}

	sealed class OpCodeHandler_EVEX_WkV : OpCodeHandlerModRM {
		readonly Register baseReg1;
		readonly Register baseReg2;
		readonly Code code;
		readonly TupleType tupleType;
		readonly uint disallowZeroingMasking;

		public OpCodeHandler_EVEX_WkV(Register baseReg, Code code, TupleType tupleType) {
			baseReg1 = baseReg;
			baseReg2 = baseReg;
			this.code = code;
			this.tupleType = tupleType;
			disallowZeroingMasking = 0;
		}

		public OpCodeHandler_EVEX_WkV(Register baseReg, Code code, TupleType tupleType, bool allowZeroingMasking) {
			baseReg1 = baseReg;
			baseReg2 = baseReg;
			this.code = code;
			this.tupleType = tupleType;
			disallowZeroingMasking = allowZeroingMasking ? 0 : uint.MaxValue;
		}

		public OpCodeHandler_EVEX_WkV(Register baseReg1, Register baseReg2, Code code, TupleType tupleType) {
			this.baseReg1 = baseReg1;
			this.baseReg2 = baseReg2;
			this.code = code;
			this.tupleType = tupleType;
			disallowZeroingMasking = 0;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.EVEX);
			if ((((uint)(decoder.state.zs.flags & StateFlags.b) | decoder.state.vvvv_invalidCheck) & decoder.invalidCheckMask) != 0)
				decoder.SetInvalidInstruction();
			instruction.InternalSetCodeNoCheck(code);

			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op1Kind = OpKind.Register;
			instruction.Op1Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase + decoder.state.extraRegisterBaseEVEX) + baseReg2;
			if (((uint)(decoder.state.zs.flags & StateFlags.z) & disallowZeroingMasking & decoder.invalidCheckMask) != 0)
				decoder.SetInvalidInstruction();
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op0Kind = OpKind.Register;
				instruction.Op0Register = (int)(decoder.state.rm + decoder.state.extraBaseRegisterBaseEVEX) + baseReg1;
			}
			else {
				instruction.Op0Kind = OpKind.Memory;
				if (((uint)(decoder.state.zs.flags & StateFlags.z) & decoder.invalidCheckMask) != 0)
					decoder.SetInvalidInstruction();
				decoder.ReadOpMem(ref instruction, tupleType);
			}
		}
	}

	sealed class OpCodeHandler_EVEX_VkM : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code code;
		readonly TupleType tupleType;

		public OpCodeHandler_EVEX_VkM(Register baseReg, Code code, TupleType tupleType) {
			this.baseReg = baseReg;
			this.code = code;
			this.tupleType = tupleType;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.EVEX);
			if ((((uint)(decoder.state.zs.flags & StateFlags.b) | decoder.state.vvvv_invalidCheck) & decoder.invalidCheckMask) != 0)
				decoder.SetInvalidInstruction();
			instruction.InternalSetCodeNoCheck(code);

			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase + decoder.state.extraRegisterBaseEVEX) + baseReg;
			if (decoder.state.mod == 3)
				decoder.SetInvalidInstruction();
			else {
				instruction.Op1Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction, tupleType);
			}
		}
	}

	sealed class OpCodeHandler_EVEX_VkWIb : OpCodeHandlerModRM {
		readonly Register baseReg1;
		readonly Register baseReg2;
		readonly Code code;
		readonly TupleType tupleType;
		readonly bool canBroadcast;

		public OpCodeHandler_EVEX_VkWIb(Register baseReg, Code code, TupleType tupleType, bool canBroadcast) {
			baseReg1 = baseReg;
			baseReg2 = baseReg;
			this.code = code;
			this.tupleType = tupleType;
			this.canBroadcast = canBroadcast;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.EVEX);
			if ((decoder.state.vvvv_invalidCheck & decoder.invalidCheckMask) != 0)
				decoder.SetInvalidInstruction();
			instruction.InternalSetCodeNoCheck(code);

			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase + decoder.state.extraRegisterBaseEVEX) + baseReg1;
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op1Kind = OpKind.Register;
				instruction.Op1Register = (int)(decoder.state.rm + decoder.state.extraBaseRegisterBaseEVEX) + baseReg2;
				if (((uint)(decoder.state.zs.flags & StateFlags.b) & decoder.invalidCheckMask) != 0)
					decoder.SetInvalidInstruction();
			}
			else {
				instruction.Op1Kind = OpKind.Memory;
				if ((decoder.state.zs.flags & StateFlags.b) != 0) {
					if (canBroadcast)
						instruction.InternalSetIsBroadcast();
					else if (decoder.invalidCheckMask != 0)
						decoder.SetInvalidInstruction();
				}
				decoder.ReadOpMem(ref instruction, tupleType);
			}
			instruction.Op2Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadByte();
		}
	}

	sealed class OpCodeHandler_EVEX_WkVIb : OpCodeHandlerModRM {
		readonly Register baseReg1;
		readonly Register baseReg2;
		readonly Code code;
		readonly TupleType tupleType;

		public OpCodeHandler_EVEX_WkVIb(Register baseReg1, Register baseReg2, Code code, TupleType tupleType) {
			this.baseReg1 = baseReg1;
			this.baseReg2 = baseReg2;
			this.code = code;
			this.tupleType = tupleType;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.EVEX);
			if ((((uint)(decoder.state.zs.flags & StateFlags.b) | decoder.state.vvvv_invalidCheck) & decoder.invalidCheckMask) != 0)
				decoder.SetInvalidInstruction();
			instruction.InternalSetCodeNoCheck(code);

			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op0Kind = OpKind.Register;
				instruction.Op0Register = (int)(decoder.state.rm + decoder.state.extraBaseRegisterBaseEVEX) + baseReg1;
			}
			else {
				instruction.Op0Kind = OpKind.Memory;
				if (((uint)(decoder.state.zs.flags & StateFlags.z) & decoder.invalidCheckMask) != 0)
					decoder.SetInvalidInstruction();
				decoder.ReadOpMem(ref instruction, tupleType);
			}
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op1Kind = OpKind.Register;
			instruction.Op1Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase + decoder.state.extraRegisterBaseEVEX) + baseReg2;
			instruction.Op2Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadByte();
		}
	}

	sealed class OpCodeHandler_EVEX_HkWIb : OpCodeHandlerModRM {
		readonly Register baseReg1;
		readonly Register baseReg2;
		readonly Code code;
		readonly TupleType tupleType;
		readonly bool canBroadcast;

		public OpCodeHandler_EVEX_HkWIb(Register baseReg, Code code, TupleType tupleType, bool canBroadcast) {
			baseReg1 = baseReg;
			baseReg2 = baseReg;
			this.code = code;
			this.tupleType = tupleType;
			this.canBroadcast = canBroadcast;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.EVEX);
			instruction.InternalSetCodeNoCheck(code);

			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = (int)decoder.state.vvvv + baseReg1;
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op1Kind = OpKind.Register;
				instruction.Op1Register = (int)(decoder.state.rm + decoder.state.extraBaseRegisterBaseEVEX) + baseReg2;
				if (((uint)(decoder.state.zs.flags & StateFlags.b) & decoder.invalidCheckMask) != 0)
					decoder.SetInvalidInstruction();
			}
			else {
				instruction.Op1Kind = OpKind.Memory;
				if ((decoder.state.zs.flags & StateFlags.b) != 0) {
					if (canBroadcast)
						instruction.InternalSetIsBroadcast();
					else if (decoder.invalidCheckMask != 0)
						decoder.SetInvalidInstruction();
				}
				decoder.ReadOpMem(ref instruction, tupleType);
			}
			instruction.Op2Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadByte();
		}
	}

	sealed class OpCodeHandler_EVEX_HWIb : OpCodeHandlerModRM {
		readonly Register baseReg1;
		readonly Register baseReg2;
		readonly Code code;
		readonly TupleType tupleType;

		public OpCodeHandler_EVEX_HWIb(Register baseReg, Code code, TupleType tupleType) {
			baseReg1 = baseReg;
			baseReg2 = baseReg;
			this.code = code;
			this.tupleType = tupleType;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.EVEX);
			instruction.InternalSetCodeNoCheck(code);
			if ((((uint)(decoder.state.zs.flags & (StateFlags.z | StateFlags.b)) | decoder.state.aaa) & decoder.invalidCheckMask) != 0)
				decoder.SetInvalidInstruction();

			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = (int)decoder.state.vvvv + baseReg1;
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op1Kind = OpKind.Register;
				instruction.Op1Register = (int)(decoder.state.rm + decoder.state.extraBaseRegisterBaseEVEX) + baseReg2;
				if (((uint)(decoder.state.zs.flags & StateFlags.b) & decoder.invalidCheckMask) != 0)
					decoder.SetInvalidInstruction();
			}
			else {
				instruction.Op1Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction, tupleType);
			}
			instruction.Op2Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadByte();
		}
	}

	sealed class OpCodeHandler_EVEX_WkVIb_er : OpCodeHandlerModRM {
		readonly Register baseReg1;
		readonly Register baseReg2;
		readonly Code code;
		readonly TupleType tupleType;

		public OpCodeHandler_EVEX_WkVIb_er(Register baseReg1, Register baseReg2, Code code, TupleType tupleType) {
			this.baseReg1 = baseReg1;
			this.baseReg2 = baseReg2;
			this.code = code;
			this.tupleType = tupleType;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.EVEX);
			if ((decoder.state.vvvv_invalidCheck & decoder.invalidCheckMask) != 0)
				decoder.SetInvalidInstruction();
			instruction.InternalSetCodeNoCheck(code);

			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op0Kind = OpKind.Register;
				instruction.Op0Register = (int)(decoder.state.rm + decoder.state.extraBaseRegisterBaseEVEX) + baseReg1;
				if ((decoder.state.zs.flags & StateFlags.b) != 0)
					instruction.InternalSetSuppressAllExceptions();
			}
			else {
				instruction.Op0Kind = OpKind.Memory;
				if (((uint)(decoder.state.zs.flags & (StateFlags.b | StateFlags.z)) & decoder.invalidCheckMask) != 0)
					decoder.SetInvalidInstruction();
				decoder.ReadOpMem(ref instruction, tupleType);
			}
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op1Kind = OpKind.Register;
			instruction.Op1Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase + decoder.state.extraRegisterBaseEVEX) + baseReg2;
			instruction.Op2Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadByte();
		}
	}

	sealed class OpCodeHandler_EVEX_VW_er : OpCodeHandlerModRM {
		readonly Register baseReg1;
		readonly Register baseReg2;
		readonly Code code;
		readonly TupleType tupleType;

		public OpCodeHandler_EVEX_VW_er(Register baseReg, Code code, TupleType tupleType) {
			baseReg1 = baseReg;
			baseReg2 = baseReg;
			this.code = code;
			this.tupleType = tupleType;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.EVEX);
			instruction.InternalSetCodeNoCheck(code);

			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase + decoder.state.extraRegisterBaseEVEX) + baseReg1;
			if ((((uint)(decoder.state.zs.flags & StateFlags.z) | decoder.state.vvvv_invalidCheck | decoder.state.aaa) & decoder.invalidCheckMask) != 0)
				decoder.SetInvalidInstruction();
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op1Kind = OpKind.Register;
				instruction.Op1Register = (int)(decoder.state.rm + decoder.state.extraBaseRegisterBaseEVEX) + baseReg2;
				if ((decoder.state.zs.flags & StateFlags.b) != 0)
					instruction.InternalSetSuppressAllExceptions();
			}
			else {
				instruction.Op1Kind = OpKind.Memory;
				if (((uint)(decoder.state.zs.flags & StateFlags.b) & decoder.invalidCheckMask) != 0)
					decoder.SetInvalidInstruction();
				decoder.ReadOpMem(ref instruction, tupleType);
			}
		}
	}

	sealed class OpCodeHandler_EVEX_VW : OpCodeHandlerModRM {
		readonly Register baseReg1;
		readonly Register baseReg2;
		readonly Code code;
		readonly TupleType tupleType;

		public OpCodeHandler_EVEX_VW(Register baseReg, Code code, TupleType tupleType) {
			baseReg1 = baseReg;
			baseReg2 = baseReg;
			this.code = code;
			this.tupleType = tupleType;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.EVEX);
			if ((((uint)(decoder.state.zs.flags & (StateFlags.z | StateFlags.b)) | decoder.state.vvvv_invalidCheck | decoder.state.aaa) & decoder.invalidCheckMask) != 0)
				decoder.SetInvalidInstruction();
			instruction.InternalSetCodeNoCheck(code);

			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase + decoder.state.extraRegisterBaseEVEX) + baseReg1;
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op1Kind = OpKind.Register;
				instruction.Op1Register = (int)(decoder.state.rm + decoder.state.extraBaseRegisterBaseEVEX) + baseReg2;
				if (((uint)(decoder.state.zs.flags & StateFlags.b) & decoder.invalidCheckMask) != 0)
					decoder.SetInvalidInstruction();
			}
			else {
				instruction.Op1Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction, tupleType);
			}
		}
	}

	sealed class OpCodeHandler_EVEX_WV : OpCodeHandlerModRM {
		readonly Register baseReg1;
		readonly Register baseReg2;
		readonly Code code;
		readonly TupleType tupleType;

		public OpCodeHandler_EVEX_WV(Register baseReg, Code code, TupleType tupleType) {
			baseReg1 = baseReg;
			baseReg2 = baseReg;
			this.code = code;
			this.tupleType = tupleType;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.EVEX);
			if ((((uint)(decoder.state.zs.flags & (StateFlags.z | StateFlags.b)) | decoder.state.vvvv_invalidCheck | decoder.state.aaa) & decoder.invalidCheckMask) != 0)
				decoder.SetInvalidInstruction();
			instruction.InternalSetCodeNoCheck(code);

			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op1Kind = OpKind.Register;
			instruction.Op1Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase + decoder.state.extraRegisterBaseEVEX) + baseReg1;
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op0Kind = OpKind.Register;
				instruction.Op0Register = (int)(decoder.state.rm + decoder.state.extraBaseRegisterBaseEVEX) + baseReg2;
				if (((uint)(decoder.state.zs.flags & StateFlags.b) & decoder.invalidCheckMask) != 0)
					decoder.SetInvalidInstruction();
			}
			else {
				instruction.Op0Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction, tupleType);
			}
		}
	}

	sealed class OpCodeHandler_EVEX_VM : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code code;
		readonly TupleType tupleType;

		public OpCodeHandler_EVEX_VM(Register baseReg, Code code, TupleType tupleType) {
			this.baseReg = baseReg;
			this.code = code;
			this.tupleType = tupleType;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.EVEX);
			if ((((uint)(decoder.state.zs.flags & (StateFlags.z | StateFlags.b)) | decoder.state.vvvv_invalidCheck | decoder.state.aaa) & decoder.invalidCheckMask) != 0)
				decoder.SetInvalidInstruction();
			instruction.InternalSetCodeNoCheck(code);

			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase + decoder.state.extraRegisterBaseEVEX) + baseReg;
			if (decoder.state.mod == 3)
				decoder.SetInvalidInstruction();
			else {
				instruction.Op1Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction, tupleType);
			}
		}
	}

	sealed class OpCodeHandler_EVEX_VK : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code code;

		public OpCodeHandler_EVEX_VK(Register baseReg, Code code) {
			this.baseReg = baseReg;
			this.code = code;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.EVEX);
			if ((((uint)(decoder.state.zs.flags & (StateFlags.b | StateFlags.z)) | decoder.state.vvvv_invalidCheck | decoder.state.aaa) & decoder.invalidCheckMask) != 0)
				decoder.SetInvalidInstruction();
			instruction.InternalSetCodeNoCheck(code);

			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase + decoder.state.extraRegisterBaseEVEX) + baseReg;
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op1Kind = OpKind.Register;
				instruction.Op1Register = (int)decoder.state.rm + Register.K0;
			}
			else
				decoder.SetInvalidInstruction();
		}
	}

	sealed class OpCodeHandler_EVEX_KR : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code code;

		public OpCodeHandler_EVEX_KR(Register baseReg, Code code) {
			this.baseReg = baseReg;
			this.code = code;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.EVEX);
			if ((((uint)(decoder.state.zs.flags & (StateFlags.b | StateFlags.z)) | decoder.state.vvvv_invalidCheck | decoder.state.aaa | decoder.state.zs.extraRegisterBase | decoder.state.extraRegisterBaseEVEX) & decoder.invalidCheckMask) != 0)
				decoder.SetInvalidInstruction();
			instruction.InternalSetCodeNoCheck(code);

			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = (int)decoder.state.reg + Register.K0;
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op1Kind = OpKind.Register;
				instruction.Op1Register = (int)(decoder.state.rm + decoder.state.extraBaseRegisterBaseEVEX) + baseReg;
			}
			else
				decoder.SetInvalidInstruction();
		}
	}

	sealed class OpCodeHandler_EVEX_KkHWIb_sae : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code code;
		readonly TupleType tupleType;
		readonly bool canBroadcast;

		public OpCodeHandler_EVEX_KkHWIb_sae(Register baseReg, Code code, TupleType tupleType, bool canBroadcast) {
			this.baseReg = baseReg;
			this.code = code;
			this.tupleType = tupleType;
			this.canBroadcast = canBroadcast;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.EVEX);
			if ((((uint)(decoder.state.zs.flags & StateFlags.z) | decoder.state.zs.extraRegisterBase | decoder.state.extraRegisterBaseEVEX) & decoder.invalidCheckMask) != 0)
				decoder.SetInvalidInstruction();
			instruction.InternalSetCodeNoCheck(code);

			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = (int)decoder.state.reg + Register.K0;
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op1Kind = OpKind.Register;
			instruction.Op1Register = (int)decoder.state.vvvv + baseReg;
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op2Kind = OpKind.Register;
				instruction.Op2Register = (int)(decoder.state.rm + decoder.state.extraBaseRegisterBaseEVEX) + baseReg;
				if ((decoder.state.zs.flags & StateFlags.b) != 0)
					instruction.InternalSetSuppressAllExceptions();
			}
			else {
				instruction.Op2Kind = OpKind.Memory;
				if ((decoder.state.zs.flags & StateFlags.b) != 0) {
					if (canBroadcast)
						instruction.InternalSetIsBroadcast();
					else if (decoder.invalidCheckMask != 0)
						decoder.SetInvalidInstruction();
				}
				decoder.ReadOpMem(ref instruction, tupleType);
			}
			instruction.Op3Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadByte();
		}
	}

	sealed class OpCodeHandler_EVEX_VkHW : OpCodeHandlerModRM {
		readonly Register baseReg1;
		readonly Register baseReg2;
		readonly Register baseReg3;
		readonly Code code;
		readonly TupleType tupleType;
		readonly bool canBroadcast;

		public OpCodeHandler_EVEX_VkHW(Register baseReg, Code code, TupleType tupleType, bool canBroadcast) {
			baseReg1 = baseReg;
			baseReg2 = baseReg;
			baseReg3 = baseReg;
			this.code = code;
			this.tupleType = tupleType;
			this.canBroadcast = canBroadcast;
		}

		public OpCodeHandler_EVEX_VkHW(Register baseReg1, Register baseReg2, Register baseReg3, Code code, TupleType tupleType, bool canBroadcast) {
			this.baseReg1 = baseReg1;
			this.baseReg2 = baseReg2;
			this.baseReg3 = baseReg3;
			this.code = code;
			this.tupleType = tupleType;
			this.canBroadcast = canBroadcast;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.EVEX);
			instruction.InternalSetCodeNoCheck(code);

			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase + decoder.state.extraRegisterBaseEVEX) + baseReg1;
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op1Kind = OpKind.Register;
			instruction.Op1Register = (int)decoder.state.vvvv + baseReg2;
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op2Kind = OpKind.Register;
				instruction.Op2Register = (int)(decoder.state.rm + decoder.state.extraBaseRegisterBaseEVEX) + baseReg3;
				if (((uint)(decoder.state.zs.flags & StateFlags.b) & decoder.invalidCheckMask) != 0)
					decoder.SetInvalidInstruction();
			}
			else {
				instruction.Op2Kind = OpKind.Memory;
				if ((decoder.state.zs.flags & StateFlags.b) != 0) {
					if (canBroadcast)
						instruction.InternalSetIsBroadcast();
					else if (decoder.invalidCheckMask != 0)
						decoder.SetInvalidInstruction();
				}
				decoder.ReadOpMem(ref instruction, tupleType);
			}
		}
	}

	sealed class OpCodeHandler_EVEX_VkHM : OpCodeHandlerModRM {
		readonly Register baseReg1;
		readonly Register baseReg2;
		readonly Code code;
		readonly TupleType tupleType;

		public OpCodeHandler_EVEX_VkHM(Register baseReg, Code code, TupleType tupleType) {
			baseReg1 = baseReg;
			baseReg2 = baseReg;
			this.code = code;
			this.tupleType = tupleType;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.EVEX);
			instruction.InternalSetCodeNoCheck(code);

			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase + decoder.state.extraRegisterBaseEVEX) + baseReg1;
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op1Kind = OpKind.Register;
			instruction.Op1Register = (int)decoder.state.vvvv + baseReg2;
			if (decoder.state.mod == 3)
				decoder.SetInvalidInstruction();
			else {
				instruction.Op2Kind = OpKind.Memory;
				if (((uint)(decoder.state.zs.flags & StateFlags.b) & decoder.invalidCheckMask) != 0)
					decoder.SetInvalidInstruction();
				decoder.ReadOpMem(ref instruction, tupleType);
			}
		}
	}

	sealed class OpCodeHandler_EVEX_VkHWIb : OpCodeHandlerModRM {
		readonly Register baseReg1;
		readonly Register baseReg2;
		readonly Register baseReg3;
		readonly Code code;
		readonly TupleType tupleType;
		readonly bool canBroadcast;

		public OpCodeHandler_EVEX_VkHWIb(Register baseReg, Code code, TupleType tupleType, bool canBroadcast) {
			baseReg1 = baseReg;
			baseReg2 = baseReg;
			baseReg3 = baseReg;
			this.code = code;
			this.tupleType = tupleType;
			this.canBroadcast = canBroadcast;
		}

		public OpCodeHandler_EVEX_VkHWIb(Register baseReg1, Register baseReg2, Register baseReg3, Code code, TupleType tupleType, bool canBroadcast) {
			this.baseReg1 = baseReg1;
			this.baseReg2 = baseReg2;
			this.baseReg3 = baseReg3;
			this.code = code;
			this.tupleType = tupleType;
			this.canBroadcast = canBroadcast;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.EVEX);
			instruction.InternalSetCodeNoCheck(code);

			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase + decoder.state.extraRegisterBaseEVEX) + baseReg1;
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op1Kind = OpKind.Register;
			instruction.Op1Register = (int)decoder.state.vvvv + baseReg2;
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op2Kind = OpKind.Register;
				instruction.Op2Register = (int)(decoder.state.rm + decoder.state.extraBaseRegisterBaseEVEX) + baseReg3;
				if (((uint)(decoder.state.zs.flags & StateFlags.b) & decoder.invalidCheckMask) != 0)
					decoder.SetInvalidInstruction();
			}
			else {
				instruction.Op2Kind = OpKind.Memory;
				if ((decoder.state.zs.flags & StateFlags.b) != 0) {
					if (canBroadcast)
						instruction.InternalSetIsBroadcast();
					else if (decoder.invalidCheckMask != 0)
						decoder.SetInvalidInstruction();
				}
				decoder.ReadOpMem(ref instruction, tupleType);
			}
			instruction.Op3Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadByte();
		}
	}

	sealed class OpCodeHandler_EVEX_VkHWIb_er : OpCodeHandlerModRM {
		readonly Register baseReg1;
		readonly Register baseReg2;
		readonly Register baseReg3;
		readonly Code code;
		readonly TupleType tupleType;
		readonly bool canBroadcast;

		public OpCodeHandler_EVEX_VkHWIb_er(Register baseReg, Code code, TupleType tupleType, bool canBroadcast) {
			baseReg1 = baseReg;
			baseReg2 = baseReg;
			baseReg3 = baseReg;
			this.code = code;
			this.tupleType = tupleType;
			this.canBroadcast = canBroadcast;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.EVEX);
			instruction.InternalSetCodeNoCheck(code);

			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase + decoder.state.extraRegisterBaseEVEX) + baseReg1;
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op1Kind = OpKind.Register;
			instruction.Op1Register = (int)decoder.state.vvvv + baseReg2;
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op2Kind = OpKind.Register;
				instruction.Op2Register = (int)(decoder.state.rm + decoder.state.extraBaseRegisterBaseEVEX) + baseReg3;
				if ((decoder.state.zs.flags & StateFlags.b) != 0)
					instruction.InternalSetSuppressAllExceptions();
			}
			else {
				instruction.Op2Kind = OpKind.Memory;
				if ((decoder.state.zs.flags & StateFlags.b) != 0) {
					if (canBroadcast)
						instruction.InternalSetIsBroadcast();
					else if (decoder.invalidCheckMask != 0)
						decoder.SetInvalidInstruction();
				}
				decoder.ReadOpMem(ref instruction, tupleType);
			}
			instruction.Op3Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadByte();
		}
	}

	sealed class OpCodeHandler_EVEX_KkHW : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code code;
		readonly TupleType tupleType;
		readonly bool canBroadcast;

		public OpCodeHandler_EVEX_KkHW(Register baseReg, Code code, TupleType tupleType, bool canBroadcast) {
			this.baseReg = baseReg;
			this.code = code;
			this.tupleType = tupleType;
			this.canBroadcast = canBroadcast;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.EVEX);
			instruction.InternalSetCodeNoCheck(code);

			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = (int)decoder.state.reg + Register.K0;
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op1Kind = OpKind.Register;
			instruction.Op1Register = (int)decoder.state.vvvv + baseReg;
			if ((((uint)(decoder.state.zs.flags & StateFlags.z) | decoder.state.zs.extraRegisterBase | decoder.state.extraRegisterBaseEVEX) & decoder.invalidCheckMask) != 0)
				decoder.SetInvalidInstruction();
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op2Kind = OpKind.Register;
				instruction.Op2Register = (int)(decoder.state.rm + decoder.state.extraBaseRegisterBaseEVEX) + baseReg;
				if (((uint)(decoder.state.zs.flags & StateFlags.b) & decoder.invalidCheckMask) != 0)
					decoder.SetInvalidInstruction();
			}
			else {
				instruction.Op2Kind = OpKind.Memory;
				if ((decoder.state.zs.flags & StateFlags.b) != 0) {
					if (canBroadcast)
						instruction.InternalSetIsBroadcast();
					else if (decoder.invalidCheckMask != 0)
						decoder.SetInvalidInstruction();
				}
				decoder.ReadOpMem(ref instruction, tupleType);
			}
		}
	}

	sealed class OpCodeHandler_EVEX_KP1HW : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code code;
		readonly TupleType tupleType;

		public OpCodeHandler_EVEX_KP1HW(Register baseReg, Code code, TupleType tupleType) {
			this.baseReg = baseReg;
			this.code = code;
			this.tupleType = tupleType;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.EVEX);
			instruction.InternalSetCodeNoCheck(code);

			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = (int)decoder.state.reg + Register.K0;
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op1Kind = OpKind.Register;
			instruction.Op1Register = (int)decoder.state.vvvv + baseReg;
			if ((((uint)(decoder.state.zs.flags & StateFlags.z) | decoder.state.aaa | decoder.state.zs.extraRegisterBase | decoder.state.extraRegisterBaseEVEX) & decoder.invalidCheckMask) != 0)
				decoder.SetInvalidInstruction();
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op2Kind = OpKind.Register;
				instruction.Op2Register = (int)(decoder.state.rm + decoder.state.extraBaseRegisterBaseEVEX) + baseReg;
				if (((uint)(decoder.state.zs.flags & StateFlags.b) & decoder.invalidCheckMask) != 0)
					decoder.SetInvalidInstruction();
			}
			else {
				instruction.Op2Kind = OpKind.Memory;
				if ((decoder.state.zs.flags & StateFlags.b) != 0)
					instruction.InternalSetIsBroadcast();
				decoder.ReadOpMem(ref instruction, tupleType);
			}
		}
	}

	sealed class OpCodeHandler_EVEX_KkHWIb : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code code;
		readonly TupleType tupleType;
		readonly bool canBroadcast;

		public OpCodeHandler_EVEX_KkHWIb(Register baseReg, Code code, TupleType tupleType, bool canBroadcast) {
			this.baseReg = baseReg;
			this.code = code;
			this.tupleType = tupleType;
			this.canBroadcast = canBroadcast;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.EVEX);
			instruction.InternalSetCodeNoCheck(code);

			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = (int)decoder.state.reg + Register.K0;
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op1Kind = OpKind.Register;
			instruction.Op1Register = (int)decoder.state.vvvv + baseReg;
			if ((((uint)(decoder.state.zs.flags & StateFlags.z) | decoder.state.zs.extraRegisterBase | decoder.state.extraRegisterBaseEVEX) & decoder.invalidCheckMask) != 0)
				decoder.SetInvalidInstruction();
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op2Kind = OpKind.Register;
				instruction.Op2Register = (int)(decoder.state.rm + decoder.state.extraBaseRegisterBaseEVEX) + baseReg;
				if (((uint)(decoder.state.zs.flags & StateFlags.b) & decoder.invalidCheckMask) != 0)
					decoder.SetInvalidInstruction();
			}
			else {
				instruction.Op2Kind = OpKind.Memory;
				if ((decoder.state.zs.flags & StateFlags.b) != 0) {
					if (canBroadcast)
						instruction.InternalSetIsBroadcast();
					else if (decoder.invalidCheckMask != 0)
						decoder.SetInvalidInstruction();
				}
				decoder.ReadOpMem(ref instruction, tupleType);
			}
			instruction.Op3Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadByte();
		}
	}

	sealed class OpCodeHandler_EVEX_WkHV : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code code;

		public OpCodeHandler_EVEX_WkHV(Register baseReg, Code code) {
			this.baseReg = baseReg;
			this.code = code;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.EVEX);
			instruction.InternalSetCodeNoCheck(code);

			Debug.Assert(decoder.state.mod == 3);
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = (int)(decoder.state.rm + decoder.state.extraBaseRegisterBaseEVEX) + baseReg;
			if (((uint)(decoder.state.zs.flags & StateFlags.b) & decoder.invalidCheckMask) != 0)
				decoder.SetInvalidInstruction();
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op1Kind = OpKind.Register;
			instruction.Op1Register = (int)decoder.state.vvvv + baseReg;
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op2Kind = OpKind.Register;
			instruction.Op2Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase + decoder.state.extraRegisterBaseEVEX) + baseReg;
		}
	}

	sealed class OpCodeHandler_EVEX_VHWIb : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code code;
		readonly TupleType tupleType;

		public OpCodeHandler_EVEX_VHWIb(Register baseReg, Code code, TupleType tupleType) {
			this.baseReg = baseReg;
			this.code = code;
			this.tupleType = tupleType;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.EVEX);
			if ((((uint)(decoder.state.zs.flags & (StateFlags.b | StateFlags.z)) | decoder.state.aaa) & decoder.invalidCheckMask) != 0)
				decoder.SetInvalidInstruction();
			instruction.InternalSetCodeNoCheck(code);

			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase + decoder.state.extraRegisterBaseEVEX) + baseReg;
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op1Kind = OpKind.Register;
			instruction.Op1Register = (int)decoder.state.vvvv + baseReg;
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op2Kind = OpKind.Register;
				instruction.Op2Register = (int)(decoder.state.rm + decoder.state.extraBaseRegisterBaseEVEX) + baseReg;
			}
			else {
				instruction.Op2Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction, tupleType);
			}
			instruction.Op3Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadByte();
		}
	}

	sealed class OpCodeHandler_EVEX_VHW : OpCodeHandlerModRM {
		readonly Register baseReg1;
		readonly Register baseReg2;
		readonly Register baseReg3;
		readonly Code codeR;
		readonly Code codeM;
		readonly TupleType tupleType;

		public OpCodeHandler_EVEX_VHW(Register baseReg, Code codeR, Code codeM, TupleType tupleType) {
			baseReg1 = baseReg;
			baseReg2 = baseReg;
			baseReg3 = baseReg;
			this.codeR = codeR;
			this.codeM = codeM;
			this.tupleType = tupleType;
		}

		public OpCodeHandler_EVEX_VHW(Register baseReg, Code code, TupleType tupleType) {
			baseReg1 = baseReg;
			baseReg2 = baseReg;
			baseReg3 = baseReg;
			codeR = code;
			codeM = code;
			this.tupleType = tupleType;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.EVEX);
			if ((((uint)(decoder.state.zs.flags & (StateFlags.b | StateFlags.z)) | decoder.state.aaa) & decoder.invalidCheckMask) != 0)
				decoder.SetInvalidInstruction();

			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase + decoder.state.extraRegisterBaseEVEX) + baseReg1;
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op1Kind = OpKind.Register;
			instruction.Op1Register = (int)decoder.state.vvvv + baseReg2;
			if (decoder.state.mod == 3) {
				instruction.InternalSetCodeNoCheck(codeR);
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op2Kind = OpKind.Register;
				instruction.Op2Register = (int)(decoder.state.rm + decoder.state.extraBaseRegisterBaseEVEX) + baseReg3;
			}
			else {
				instruction.InternalSetCodeNoCheck(codeM);
				instruction.Op2Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction, tupleType);
			}
		}
	}

	sealed class OpCodeHandler_EVEX_VHM : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code code;
		readonly TupleType tupleType;

		public OpCodeHandler_EVEX_VHM(Register baseReg, Code code, TupleType tupleType) {
			this.baseReg = baseReg;
			this.code = code;
			this.tupleType = tupleType;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.EVEX);
			if ((((uint)(decoder.state.zs.flags & (StateFlags.b | StateFlags.z)) | decoder.state.aaa) & decoder.invalidCheckMask) != 0)
				decoder.SetInvalidInstruction();
			instruction.InternalSetCodeNoCheck(code);

			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase + decoder.state.extraRegisterBaseEVEX) + baseReg;
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op1Kind = OpKind.Register;
			instruction.Op1Register = (int)decoder.state.vvvv + baseReg;
			if (decoder.state.mod == 3)
				decoder.SetInvalidInstruction();
			else {
				instruction.Op2Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction, tupleType);
			}
		}
	}

	sealed class OpCodeHandler_EVEX_Gv_W_er : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code codeW0;
		readonly Code codeW1;
		readonly TupleType tupleType;
		readonly bool onlySAE;

		public OpCodeHandler_EVEX_Gv_W_er(Register baseReg, Code codeW0, Code codeW1, TupleType tupleType, bool onlySAE) {
			this.baseReg = baseReg;
			this.codeW0 = codeW0;
			this.codeW1 = codeW1;
			this.tupleType = tupleType;
			this.onlySAE = onlySAE;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.EVEX);
			if ((((uint)(decoder.state.zs.flags & StateFlags.z) | decoder.state.vvvv_invalidCheck | decoder.state.aaa | decoder.state.extraRegisterBaseEVEX) & decoder.invalidCheckMask) != 0)
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
				instruction.Op1Register = (int)(decoder.state.rm + decoder.state.extraBaseRegisterBaseEVEX) + baseReg;
				if ((decoder.state.zs.flags & StateFlags.b) != 0) {
					if (onlySAE)
						instruction.InternalSetSuppressAllExceptions();
					else {
						Static.Assert((int)RoundingControl.None == 0 ? 0 : -1);
						Static.Assert((int)RoundingControl.RoundToNearest == 1 ? 0 : -1);
						Static.Assert((int)RoundingControl.RoundDown == 2 ? 0 : -1);
						Static.Assert((int)RoundingControl.RoundUp == 3 ? 0 : -1);
						Static.Assert((int)RoundingControl.RoundTowardZero == 4 ? 0 : -1);
						instruction.InternalRoundingControl = decoder.state.vectorLength + (uint)RoundingControl.RoundToNearest;
					}
				}
			}
			else {
				if (((uint)(decoder.state.zs.flags & StateFlags.b) & decoder.invalidCheckMask) != 0)
					decoder.SetInvalidInstruction();
				instruction.Op1Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction, tupleType);
			}
		}
	}

	sealed class OpCodeHandler_EVEX_VX_Ev : OpCodeHandlerModRM {
		readonly Code code32;
		readonly Code code64;
		readonly TupleType tupleTypeW0;
		readonly TupleType tupleTypeW1;

		public OpCodeHandler_EVEX_VX_Ev(Code code32, Code code64, TupleType tupleTypeW0, TupleType tupleTypeW1) {
			this.code32 = code32;
			this.code64 = code64;
			this.tupleTypeW0 = tupleTypeW0;
			this.tupleTypeW1 = tupleTypeW1;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.EVEX);
			if ((((uint)(decoder.state.zs.flags & (StateFlags.b | StateFlags.z)) | decoder.state.vvvv_invalidCheck | decoder.state.aaa) & decoder.invalidCheckMask) != 0)
				decoder.SetInvalidInstruction();
			Register gpr;
			TupleType tupleType;
			if (((uint)decoder.state.zs.flags & decoder.is64bMode_and_W) != 0) {
				instruction.InternalSetCodeNoCheck(code64);
				tupleType = tupleTypeW1;
				gpr = Register.RAX;
			}
			else {
				instruction.InternalSetCodeNoCheck(code32);
				tupleType = tupleTypeW0;
				gpr = Register.EAX;
			}
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase + decoder.state.extraRegisterBaseEVEX) + Register.XMM0;
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op1Kind = OpKind.Register;
				instruction.Op1Register = (int)(decoder.state.rm + decoder.state.zs.extraBaseRegisterBase) + gpr;
			}
			else {
				instruction.Op1Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction, tupleType);
			}
		}
	}

	sealed class OpCodeHandler_EVEX_Ev_VX : OpCodeHandlerModRM {
		readonly Code code32;
		readonly Code code64;
		readonly TupleType tupleTypeW0;
		readonly TupleType tupleTypeW1;

		public OpCodeHandler_EVEX_Ev_VX(Code code32, Code code64, TupleType tupleTypeW0, TupleType tupleTypeW1) {
			this.code32 = code32;
			this.code64 = code64;
			this.tupleTypeW0 = tupleTypeW0;
			this.tupleTypeW1 = tupleTypeW1;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.EVEX);
			if ((((uint)(decoder.state.zs.flags & (StateFlags.b | StateFlags.z)) | decoder.state.vvvv_invalidCheck | decoder.state.aaa) & decoder.invalidCheckMask) != 0)
				decoder.SetInvalidInstruction();
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op1Kind = OpKind.Register;
			instruction.Op1Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase + decoder.state.extraRegisterBaseEVEX) + Register.XMM0;
			Register gpr;
			TupleType tupleType;
			if (((uint)decoder.state.zs.flags & decoder.is64bMode_and_W) != 0) {
				instruction.InternalSetCodeNoCheck(code64);
				tupleType = tupleTypeW1;
				gpr = Register.RAX;
			}
			else {
				instruction.InternalSetCodeNoCheck(code32);
				tupleType = tupleTypeW0;
				gpr = Register.EAX;
			}
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op0Kind = OpKind.Register;
				instruction.Op0Register = (int)(decoder.state.rm + decoder.state.zs.extraBaseRegisterBase) + gpr;
			}
			else {
				instruction.Op0Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction, tupleType);
			}
		}
	}

	sealed class OpCodeHandler_EVEX_Ev_VX_Ib : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_EVEX_Ev_VX_Ib(Register baseReg, Code code32, Code code64) {
			this.baseReg = baseReg;
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.EVEX);
			if ((((uint)(decoder.state.zs.flags & (StateFlags.b | StateFlags.z)) | decoder.state.vvvv_invalidCheck | decoder.state.aaa | decoder.state.extraRegisterBaseEVEX) & decoder.invalidCheckMask) != 0)
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
			Debug.Assert(decoder.state.mod == 3);
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op1Kind = OpKind.Register;
			instruction.Op1Register = (int)(decoder.state.rm + decoder.state.extraBaseRegisterBaseEVEX) + baseReg;
			instruction.Op2Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadByte();
		}
	}

	sealed class OpCodeHandler_EVEX_MV : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code code;
		readonly TupleType tupleType;

		public OpCodeHandler_EVEX_MV(Register baseReg, Code code, TupleType tupleType) {
			this.baseReg = baseReg;
			this.code = code;
			this.tupleType = tupleType;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.EVEX);
			if ((((uint)(decoder.state.zs.flags & (StateFlags.b | StateFlags.z)) | decoder.state.vvvv_invalidCheck | decoder.state.aaa) & decoder.invalidCheckMask) != 0)
				decoder.SetInvalidInstruction();
			instruction.InternalSetCodeNoCheck(code);

			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op1Kind = OpKind.Register;
			instruction.Op1Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase + decoder.state.extraRegisterBaseEVEX) + baseReg;
			if (decoder.state.mod == 3)
				decoder.SetInvalidInstruction();
			else {
				instruction.Op0Kind = OpKind.Memory;
				decoder.ReadOpMem(ref instruction, tupleType);
			}
		}
	}

	sealed class OpCodeHandler_EVEX_VkEv_REXW : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code code32;
		readonly Code code64;

		public OpCodeHandler_EVEX_VkEv_REXW(Register baseReg, Code code32) {
			this.baseReg = baseReg;
			this.code32 = code32;
			code64 = Code.INVALID;
		}

		public OpCodeHandler_EVEX_VkEv_REXW(Register baseReg, Code code32, Code code64) {
			this.baseReg = baseReg;
			this.code32 = code32;
			this.code64 = code64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.EVEX);
			if ((((uint)(decoder.state.zs.flags & StateFlags.b) | decoder.state.vvvv_invalidCheck) & decoder.invalidCheckMask) != 0)
				decoder.SetInvalidInstruction();
			Register gpr;
			if (((uint)decoder.state.zs.flags & decoder.is64bMode_and_W) != 0) {
				Debug.Assert(code64 != Code.INVALID);
				instruction.InternalSetCodeNoCheck(code64);
				gpr = Register.RAX;
			}
			else {
				instruction.InternalSetCodeNoCheck(code32);
				gpr = Register.EAX;
			}

			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase + decoder.state.extraRegisterBaseEVEX) + baseReg;
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op1Kind = OpKind.Register;
				instruction.Op1Register = (int)(decoder.state.rm + decoder.state.zs.extraBaseRegisterBase) + gpr;
			}
			else
				decoder.SetInvalidInstruction();
		}
	}

	sealed class OpCodeHandler_EVEX_Vk_VSIB : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Register vsibBase;
		readonly Code code;
		readonly TupleType tupleType;

		public OpCodeHandler_EVEX_Vk_VSIB(Register baseReg, Register vsibBase, Code code, TupleType tupleType) {
			this.baseReg = baseReg;
			this.vsibBase = vsibBase;
			this.code = code;
			this.tupleType = tupleType;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.EVEX);
			if (decoder.invalidCheckMask != 0 && (((uint)(decoder.state.zs.flags & (StateFlags.z | StateFlags.b)) | (decoder.state.vvvv_invalidCheck & 0xF)) != 0 || decoder.state.aaa == 0))
				decoder.SetInvalidInstruction();
			instruction.InternalSetCodeNoCheck(code);

			int regNum = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase + decoder.state.extraRegisterBaseEVEX);
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = regNum + baseReg;
			if (decoder.state.mod == 3)
				decoder.SetInvalidInstruction();
			else {
				instruction.Op1Kind = OpKind.Memory;
				decoder.ReadOpMem_VSIB(ref instruction, vsibBase, tupleType);
				if (decoder.invalidCheckMask != 0) {
					if ((uint)regNum == ((uint)(instruction.MemoryIndex - Register.XMM0) % (uint)IcedConstants.VMM_count))
						decoder.SetInvalidInstruction();
				}
			}
		}
	}

	sealed class OpCodeHandler_EVEX_VSIB_k1_VX : OpCodeHandlerModRM {
		readonly Register vsibIndex;
		readonly Register baseReg;
		readonly Code code;
		readonly TupleType tupleType;

		public OpCodeHandler_EVEX_VSIB_k1_VX(Register vsibIndex, Register baseReg, Code code, TupleType tupleType) {
			this.vsibIndex = vsibIndex;
			this.baseReg = baseReg;
			this.code = code;
			this.tupleType = tupleType;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.EVEX);
			if (decoder.invalidCheckMask != 0 && (((uint)(decoder.state.zs.flags & (StateFlags.b | StateFlags.z)) | (decoder.state.vvvv_invalidCheck & 0xF)) != 0 || decoder.state.aaa == 0))
				decoder.SetInvalidInstruction();
			instruction.InternalSetCodeNoCheck(code);

			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op1Kind = OpKind.Register;
			instruction.Op1Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase + decoder.state.extraRegisterBaseEVEX) + baseReg;
			if (decoder.state.mod == 3)
				decoder.SetInvalidInstruction();
			else {
				instruction.Op0Kind = OpKind.Memory;
				decoder.ReadOpMem_VSIB(ref instruction, vsibIndex, tupleType);
			}
		}
	}

	sealed class OpCodeHandler_EVEX_VSIB_k1 : OpCodeHandlerModRM {
		readonly Register vsibIndex;
		readonly Code code;
		readonly TupleType tupleType;

		public OpCodeHandler_EVEX_VSIB_k1(Register vsibIndex, Code code, TupleType tupleType) {
			this.vsibIndex = vsibIndex;
			this.code = code;
			this.tupleType = tupleType;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.EVEX);
			if (decoder.invalidCheckMask != 0 && (((uint)(decoder.state.zs.flags & (StateFlags.b | StateFlags.z)) | (decoder.state.vvvv_invalidCheck & 0xF)) != 0 || decoder.state.aaa == 0))
				decoder.SetInvalidInstruction();
			instruction.InternalSetCodeNoCheck(code);

			if (decoder.state.mod == 3)
				decoder.SetInvalidInstruction();
			else {
				instruction.Op0Kind = OpKind.Memory;
				decoder.ReadOpMem_VSIB(ref instruction, vsibIndex, tupleType);
			}
		}
	}

	sealed class OpCodeHandler_EVEX_GvM_VX_Ib : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code code32;
		readonly Code code64;
		readonly TupleType tupleType32;
		readonly TupleType tupleType64;

		public OpCodeHandler_EVEX_GvM_VX_Ib(Register baseReg, Code code32, Code code64, TupleType tupleType32, TupleType tupleType64) {
			this.baseReg = baseReg;
			this.code32 = code32;
			this.code64 = code64;
			this.tupleType32 = tupleType32;
			this.tupleType64 = tupleType64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.EVEX);
			if ((((uint)(decoder.state.zs.flags & (StateFlags.b | StateFlags.z)) | decoder.state.vvvv_invalidCheck | decoder.state.aaa) & decoder.invalidCheckMask) != 0)
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
				if (((uint)decoder.state.zs.flags & decoder.is64bMode_and_W) != 0)
					decoder.ReadOpMem(ref instruction, tupleType64);
				else
					decoder.ReadOpMem(ref instruction, tupleType32);
			}
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op1Kind = OpKind.Register;
			instruction.Op1Register = (int)(decoder.state.reg + decoder.state.zs.extraRegisterBase + decoder.state.extraRegisterBaseEVEX) + baseReg;
			instruction.Op2Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadByte();
		}
	}

	sealed class OpCodeHandler_EVEX_KkWIb : OpCodeHandlerModRM {
		readonly Register baseReg;
		readonly Code code;
		readonly TupleType tupleType;
		readonly bool canBroadcast;

		public OpCodeHandler_EVEX_KkWIb(Register baseReg, Code code, TupleType tupleType, bool canBroadcast) {
			this.baseReg = baseReg;
			this.code = code;
			this.tupleType = tupleType;
			this.canBroadcast = canBroadcast;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.EVEX);
			if ((((uint)(decoder.state.zs.flags & StateFlags.z) | decoder.state.vvvv_invalidCheck | decoder.state.zs.extraRegisterBase | decoder.state.extraRegisterBaseEVEX) & decoder.invalidCheckMask) != 0)
				decoder.SetInvalidInstruction();
			instruction.InternalSetCodeNoCheck(code);

			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = (int)decoder.state.reg + Register.K0;
			if (decoder.state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op1Kind = OpKind.Register;
				instruction.Op1Register = (int)(decoder.state.rm + decoder.state.extraBaseRegisterBaseEVEX) + baseReg;
				if (((uint)(decoder.state.zs.flags & StateFlags.b) & decoder.invalidCheckMask) != 0)
					decoder.SetInvalidInstruction();
			}
			else {
				instruction.Op1Kind = OpKind.Memory;
				if ((decoder.state.zs.flags & StateFlags.b) != 0) {
					if (canBroadcast)
						instruction.InternalSetIsBroadcast();
					else if (decoder.invalidCheckMask != 0)
						decoder.SetInvalidInstruction();
				}
				decoder.ReadOpMem(ref instruction, tupleType);
			}
			instruction.Op2Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadByte();
		}
	}
}
#endif
