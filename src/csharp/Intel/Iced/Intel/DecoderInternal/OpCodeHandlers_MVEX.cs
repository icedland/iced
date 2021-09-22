// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if DECODER && MVEX
using System;
using System.Diagnostics;

namespace Iced.Intel.DecoderInternal {
	sealed class OpCodeHandler_EH : OpCodeHandlerModRM {
		readonly OpCodeHandler handlerEH0;
		readonly OpCodeHandler handlerEH1;

		public OpCodeHandler_EH(OpCodeHandler handlerEH0, OpCodeHandler handlerEH1) {
			this.handlerEH0 = handlerEH0 ?? throw new ArgumentNullException(nameof(handlerEH0));
			this.handlerEH1 = handlerEH1 ?? throw new ArgumentNullException(nameof(handlerEH1));
			Debug.Assert(handlerEH0.HasModRM == HasModRM);
			Debug.Assert(handlerEH1.HasModRM == HasModRM);
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.MVEX);
			((decoder.state.flags & StateFlags.MvexEH) != 0 ? handlerEH1 : handlerEH0).Decode(decoder, ref instruction);
		}
	}

	sealed class OpCodeHandler_MVEX_M : OpCodeHandlerModRM {
		readonly Code code;

		public OpCodeHandler_MVEX_M(Code code) {
			Debug.Assert(new MvexInfo(code).IgnoresOpMaskRegister);
			Debug.Assert(new MvexInfo(code).CanUseEvictionHint);
			this.code = code;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.MVEX);
			if ((state.vvvv_invalidCheck & decoder.invalidCheckMask) != 0)
				decoder.SetInvalidInstruction();
			instruction.OpMask = Register.None; // It's ignored (see ctor)
			instruction.Code = code;
			var mvex = new MvexInfo(code);
			var sss = state.Sss;
			if (state.mod == 3)
				decoder.SetInvalidInstruction();
			else {
				instruction.Op0Kind = OpKind.Memory;
				if ((state.flags & StateFlags.MvexEH) != 0)
					instruction.InternalSetIsMvexEvictionHint();
				if ((mvex.InvalidConvFns & (1U << sss) & decoder.invalidCheckMask) != 0)
					decoder.SetInvalidInstruction();
				instruction.InternalSetMvexRegMemConv(MvexRegMemConv.MemConvNone + sss);
				decoder.ReadOpMem(ref instruction, mvex.GetTupleType(sss));
			}
		}
	}

	sealed class OpCodeHandler_MVEX_MV : OpCodeHandlerModRM {
		readonly Code code;

		public OpCodeHandler_MVEX_MV(Code code) {
			Debug.Assert(!new MvexInfo(code).IgnoresOpMaskRegister);
			this.code = code;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.MVEX);
			if ((state.vvvv_invalidCheck & decoder.invalidCheckMask) != 0)
				decoder.SetInvalidInstruction();
			instruction.Code = code;
			var mvex = new MvexInfo(code);
			var sss = state.Sss;
			if (state.mod == 3)
				decoder.SetInvalidInstruction();
			else {
				instruction.Op0Kind = OpKind.Memory;
				if (mvex.CanUseEvictionHint && (state.flags & StateFlags.MvexEH) != 0)
					instruction.InternalSetIsMvexEvictionHint();
				if ((mvex.InvalidConvFns & (1U << sss) & decoder.invalidCheckMask) != 0)
					decoder.SetInvalidInstruction();
				instruction.InternalSetMvexRegMemConv(MvexRegMemConv.MemConvNone + sss);
				decoder.ReadOpMem(ref instruction, mvex.GetTupleType(sss));
			}
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op1Kind = OpKind.Register;
			instruction.Op1Register = (int)(state.reg + state.extraRegisterBase + state.extraRegisterBaseEVEX) + Register.ZMM0;
		}
	}

	sealed class OpCodeHandler_MVEX_VW : OpCodeHandlerModRM {
		readonly Code code;

		public OpCodeHandler_MVEX_VW(Code code) {
			Debug.Assert(!new MvexInfo(code).IgnoresOpMaskRegister);
			Debug.Assert(new MvexInfo(code).CanUseEvictionHint);
			this.code = code;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.MVEX);
			if ((state.vvvv_invalidCheck & decoder.invalidCheckMask) != 0)
				decoder.SetInvalidInstruction();
			instruction.Code = code;

			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = (int)(state.reg + state.extraRegisterBase + state.extraRegisterBaseEVEX) + Register.ZMM0;
			var mvex = new MvexInfo(code);
			var sss = state.Sss;
			if (state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op1Kind = OpKind.Register;
				instruction.Op1Register = (int)(state.rm + state.extraBaseRegisterBaseEVEX) + Register.ZMM0;
				if ((state.flags & StateFlags.MvexEH) != 0) {
					if (mvex.CanUseSuppressAllExceptions) {
						if ((sss & 4) != 0)
							instruction.InternalSetSuppressAllExceptions();
						if (mvex.CanUseRoundingControl) {
							Static.Assert((int)RoundingControl.None == 0 ? 0 : -1);
							Static.Assert((int)RoundingControl.RoundToNearest == 1 ? 0 : -1);
							Static.Assert((int)RoundingControl.RoundDown == 2 ? 0 : -1);
							Static.Assert((int)RoundingControl.RoundUp == 3 ? 0 : -1);
							Static.Assert((int)RoundingControl.RoundTowardZero == 4 ? 0 : -1);
							instruction.InternalRoundingControl = ((uint)sss & 3) + (uint)RoundingControl.RoundToNearest;
						}
					}
					else if (mvex.NoSaeRc && ((uint)sss & decoder.invalidCheckMask) != 0)
						decoder.SetInvalidInstruction();
				}
				else {
					if ((mvex.InvalidSwizzleFns & (1U << sss) & decoder.invalidCheckMask) != 0)
						decoder.SetInvalidInstruction();
					Debug.Assert((uint)sss <= 7);
					instruction.InternalSetMvexRegMemConv(MvexRegMemConv.RegSwizzleNone + sss);
				}
			}
			else {
				instruction.Op1Kind = OpKind.Memory;
				if ((state.flags & StateFlags.MvexEH) != 0)
					instruction.InternalSetIsMvexEvictionHint();
				if ((mvex.InvalidConvFns & (1U << sss) & decoder.invalidCheckMask) != 0)
					decoder.SetInvalidInstruction();
				instruction.InternalSetMvexRegMemConv(MvexRegMemConv.MemConvNone + sss);
				decoder.ReadOpMem(ref instruction, mvex.GetTupleType(sss));
			}
		}
	}

	sealed class OpCodeHandler_MVEX_HWIb : OpCodeHandlerModRM {
		readonly Code code;

		public OpCodeHandler_MVEX_HWIb(Code code) {
			Debug.Assert(!new MvexInfo(code).IgnoresOpMaskRegister);
			Debug.Assert(new MvexInfo(code).CanUseEvictionHint);
			this.code = code;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.MVEX);
			instruction.Code = code;

			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = (int)state.vvvv + Register.ZMM0;
			var mvex = new MvexInfo(code);
			var sss = state.Sss;
			if (state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op1Kind = OpKind.Register;
				instruction.Op1Register = (int)(state.rm + state.extraBaseRegisterBaseEVEX) + Register.ZMM0;
				if ((state.flags & StateFlags.MvexEH) != 0) {
					if (mvex.CanUseSuppressAllExceptions) {
						if ((sss & 4) != 0)
							instruction.InternalSetSuppressAllExceptions();
						if (mvex.CanUseRoundingControl) {
							Static.Assert((int)RoundingControl.None == 0 ? 0 : -1);
							Static.Assert((int)RoundingControl.RoundToNearest == 1 ? 0 : -1);
							Static.Assert((int)RoundingControl.RoundDown == 2 ? 0 : -1);
							Static.Assert((int)RoundingControl.RoundUp == 3 ? 0 : -1);
							Static.Assert((int)RoundingControl.RoundTowardZero == 4 ? 0 : -1);
							instruction.InternalRoundingControl = ((uint)sss & 3) + (uint)RoundingControl.RoundToNearest;
						}
					}
					else if (mvex.NoSaeRc && ((uint)sss & decoder.invalidCheckMask) != 0)
						decoder.SetInvalidInstruction();
				}
				else {
					if ((mvex.InvalidSwizzleFns & (1U << sss) & decoder.invalidCheckMask) != 0)
						decoder.SetInvalidInstruction();
					Debug.Assert((uint)sss <= 7);
					instruction.InternalSetMvexRegMemConv(MvexRegMemConv.RegSwizzleNone + sss);
				}
			}
			else {
				instruction.Op1Kind = OpKind.Memory;
				if ((state.flags & StateFlags.MvexEH) != 0)
					instruction.InternalSetIsMvexEvictionHint();
				if ((mvex.InvalidConvFns & (1U << sss) & decoder.invalidCheckMask) != 0)
					decoder.SetInvalidInstruction();
				instruction.InternalSetMvexRegMemConv(MvexRegMemConv.MemConvNone + sss);
				decoder.ReadOpMem(ref instruction, mvex.GetTupleType(sss));
			}
			instruction.Op2Kind = OpKind.Immediate8;
			instruction.Immediate8 = (byte)decoder.ReadByte();
		}
	}

	sealed class OpCodeHandler_MVEX_VWIb : OpCodeHandlerModRM {
		readonly Code code;

		public OpCodeHandler_MVEX_VWIb(Code code) {
			Debug.Assert(!new MvexInfo(code).IgnoresOpMaskRegister);
			Debug.Assert(new MvexInfo(code).CanUseEvictionHint);
			this.code = code;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.MVEX);
			if ((state.vvvv_invalidCheck & decoder.invalidCheckMask) != 0)
				decoder.SetInvalidInstruction();
			instruction.Code = code;

			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = (int)(state.reg + state.extraRegisterBase + state.extraRegisterBaseEVEX) + Register.ZMM0;
			var mvex = new MvexInfo(code);
			var sss = state.Sss;
			if (state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op1Kind = OpKind.Register;
				instruction.Op1Register = (int)(state.rm + state.extraBaseRegisterBaseEVEX) + Register.ZMM0;
				if ((state.flags & StateFlags.MvexEH) != 0) {
					if (mvex.CanUseSuppressAllExceptions) {
						if ((sss & 4) != 0)
							instruction.InternalSetSuppressAllExceptions();
						if (mvex.CanUseRoundingControl) {
							Static.Assert((int)RoundingControl.None == 0 ? 0 : -1);
							Static.Assert((int)RoundingControl.RoundToNearest == 1 ? 0 : -1);
							Static.Assert((int)RoundingControl.RoundDown == 2 ? 0 : -1);
							Static.Assert((int)RoundingControl.RoundUp == 3 ? 0 : -1);
							Static.Assert((int)RoundingControl.RoundTowardZero == 4 ? 0 : -1);
							instruction.InternalRoundingControl = ((uint)sss & 3) + (uint)RoundingControl.RoundToNearest;
						}
					}
					else if (mvex.NoSaeRc && ((uint)sss & decoder.invalidCheckMask) != 0)
						decoder.SetInvalidInstruction();
				}
				else {
					if ((mvex.InvalidSwizzleFns & (1U << sss) & decoder.invalidCheckMask) != 0)
						decoder.SetInvalidInstruction();
					Debug.Assert((uint)sss <= 7);
					instruction.InternalSetMvexRegMemConv(MvexRegMemConv.RegSwizzleNone + sss);
				}
			}
			else {
				instruction.Op1Kind = OpKind.Memory;
				if ((state.flags & StateFlags.MvexEH) != 0)
					instruction.InternalSetIsMvexEvictionHint();
				if ((mvex.InvalidConvFns & (1U << sss) & decoder.invalidCheckMask) != 0)
					decoder.SetInvalidInstruction();
				instruction.InternalSetMvexRegMemConv(MvexRegMemConv.MemConvNone + sss);
				decoder.ReadOpMem(ref instruction, mvex.GetTupleType(sss));
			}
			instruction.Op2Kind = OpKind.Immediate8;
			instruction.Immediate8 = (byte)decoder.ReadByte();
		}
	}

	sealed class OpCodeHandler_MVEX_VHW : OpCodeHandlerModRM {
		readonly Code code;

		public OpCodeHandler_MVEX_VHW(Code code) {
			Debug.Assert(!new MvexInfo(code).IgnoresOpMaskRegister);
			Debug.Assert(new MvexInfo(code).CanUseEvictionHint);
			this.code = code;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.MVEX);
			instruction.Code = code;

			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = (int)(state.reg + state.extraRegisterBase + state.extraRegisterBaseEVEX) + Register.ZMM0;
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op1Kind = OpKind.Register;
			instruction.Op1Register = (int)state.vvvv + Register.ZMM0;
			var mvex = new MvexInfo(code);
			if (mvex.RequireOpMaskRegister && decoder.invalidCheckMask != 0 && state.aaa == 0)
				decoder.SetInvalidInstruction();
			var sss = state.Sss;
			if (state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op2Kind = OpKind.Register;
				instruction.Op2Register = (int)(state.rm + state.extraBaseRegisterBaseEVEX) + Register.ZMM0;
				if ((state.flags & StateFlags.MvexEH) != 0) {
					if (mvex.CanUseSuppressAllExceptions) {
						if ((sss & 4) != 0)
							instruction.InternalSetSuppressAllExceptions();
						if (mvex.CanUseRoundingControl) {
							Static.Assert((int)RoundingControl.None == 0 ? 0 : -1);
							Static.Assert((int)RoundingControl.RoundToNearest == 1 ? 0 : -1);
							Static.Assert((int)RoundingControl.RoundDown == 2 ? 0 : -1);
							Static.Assert((int)RoundingControl.RoundUp == 3 ? 0 : -1);
							Static.Assert((int)RoundingControl.RoundTowardZero == 4 ? 0 : -1);
							instruction.InternalRoundingControl = ((uint)sss & 3) + (uint)RoundingControl.RoundToNearest;
						}
					}
					else if (mvex.NoSaeRc && ((uint)sss & decoder.invalidCheckMask) != 0)
						decoder.SetInvalidInstruction();
				}
				else {
					if ((mvex.InvalidSwizzleFns & (1U << sss) & decoder.invalidCheckMask) != 0)
						decoder.SetInvalidInstruction();
					Debug.Assert((uint)sss <= 7);
					instruction.InternalSetMvexRegMemConv(MvexRegMemConv.RegSwizzleNone + sss);
				}
			}
			else {
				instruction.Op2Kind = OpKind.Memory;
				if ((state.flags & StateFlags.MvexEH) != 0)
					instruction.InternalSetIsMvexEvictionHint();
				if ((mvex.InvalidConvFns & (1U << sss) & decoder.invalidCheckMask) != 0)
					decoder.SetInvalidInstruction();
				instruction.InternalSetMvexRegMemConv(MvexRegMemConv.MemConvNone + sss);
				decoder.ReadOpMem(ref instruction, mvex.GetTupleType(sss));
			}
		}
	}

	sealed class OpCodeHandler_MVEX_VHWIb : OpCodeHandlerModRM {
		readonly Code code;

		public OpCodeHandler_MVEX_VHWIb(Code code) {
			Debug.Assert(!new MvexInfo(code).IgnoresOpMaskRegister);
			Debug.Assert(new MvexInfo(code).CanUseEvictionHint);
			this.code = code;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.MVEX);
			instruction.Code = code;

			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = (int)(state.reg + state.extraRegisterBase + state.extraRegisterBaseEVEX) + Register.ZMM0;
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op1Kind = OpKind.Register;
			instruction.Op1Register = (int)state.vvvv + Register.ZMM0;
			var mvex = new MvexInfo(code);
			var sss = state.Sss;
			if (state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op2Kind = OpKind.Register;
				instruction.Op2Register = (int)(state.rm + state.extraBaseRegisterBaseEVEX) + Register.ZMM0;
				if ((state.flags & StateFlags.MvexEH) != 0) {
					if (mvex.CanUseSuppressAllExceptions) {
						if ((sss & 4) != 0)
							instruction.InternalSetSuppressAllExceptions();
						if (mvex.CanUseRoundingControl) {
							Static.Assert((int)RoundingControl.None == 0 ? 0 : -1);
							Static.Assert((int)RoundingControl.RoundToNearest == 1 ? 0 : -1);
							Static.Assert((int)RoundingControl.RoundDown == 2 ? 0 : -1);
							Static.Assert((int)RoundingControl.RoundUp == 3 ? 0 : -1);
							Static.Assert((int)RoundingControl.RoundTowardZero == 4 ? 0 : -1);
							instruction.InternalRoundingControl = ((uint)sss & 3) + (uint)RoundingControl.RoundToNearest;
						}
					}
					else if (mvex.NoSaeRc && ((uint)sss & decoder.invalidCheckMask) != 0)
						decoder.SetInvalidInstruction();
				}
				else {
					if ((mvex.InvalidSwizzleFns & (1U << sss) & decoder.invalidCheckMask) != 0)
						decoder.SetInvalidInstruction();
					Debug.Assert((uint)sss <= 7);
					instruction.InternalSetMvexRegMemConv(MvexRegMemConv.RegSwizzleNone + sss);
				}
			}
			else {
				instruction.Op2Kind = OpKind.Memory;
				if ((state.flags & StateFlags.MvexEH) != 0)
					instruction.InternalSetIsMvexEvictionHint();
				if ((mvex.InvalidConvFns & (1U << sss) & decoder.invalidCheckMask) != 0)
					decoder.SetInvalidInstruction();
				instruction.InternalSetMvexRegMemConv(MvexRegMemConv.MemConvNone + sss);
				decoder.ReadOpMem(ref instruction, mvex.GetTupleType(sss));
			}
			instruction.Op3Kind = OpKind.Immediate8;
			instruction.Immediate8 = (byte)decoder.ReadByte();
		}
	}

	sealed class OpCodeHandler_MVEX_VKW : OpCodeHandlerModRM {
		readonly Code code;

		public OpCodeHandler_MVEX_VKW(Code code) {
			Debug.Assert(!new MvexInfo(code).IgnoresOpMaskRegister);
			Debug.Assert(new MvexInfo(code).CanUseEvictionHint);
			this.code = code;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.MVEX);
			if ((state.vvvv & decoder.invalidCheckMask) > 7)
				decoder.SetInvalidInstruction();
			instruction.Code = code;

			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = (int)(state.reg + state.extraRegisterBase + state.extraRegisterBaseEVEX) + Register.ZMM0;
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op1Kind = OpKind.Register;
			instruction.Op1Register = ((int)state.vvvv & 7) + Register.K0;
			var mvex = new MvexInfo(code);
			var sss = state.Sss;
			if (state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op2Kind = OpKind.Register;
				instruction.Op2Register = (int)(state.rm + state.extraBaseRegisterBaseEVEX) + Register.ZMM0;
				if ((state.flags & StateFlags.MvexEH) != 0) {
					if (mvex.CanUseSuppressAllExceptions) {
						if ((sss & 4) != 0)
							instruction.InternalSetSuppressAllExceptions();
						if (mvex.CanUseRoundingControl) {
							Static.Assert((int)RoundingControl.None == 0 ? 0 : -1);
							Static.Assert((int)RoundingControl.RoundToNearest == 1 ? 0 : -1);
							Static.Assert((int)RoundingControl.RoundDown == 2 ? 0 : -1);
							Static.Assert((int)RoundingControl.RoundUp == 3 ? 0 : -1);
							Static.Assert((int)RoundingControl.RoundTowardZero == 4 ? 0 : -1);
							instruction.InternalRoundingControl = ((uint)sss & 3) + (uint)RoundingControl.RoundToNearest;
						}
					}
					else if (mvex.NoSaeRc && ((uint)sss & decoder.invalidCheckMask) != 0)
						decoder.SetInvalidInstruction();
				}
				else {
					if ((mvex.InvalidSwizzleFns & (1U << sss) & decoder.invalidCheckMask) != 0)
						decoder.SetInvalidInstruction();
					Debug.Assert((uint)sss <= 7);
					instruction.InternalSetMvexRegMemConv(MvexRegMemConv.RegSwizzleNone + sss);
				}
			}
			else {
				instruction.Op2Kind = OpKind.Memory;
				if ((state.flags & StateFlags.MvexEH) != 0)
					instruction.InternalSetIsMvexEvictionHint();
				if ((mvex.InvalidConvFns & (1U << sss) & decoder.invalidCheckMask) != 0)
					decoder.SetInvalidInstruction();
				instruction.InternalSetMvexRegMemConv(MvexRegMemConv.MemConvNone + sss);
				decoder.ReadOpMem(ref instruction, mvex.GetTupleType(sss));
			}
		}
	}

	sealed class OpCodeHandler_MVEX_KHW : OpCodeHandlerModRM {
		readonly Code code;

		public OpCodeHandler_MVEX_KHW(Code code) {
			Debug.Assert(!new MvexInfo(code).IgnoresOpMaskRegister);
			Debug.Assert(new MvexInfo(code).CanUseEvictionHint);
			this.code = code;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.MVEX);
			instruction.Code = code;

			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = (int)state.reg + Register.K0;
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op1Kind = OpKind.Register;
			instruction.Op1Register = (int)state.vvvv + Register.ZMM0;
			var mvex = new MvexInfo(code);
			var sss = state.Sss;
			if (state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op2Kind = OpKind.Register;
				instruction.Op2Register = (int)(state.rm + state.extraBaseRegisterBaseEVEX) + Register.ZMM0;
				if ((state.flags & StateFlags.MvexEH) != 0) {
					if (mvex.CanUseSuppressAllExceptions) {
						if ((sss & 4) != 0)
							instruction.InternalSetSuppressAllExceptions();
						if (mvex.CanUseRoundingControl) {
							Static.Assert((int)RoundingControl.None == 0 ? 0 : -1);
							Static.Assert((int)RoundingControl.RoundToNearest == 1 ? 0 : -1);
							Static.Assert((int)RoundingControl.RoundDown == 2 ? 0 : -1);
							Static.Assert((int)RoundingControl.RoundUp == 3 ? 0 : -1);
							Static.Assert((int)RoundingControl.RoundTowardZero == 4 ? 0 : -1);
							instruction.InternalRoundingControl = ((uint)sss & 3) + (uint)RoundingControl.RoundToNearest;
						}
					}
					else if (mvex.NoSaeRc && ((uint)sss & decoder.invalidCheckMask) != 0)
						decoder.SetInvalidInstruction();
				}
				else {
					if ((mvex.InvalidSwizzleFns & (1U << sss) & decoder.invalidCheckMask) != 0)
						decoder.SetInvalidInstruction();
					Debug.Assert((uint)sss <= 7);
					instruction.InternalSetMvexRegMemConv(MvexRegMemConv.RegSwizzleNone + sss);
				}
			}
			else {
				instruction.Op2Kind = OpKind.Memory;
				if ((state.flags & StateFlags.MvexEH) != 0)
					instruction.InternalSetIsMvexEvictionHint();
				if ((mvex.InvalidConvFns & (1U << sss) & decoder.invalidCheckMask) != 0)
					decoder.SetInvalidInstruction();
				instruction.InternalSetMvexRegMemConv(MvexRegMemConv.MemConvNone + sss);
				decoder.ReadOpMem(ref instruction, mvex.GetTupleType(sss));
			}
			if (((state.extraRegisterBase | state.extraRegisterBaseEVEX) & decoder.invalidCheckMask) != 0)
				decoder.SetInvalidInstruction();
		}
	}

	sealed class OpCodeHandler_MVEX_KHWIb : OpCodeHandlerModRM {
		readonly Code code;

		public OpCodeHandler_MVEX_KHWIb(Code code) {
			Debug.Assert(!new MvexInfo(code).IgnoresOpMaskRegister);
			Debug.Assert(new MvexInfo(code).CanUseEvictionHint);
			this.code = code;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.MVEX);
			instruction.Code = code;

			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = (int)state.reg + Register.K0;
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op1Kind = OpKind.Register;
			instruction.Op1Register = (int)state.vvvv + Register.ZMM0;
			var mvex = new MvexInfo(code);
			var sss = state.Sss;
			if (state.mod == 3) {
				Static.Assert(OpKind.Register == 0 ? 0 : -1);
				//instruction.Op2Kind = OpKind.Register;
				instruction.Op2Register = (int)(state.rm + state.extraBaseRegisterBaseEVEX) + Register.ZMM0;
				if ((state.flags & StateFlags.MvexEH) != 0) {
					if (mvex.CanUseSuppressAllExceptions) {
						if ((sss & 4) != 0)
							instruction.InternalSetSuppressAllExceptions();
						if (mvex.CanUseRoundingControl) {
							Static.Assert((int)RoundingControl.None == 0 ? 0 : -1);
							Static.Assert((int)RoundingControl.RoundToNearest == 1 ? 0 : -1);
							Static.Assert((int)RoundingControl.RoundDown == 2 ? 0 : -1);
							Static.Assert((int)RoundingControl.RoundUp == 3 ? 0 : -1);
							Static.Assert((int)RoundingControl.RoundTowardZero == 4 ? 0 : -1);
							instruction.InternalRoundingControl = ((uint)sss & 3) + (uint)RoundingControl.RoundToNearest;
						}
					}
					else if (mvex.NoSaeRc && ((uint)sss & decoder.invalidCheckMask) != 0)
						decoder.SetInvalidInstruction();
				}
				else {
					if ((mvex.InvalidSwizzleFns & (1U << sss) & decoder.invalidCheckMask) != 0)
						decoder.SetInvalidInstruction();
					Debug.Assert((uint)sss <= 7);
					instruction.InternalSetMvexRegMemConv(MvexRegMemConv.RegSwizzleNone + sss);
				}
			}
			else {
				instruction.Op2Kind = OpKind.Memory;
				if ((state.flags & StateFlags.MvexEH) != 0)
					instruction.InternalSetIsMvexEvictionHint();
				if ((mvex.InvalidConvFns & (1U << sss) & decoder.invalidCheckMask) != 0)
					decoder.SetInvalidInstruction();
				instruction.InternalSetMvexRegMemConv(MvexRegMemConv.MemConvNone + sss);
				decoder.ReadOpMem(ref instruction, mvex.GetTupleType(sss));
			}
			if (((state.extraRegisterBase | state.extraRegisterBaseEVEX) & decoder.invalidCheckMask) != 0)
				decoder.SetInvalidInstruction();
			instruction.Op3Kind = OpKind.Immediate8;
			instruction.Immediate8 = (byte)decoder.ReadByte();
		}
	}

	sealed class OpCodeHandler_MVEX_VSIB : OpCodeHandlerModRM {
		readonly Code code;

		public OpCodeHandler_MVEX_VSIB(Code code) {
			Debug.Assert(!new MvexInfo(code).IgnoresOpMaskRegister);
			Debug.Assert(new MvexInfo(code).CanUseEvictionHint);
			this.code = code;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.MVEX);
			if (decoder.invalidCheckMask != 0 && ((state.vvvv_invalidCheck & 0xF) != 0 || state.aaa == 0))
				decoder.SetInvalidInstruction();
			instruction.Code = code;

			var mvex = new MvexInfo(code);
			var sss = state.Sss;
			if (state.mod == 3)
				decoder.SetInvalidInstruction();
			else {
				instruction.Op0Kind = OpKind.Memory;
				if ((state.flags & StateFlags.MvexEH) != 0)
					instruction.InternalSetIsMvexEvictionHint();
				if ((mvex.InvalidConvFns & (1U << sss) & decoder.invalidCheckMask) != 0)
					decoder.SetInvalidInstruction();
				instruction.InternalSetMvexRegMemConv(MvexRegMemConv.MemConvNone + sss);
				decoder.ReadOpMem_VSIB(ref instruction, Register.ZMM0, mvex.GetTupleType(sss));
			}
		}
	}

	sealed class OpCodeHandler_MVEX_VSIB_V : OpCodeHandlerModRM {
		readonly Code code;

		public OpCodeHandler_MVEX_VSIB_V(Code code) {
			Debug.Assert(!new MvexInfo(code).IgnoresOpMaskRegister);
			Debug.Assert(new MvexInfo(code).CanUseEvictionHint);
			this.code = code;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.MVEX);
			if (decoder.invalidCheckMask != 0 && ((state.vvvv_invalidCheck & 0xF) != 0 || state.aaa == 0))
				decoder.SetInvalidInstruction();
			instruction.Code = code;

			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op1Kind = OpKind.Register;
			instruction.Op1Register = (int)(state.reg + state.extraRegisterBase + state.extraRegisterBaseEVEX) + Register.ZMM0;
			var mvex = new MvexInfo(code);
			var sss = state.Sss;
			if (state.mod == 3)
				decoder.SetInvalidInstruction();
			else {
				instruction.Op0Kind = OpKind.Memory;
				if ((state.flags & StateFlags.MvexEH) != 0)
					instruction.InternalSetIsMvexEvictionHint();
				if ((mvex.InvalidConvFns & (1U << sss) & decoder.invalidCheckMask) != 0)
					decoder.SetInvalidInstruction();
				instruction.InternalSetMvexRegMemConv(MvexRegMemConv.MemConvNone + sss);
				decoder.ReadOpMem_VSIB(ref instruction, Register.ZMM0, mvex.GetTupleType(sss));
			}
		}
	}

	sealed class OpCodeHandler_MVEX_V_VSIB : OpCodeHandlerModRM {
		readonly Code code;

		public OpCodeHandler_MVEX_V_VSIB(Code code) {
			Debug.Assert(!new MvexInfo(code).IgnoresOpMaskRegister);
			Debug.Assert(new MvexInfo(code).CanUseEvictionHint);
			this.code = code;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.MVEX);
			if (decoder.invalidCheckMask != 0 && ((state.vvvv_invalidCheck & 0xF) != 0 || state.aaa == 0))
				decoder.SetInvalidInstruction();
			instruction.Code = code;

			int regNum = (int)(state.reg + state.extraRegisterBase + state.extraRegisterBaseEVEX);
			Static.Assert(OpKind.Register == 0 ? 0 : -1);
			//instruction.Op0Kind = OpKind.Register;
			instruction.Op0Register = regNum + Register.ZMM0;
			var mvex = new MvexInfo(code);
			var sss = state.Sss;
			if (state.mod == 3)
				decoder.SetInvalidInstruction();
			else {
				instruction.Op1Kind = OpKind.Memory;
				if ((state.flags & StateFlags.MvexEH) != 0)
					instruction.InternalSetIsMvexEvictionHint();
				if ((mvex.InvalidConvFns & (1U << sss) & decoder.invalidCheckMask) != 0)
					decoder.SetInvalidInstruction();
				instruction.InternalSetMvexRegMemConv(MvexRegMemConv.MemConvNone + sss);
				decoder.ReadOpMem_VSIB(ref instruction, Register.ZMM0, mvex.GetTupleType(sss));
				if (decoder.invalidCheckMask != 0) {
					if ((uint)regNum == ((uint)(instruction.MemoryIndex - Register.XMM0) % (uint)IcedConstants.VMM_count))
						decoder.SetInvalidInstruction();
				}
			}
		}
	}
}
#endif
