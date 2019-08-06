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

#if (!NO_DECODER32 || !NO_DECODER64) && !NO_DECODER
using System;
using System.Diagnostics;

namespace Iced.Intel.DecoderInternal {
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

	sealed class OpCodeHandler_Simple_ModRM : OpCodeHandlerModRM {
		readonly Code code;
		public OpCodeHandler_Simple_ModRM(Code code) => this.code = code;
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
			Debug.Assert(OpKind.Register == 0);
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
			instruction.InternalCode = code;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = reg;
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
			instruction.InternalOp0Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadIb();
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.InternalOp1Register = reg;
		}
	}

	sealed class OpCodeHandler_AL_DX : OpCodeHandler {
		readonly Code code;

		public OpCodeHandler_AL_DX(Code code) => this.code = code;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			instruction.InternalCode = code;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = Register.AL;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.InternalOp1Register = Register.DX;
		}
	}

	sealed class OpCodeHandler_DX_AL : OpCodeHandler {
		readonly Code code;

		public OpCodeHandler_DX_AL(Code code) => this.code = code;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			instruction.InternalCode = code;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp0Kind = OpKind.Register;
			instruction.InternalOp0Register = Register.DX;
			Debug.Assert(OpKind.Register == 0);
			//instruction.InternalOp1Kind = OpKind.Register;
			instruction.InternalOp1Register = Register.AL;
		}
	}

	sealed class OpCodeHandler_Ib : OpCodeHandler {
		readonly Code code;

		public OpCodeHandler_Ib(Code code) => this.code = code;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			instruction.InternalCode = code;
			instruction.InternalOp0Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadIb();
		}
	}

	sealed class OpCodeHandler_Ib3 : OpCodeHandlerModRM {
		readonly Code code;

		public OpCodeHandler_Ib3(Code code) => this.code = code;

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			instruction.InternalCode = code;
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
			Debug.Assert((int)MandatoryPrefix.None == 0);
			Debug.Assert((int)MandatoryPrefix.P66 == 1);
			Debug.Assert((int)MandatoryPrefix.PF3 == 2);
			Debug.Assert((int)MandatoryPrefix.PF2 == 3);
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

		public OpCodeHandler_MandatoryPrefix_F3_F2(OpCodeHandler handlerNormal, OpCodeHandler handlerF3, OpCodeHandler handlerF2) {
			this.handlerNormal = handlerNormal ?? throw new ArgumentNullException(nameof(handlerNormal));
			this.handlerF3 = handlerF3 ?? throw new ArgumentNullException(nameof(handlerF3));
			this.handlerF2 = handlerF2 ?? throw new ArgumentNullException(nameof(handlerF2));
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
			if (handler.HasModRM)
				decoder.ReadModRM();
			handler.Decode(decoder, ref instruction);
		}
	}

	sealed class OpCodeHandler_MandatoryPrefix2 : OpCodeHandlerModRM {
		readonly OpCodeHandler[] handlers;

		public OpCodeHandler_MandatoryPrefix2(OpCodeHandler handler)
			: this(handler, OpCodeHandler_Invalid.Instance, OpCodeHandler_Invalid.Instance, OpCodeHandler_Invalid.Instance) { }

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
			Debug.Assert(
				decoder.state.Encoding == EncodingKind.VEX ||
				decoder.state.Encoding == EncodingKind.EVEX ||
				decoder.state.Encoding == EncodingKind.XOP);
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
			Debug.Assert(
				decoder.state.Encoding == EncodingKind.VEX ||
				decoder.state.Encoding == EncodingKind.EVEX ||
				decoder.state.Encoding == EncodingKind.XOP);
			handlers[(int)decoder.state.mandatoryPrefix].Decode(decoder, ref instruction);
		}
	}

	sealed class OpCodeHandler_MandatoryPrefix_NoModRM : OpCodeHandler {
		readonly OpCodeHandler[] handlers;

		public OpCodeHandler_MandatoryPrefix_NoModRM(OpCodeHandler handler, OpCodeHandler handler66, OpCodeHandler handlerF3, OpCodeHandler handlerF2) {
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
			Debug.Assert(decoder.state.Encoding == EncodingKind.VEX || decoder.state.Encoding == EncodingKind.XOP);
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
			Debug.Assert(decoder.state.Encoding == EncodingKind.VEX || decoder.state.Encoding == EncodingKind.XOP);
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
			if (state.mod == 3) {
				Debug.Assert(OpKind.Register == 0);
				//instruction.InternalOp0Kind = OpKind.Register;
				instruction.InternalOp0Register = (int)state.rm + Register.MM0;
			}
			else
				decoder.SetInvalidInstruction();
			instruction.InternalOp1Kind = OpKind.Immediate8;
			instruction.InternalImmediate8 = decoder.ReadByte();
		}
	}

	readonly struct HandlerOptions {
		public readonly OpCodeHandler handler;
		public readonly DecoderOptions options;
		public HandlerOptions(OpCodeHandler handler, DecoderOptions options) {
			this.handler = handler;
			this.options = options;
		}
	}

	sealed class OpCodeHandler_Options : OpCodeHandler {
		readonly OpCodeHandler defaultHandler;
		readonly HandlerOptions[] infos;

		public OpCodeHandler_Options(OpCodeHandler defaultHandler, OpCodeHandler handler1, DecoderOptions options1) {
			this.defaultHandler = defaultHandler ?? throw new ArgumentNullException(nameof(defaultHandler));
			infos = new HandlerOptions[] {
				new HandlerOptions(handler1, options1),
			};
		}

		public OpCodeHandler_Options(OpCodeHandler defaultHandler, OpCodeHandler handler1, DecoderOptions options1, OpCodeHandler handler2, DecoderOptions options2) {
			this.defaultHandler = defaultHandler ?? throw new ArgumentNullException(nameof(defaultHandler));
			infos = new HandlerOptions[] {
				new HandlerOptions(handler1 ?? throw new ArgumentNullException(nameof(handler1)), options1),
				new HandlerOptions(handler2 ?? throw new ArgumentNullException(nameof(handler2)), options2),
			};
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			var handler = defaultHandler;
			var options = decoder.options;
			foreach (var info in infos) {
				if ((options & info.options) != 0) {
					handler = info.handler;
					break;
				}
			}
			if (handler.HasModRM)
				decoder.ReadModRM();
			handler.Decode(decoder, ref instruction);
		}
	}

	sealed class OpCodeHandler_Options_DontReadModRM : OpCodeHandlerModRM {
		readonly OpCodeHandler defaultHandler;
		readonly HandlerOptions[] infos;

		public OpCodeHandler_Options_DontReadModRM(OpCodeHandler defaultHandler, OpCodeHandler handler1, DecoderOptions options1) {
			this.defaultHandler = defaultHandler ?? throw new ArgumentNullException(nameof(defaultHandler));
			infos = new HandlerOptions[] {
				new HandlerOptions(handler1, options1),
			};
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			var handler = defaultHandler;
			var options = decoder.options;
			foreach (var info in infos) {
				if ((options & info.options) != 0) {
					handler = info.handler;
					break;
				}
			}
			handler.Decode(decoder, ref instruction);
		}
	}

	sealed class OpCodeHandler_ReservedNop : OpCodeHandlerModRM {
		readonly OpCodeHandler reservedNopHandler;
		readonly OpCodeHandler otherHandler;

		public OpCodeHandler_ReservedNop(OpCodeHandler reservedNopHandler, OpCodeHandler otherHandler) {
			this.reservedNopHandler = reservedNopHandler ?? throw new ArgumentNullException(nameof(reservedNopHandler));
			this.otherHandler = otherHandler ?? throw new ArgumentNullException(nameof(otherHandler));
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) =>
			((decoder.options & DecoderOptions.ForceReservedNop) != 0 ? reservedNopHandler : otherHandler).Decode(decoder, ref instruction);
	}
}
#endif
