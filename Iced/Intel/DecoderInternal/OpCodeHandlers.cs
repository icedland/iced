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

#if (!NO_DECODER32 || !NO_DECODER64) && !NO_DECODER
using System;
using System.Diagnostics;

namespace Iced.Intel.DecoderInternal {
	enum HandlerFlags : uint {
		None					= 0,
		Xacquire				= 0x00000001,
		Xrelease				= 0x00000002,
		XacquireRelease			= Xacquire | Xrelease,
		XacquireReleaseNoLock	= 0x00000004,
	}

	abstract class OpCodeHandler {
		public readonly bool HasModRM;

		protected OpCodeHandler() { }
		protected OpCodeHandler(bool hasModRM) => HasModRM = hasModRM;

		public abstract void Decode(Decoder decoder, ref Instruction instruction);
	}

	abstract class OpCodeHandlerModRM : OpCodeHandler {
		protected OpCodeHandlerModRM() : base(true) { }
	}

	sealed class OpCodeHandler_Invalid : OpCodeHandlerModRM {
		public static readonly OpCodeHandler_Invalid Instance = new OpCodeHandler_Invalid();
		OpCodeHandler_Invalid() { }
		public override void Decode(Decoder decoder, ref Instruction instruction) => decoder.SetInvalidInstruction();
	}

	sealed class OpCodeHandler_Invalid_NoModRM : OpCodeHandler {
		public static readonly OpCodeHandler_Invalid_NoModRM Instance = new OpCodeHandler_Invalid_NoModRM();
		OpCodeHandler_Invalid_NoModRM() { }
		public override void Decode(Decoder decoder, ref Instruction instruction) => decoder.SetInvalidInstruction();
	}

	sealed class OpCodeHandler_Simple : OpCodeHandler {
		readonly Code code;

		public OpCodeHandler_Simple(Code code) => this.code = code;

		public override void Decode(Decoder decoder, ref Instruction instruction) => instruction.InternalCode = code;
	}

	sealed class OpCodeHandler_Reg : OpCodeHandler {
		readonly Code code;
		readonly Register reg;

		public OpCodeHandler_Reg(Code code, Register reg) {
			this.code = code;
			this.reg = reg;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			instruction.InternalCode = code;
			instruction.InternalOpCount = 1;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
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
			instruction.InternalCode = code;
			instruction.InternalOpCount = 2;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.Op0Register = reg;
			instruction.InternalOp1Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadIb();
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
			instruction.InternalCode = code;
			instruction.InternalOpCount = 2;
			instruction.InternalOp0Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadIb();
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.Op1Register = reg;
		}
	}

	sealed class OpCodeHandler_AL_DX : OpCodeHandler {
		readonly Code code;

		public OpCodeHandler_AL_DX(Code code) => this.code = code;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			instruction.InternalCode = code;
			instruction.InternalOpCount = 2;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.Op0Register = Register.AL;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.Op1Register = Register.DX;
		}
	}

	sealed class OpCodeHandler_DX_AL : OpCodeHandler {
		readonly Code code;

		public OpCodeHandler_DX_AL(Code code) => this.code = code;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			instruction.InternalCode = code;
			instruction.InternalOpCount = 2;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.Op0Register = Register.DX;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.Op1Register = Register.AL;
		}
	}

	sealed class OpCodeHandler_Ib : OpCodeHandler {
		readonly Code code;

		public OpCodeHandler_Ib(Code code) => this.code = code;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			instruction.InternalCode = code;
			instruction.InternalOpCount = 1;
			instruction.InternalOp0Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadIb();
		}
	}

	sealed class OpCodeHandler_Ib3 : OpCodeHandlerModRM {
		readonly Code code;

		public OpCodeHandler_Ib3(Code code) => this.code = code;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			instruction.InternalCode = code;
			instruction.InternalOpCount = 1;
			instruction.InternalOp0Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadIb();
		}
	}

	sealed class OpCodeHandler_Group8x8 : OpCodeHandlerModRM {
		readonly OpCodeHandler[] tableLow;
		readonly OpCodeHandler[] tableHigh;

		public OpCodeHandler_Group8x8(OpCodeHandler[] tableLow, OpCodeHandler[] tableHigh) {
			if (tableLow.Length != 8)
				throw new ArgumentException(nameof(tableLow));
			if (tableHigh.Length != 8)
				throw new ArgumentException(nameof(tableHigh));
			this.tableLow = tableLow;
			this.tableHigh = tableHigh;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			OpCodeHandler handler;
			if (state.mod == 3)
				handler = tableHigh[state.reg];
			else
				handler = tableLow[state.reg];
			handler.Decode(decoder, ref instruction);
		}
	}

	sealed class OpCodeHandler_Group8x64 : OpCodeHandlerModRM {
		readonly OpCodeHandler[] tableLow;
		readonly OpCodeHandler[] tableHigh;

		public OpCodeHandler_Group8x64(OpCodeHandler[] tableLow, OpCodeHandler[] tableHigh) {
			if (tableLow.Length != 8)
				throw new ArgumentException(nameof(tableLow));
			if (tableHigh.Length != 64)
				throw new ArgumentException(nameof(tableHigh));
			this.tableLow = tableLow;
			this.tableHigh = tableHigh;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			OpCodeHandler handler;
			if (state.mod == 3) {
				// A handler can be null in tableHigh, useful in 0F01 table and similar tables
				handler = tableHigh[state.modrm & 0x3F] ?? tableLow[state.reg];
			}
			else
				handler = tableLow[state.reg];
			handler.Decode(decoder, ref instruction);
		}
	}

	sealed class OpCodeHandler_Group : OpCodeHandlerModRM {
		readonly OpCodeHandler[] groupHandlers;
		public OpCodeHandler_Group(OpCodeHandler[] groupHandlers) => this.groupHandlers = groupHandlers ?? throw new ArgumentNullException(nameof(groupHandlers));
		public override void Decode(Decoder decoder, ref Instruction instruction) => groupHandlers[decoder.state.reg].Decode(decoder, ref instruction);
	}

	sealed class OpCodeHandler_AnotherTable : OpCodeHandler {
		readonly OpCodeHandler[] otherTable;
		public OpCodeHandler_AnotherTable(OpCodeHandler[] otherTable) => this.otherTable = otherTable ?? throw new ArgumentNullException(nameof(otherTable));
		public override void Decode(Decoder decoder, ref Instruction instruction) => decoder.DecodeTable(otherTable, ref instruction);
	}

	sealed class OpCodeHandler_MandatoryPrefix : OpCodeHandlerModRM {
		readonly OpCodeHandler[] handlers;

		public OpCodeHandler_MandatoryPrefix(OpCodeHandler handler, OpCodeHandler handler66, OpCodeHandler handlerF3, OpCodeHandler handlerF2) {
			Debug.Assert((int)MandatoryPrefix.None == 0);
			Debug.Assert((int)MandatoryPrefix.P66 == 1);
			Debug.Assert((int)MandatoryPrefix.PF3 == 2);
			Debug.Assert((int)MandatoryPrefix.PF2 == 3);
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

	[Flags]
	enum LegacyHandlerFlags {
		HandlerReg				= 0x00000001,
		HandlerMem				= 0x00000002,
		Handler66Reg			= 0x00000004,
		Handler66Mem			= 0x00000008,
		HandlerF3Reg			= 0x00000010,
		HandlerF3Mem			= 0x00000020,
		HandlerF2Reg			= 0x00000040,
		HandlerF2Mem			= 0x00000080,
	}

	sealed class OpCodeHandler_MandatoryPrefix3 : OpCodeHandlerModRM {
		readonly (OpCodeHandler handler, bool mandatoryPrefix)[] handlers_reg;
		readonly (OpCodeHandler handler, bool mandatoryPrefix)[] handlers_mem;

		public OpCodeHandler_MandatoryPrefix3(OpCodeHandler handler_reg, OpCodeHandler handler_mem, OpCodeHandler handler66_reg, OpCodeHandler handler66_mem, OpCodeHandler handlerF3_reg, OpCodeHandler handlerF3_mem, OpCodeHandler handlerF2_reg, OpCodeHandler handlerF2_mem, LegacyHandlerFlags flags) {
			Debug.Assert((int)MandatoryPrefix.None == 0);
			Debug.Assert((int)MandatoryPrefix.P66 == 1);
			Debug.Assert((int)MandatoryPrefix.PF3 == 2);
			Debug.Assert((int)MandatoryPrefix.PF2 == 3);
			handlers_reg = new(OpCodeHandler handler, bool mandatoryPrefix)[4] {
				(handler_reg ?? throw new ArgumentNullException(nameof(handler_reg)), (flags & LegacyHandlerFlags.HandlerReg) == 0),
				(handler66_reg ?? throw new ArgumentNullException(nameof(handler66_reg)), (flags & LegacyHandlerFlags.Handler66Reg) == 0),
				(handlerF3_reg ?? throw new ArgumentNullException(nameof(handlerF3_reg)), (flags & LegacyHandlerFlags.HandlerF3Reg) == 0),
				(handlerF2_reg ?? throw new ArgumentNullException(nameof(handlerF2_reg)), (flags & LegacyHandlerFlags.HandlerF2Reg) == 0),
			};
			handlers_mem = new(OpCodeHandler handler, bool mandatoryPrefix)[4] {
				(handler_mem ?? throw new ArgumentNullException(nameof(handler_mem)), (flags & LegacyHandlerFlags.HandlerMem) == 0),
				(handler66_mem ?? throw new ArgumentNullException(nameof(handler66_mem)), (flags & LegacyHandlerFlags.Handler66Mem) == 0),
				(handlerF3_mem ?? throw new ArgumentNullException(nameof(handlerF3_mem)), (flags & LegacyHandlerFlags.HandlerF3Mem) == 0),
				(handlerF2_mem ?? throw new ArgumentNullException(nameof(handlerF2_mem)), (flags & LegacyHandlerFlags.HandlerF2Mem) == 0),
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

	sealed class OpCodeHandler_MandatoryPrefix_F3_F2 : OpCodeHandlerModRM {
		readonly OpCodeHandler handlerNormal;
		readonly OpCodeHandler handlerF3;
		readonly OpCodeHandler handlerF2;

		public OpCodeHandler_MandatoryPrefix_F3_F2(OpCodeHandler handlerNormal, OpCodeHandler handlerF3, OpCodeHandler handlerF2) {
			this.handlerNormal = handlerNormal ?? throw new ArgumentNullException(nameof(handlerNormal));
			this.handlerF3 = handlerF3 ?? throw new ArgumentNullException(nameof(handlerF3));
			this.handlerF2 = handlerF2 ?? throw new ArgumentNullException(nameof(handlerF2));
			Debug.Assert(handlerNormal.HasModRM == HasModRM);
			Debug.Assert(handlerF3.HasModRM == HasModRM);
			Debug.Assert(handlerF2.HasModRM == HasModRM);
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			OpCodeHandler handler;
			var prefix = decoder.state.mandatoryPrefix;
			if (prefix == MandatoryPrefix.PF3) {
				decoder.ClearMandatoryPrefixF3(ref instruction);
				handler = handlerF3;
			}
			else if (prefix == MandatoryPrefix.PF2) {
				decoder.ClearMandatoryPrefixF2(ref instruction);
				handler = handlerF2;
			}
			else {
				Debug.Assert(prefix == MandatoryPrefix.None || prefix == MandatoryPrefix.P66);
				handler = handlerNormal;
			}
			handler.Decode(decoder, ref instruction);
		}
	}

	sealed class OpCodeHandler_MandatoryPrefix2 : OpCodeHandlerModRM {
		readonly OpCodeHandler[] handlers;

		public OpCodeHandler_MandatoryPrefix2(OpCodeHandler handler, OpCodeHandler handler66, OpCodeHandler handlerF3, OpCodeHandler handlerF2) {
			Debug.Assert((int)MandatoryPrefix.None == 0);
			Debug.Assert((int)MandatoryPrefix.P66 == 1);
			Debug.Assert((int)MandatoryPrefix.PF3 == 2);
			Debug.Assert((int)MandatoryPrefix.PF2 == 3);
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
			Debug.Assert(decoder.state.Encoding == EncodingKind.VEX || decoder.state.Encoding == EncodingKind.EVEX);
			handlers[(int)decoder.state.mandatoryPrefix].Decode(decoder, ref instruction);
		}
	}

	sealed class OpCodeHandler_MandatoryPrefix2_NoModRM : OpCodeHandler {
		readonly OpCodeHandler[] handlers;

		public OpCodeHandler_MandatoryPrefix2_NoModRM(OpCodeHandler handler, OpCodeHandler handler66, OpCodeHandler handlerF3, OpCodeHandler handlerF2) {
			Debug.Assert((int)MandatoryPrefix.None == 0);
			Debug.Assert((int)MandatoryPrefix.P66 == 1);
			Debug.Assert((int)MandatoryPrefix.PF3 == 2);
			Debug.Assert((int)MandatoryPrefix.PF2 == 3);
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
			Debug.Assert(decoder.state.Encoding == EncodingKind.VEX || decoder.state.Encoding == EncodingKind.EVEX);
			handlers[(int)decoder.state.mandatoryPrefix].Decode(decoder, ref instruction);
		}
	}

	sealed class OpCodeHandler_MandatoryPrefix_MaybeModRM : OpCodeHandler {
		readonly OpCodeHandler[] handlers;

		public OpCodeHandler_MandatoryPrefix_MaybeModRM(OpCodeHandler handler, OpCodeHandler handler66, OpCodeHandler handlerF3, OpCodeHandler handlerF2) {
			Debug.Assert((int)MandatoryPrefix.None == 0);
			Debug.Assert((int)MandatoryPrefix.P66 == 1);
			Debug.Assert((int)MandatoryPrefix.PF3 == 2);
			Debug.Assert((int)MandatoryPrefix.PF2 == 3);
			handlers = new OpCodeHandler[4] {
				handler ?? throw new ArgumentNullException(nameof(handler)),
				handler66 ?? throw new ArgumentNullException(nameof(handler66)),
				handlerF3 ?? throw new ArgumentNullException(nameof(handlerF3)),
				handlerF2 ?? throw new ArgumentNullException(nameof(handlerF2)),
			};
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(decoder.state.Encoding == EncodingKind.Legacy);
			decoder.ClearMandatoryPrefix(ref instruction);
			var handler = handlers[(int)decoder.state.mandatoryPrefix];
			if (handler.HasModRM)
				decoder.ReadModRM();
			handler.Decode(decoder, ref instruction);
		}
	}

	sealed class OpCodeHandler_VectorLength_VEX : OpCodeHandlerModRM {
		readonly OpCodeHandler[] handlers;

		public OpCodeHandler_VectorLength_VEX(OpCodeHandler handler128, OpCodeHandler handler256) {
			Debug.Assert((int)VectorLength.L128 == 0);
			Debug.Assert((int)VectorLength.L256 == 1);
			Debug.Assert((int)VectorLength.L512 == 2);
			Debug.Assert((int)VectorLength.Unknown == 3);
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
			Debug.Assert(decoder.state.Encoding == EncodingKind.VEX);
			handlers[(int)decoder.state.vectorLength].Decode(decoder, ref instruction);
		}
	}

	sealed class OpCodeHandler_VectorLength_NoModRM_VEX : OpCodeHandler {
		readonly OpCodeHandler[] handlers;

		public OpCodeHandler_VectorLength_NoModRM_VEX(OpCodeHandler handler128, OpCodeHandler handler256) {
			Debug.Assert((int)VectorLength.L128 == 0);
			Debug.Assert((int)VectorLength.L256 == 1);
			Debug.Assert((int)VectorLength.L512 == 2);
			Debug.Assert((int)VectorLength.Unknown == 3);
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
			Debug.Assert(decoder.state.Encoding == EncodingKind.VEX);
			handlers[(int)decoder.state.vectorLength].Decode(decoder, ref instruction);
		}
	}

	sealed class OpCodeHandler_VectorLength_EVEX : OpCodeHandlerModRM {
		readonly OpCodeHandler[] handlers;

		public OpCodeHandler_VectorLength_EVEX(OpCodeHandler handler128, OpCodeHandler handler256, OpCodeHandler handler512) {
			Debug.Assert((int)VectorLength.L128 == 0);
			Debug.Assert((int)VectorLength.L256 == 1);
			Debug.Assert((int)VectorLength.L512 == 2);
			Debug.Assert((int)VectorLength.Unknown == 3);
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
			Debug.Assert((int)VectorLength.L128 == 0);
			Debug.Assert((int)VectorLength.L256 == 1);
			Debug.Assert((int)VectorLength.L512 == 2);
			Debug.Assert((int)VectorLength.Unknown == 3);
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
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.EVEX);
			int index = (int)state.vectorLength;
			if (state.mod == 3 && (state.flags & StateFlags.b) != 0)
				index = (int)VectorLength.L512;
			handlers[index].Decode(decoder, ref instruction);
		}
	}

	sealed class OpCodeHandler_W : OpCodeHandlerModRM {
		readonly OpCodeHandler handlerW0;
		readonly OpCodeHandler handlerW1;

		public OpCodeHandler_W(OpCodeHandler handlerW0, OpCodeHandler handlerW1) {
			this.handlerW0 = handlerW0 ?? throw new ArgumentNullException(nameof(handlerW0));
			this.handlerW1 = handlerW1 ?? throw new ArgumentNullException(nameof(handlerW1));
			Debug.Assert(handlerW0.HasModRM == HasModRM);
			Debug.Assert(handlerW1.HasModRM == HasModRM);
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) => ((decoder.state.flags & StateFlags.W) != 0 ? handlerW1 : handlerW0).Decode(decoder, ref instruction);
	}

	sealed class OpCodeHandler_RM : OpCodeHandlerModRM {
		readonly OpCodeHandler reg;
		readonly OpCodeHandler mem;

		public OpCodeHandler_RM(OpCodeHandler reg, OpCodeHandler mem) {
			this.reg = reg ?? throw new ArgumentNullException(nameof(reg));
			this.mem = mem ?? throw new ArgumentNullException(nameof(mem));
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) => (decoder.state.mod == 3 ? reg : mem).Decode(decoder, ref instruction);
	}

	sealed class OpCodeHandler_NIb : OpCodeHandlerModRM {
		readonly Code code;

		public OpCodeHandler_NIb(Code code) => this.code = code;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			ref var state = ref decoder.state;
			Debug.Assert(state.Encoding == EncodingKind.Legacy);
			instruction.InternalCode = code;
			instruction.InternalOpCount = 2;
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.Op0Register = (int)state.rm + Register.MM0;
			}
			else
				decoder.SetInvalidInstruction();
			instruction.InternalOp1Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadByte();
		}
	}
}
#endif
