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

	sealed class OpCodeHandler_Group8x8 : OpCodeHandlerModRM {
		readonly OpCodeHandler[] tableLow;
		readonly OpCodeHandler[] tableHigh;

		public OpCodeHandler_Group8x8(OpCodeHandler[] tableLow, OpCodeHandler[] tableHigh) {
			if (tableLow.Length != 8)
				throw new ArgumentOutOfRangeException(nameof(tableLow));
			if (tableHigh.Length != 8)
				throw new ArgumentOutOfRangeException(nameof(tableHigh));
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
		readonly OpCodeHandler?[] tableHigh;

		public OpCodeHandler_Group8x64(OpCodeHandler[] tableLow, OpCodeHandler?[] tableHigh) {
			if (tableLow.Length != 8)
				throw new ArgumentOutOfRangeException(nameof(tableLow));
			if (tableHigh.Length != 64)
				throw new ArgumentOutOfRangeException(nameof(tableHigh));
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

	sealed class OpCodeHandler_MandatoryPrefix2 : OpCodeHandlerModRM {
		readonly OpCodeHandler[] handlers;

		public OpCodeHandler_MandatoryPrefix2(OpCodeHandler handler)
			: this(handler, OpCodeHandler_Invalid.Instance, OpCodeHandler_Invalid.Instance, OpCodeHandler_Invalid.Instance) { }

		public OpCodeHandler_MandatoryPrefix2(OpCodeHandler handler, OpCodeHandler handler66, OpCodeHandler handlerF3, OpCodeHandler handlerF2) {
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
			Debug.Assert(
				decoder.state.Encoding == EncodingKind.VEX ||
				decoder.state.Encoding == EncodingKind.EVEX ||
				decoder.state.Encoding == EncodingKind.XOP);
			handlers[(int)decoder.state.mandatoryPrefix].Decode(decoder, ref instruction);
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

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			Debug.Assert(
				decoder.state.Encoding == EncodingKind.VEX ||
				decoder.state.Encoding == EncodingKind.EVEX ||
				decoder.state.Encoding == EncodingKind.XOP);
			((decoder.state.flags & StateFlags.W) != 0 ? handlerW1 : handlerW0).Decode(decoder, ref instruction);
		}
	}

	sealed class OpCodeHandler_Bitness : OpCodeHandler {
		readonly OpCodeHandler handler1632;
		readonly OpCodeHandler handler64;

		public OpCodeHandler_Bitness(OpCodeHandler handler1632, OpCodeHandler handler64) {
			this.handler1632 = handler1632;
			this.handler64 = handler64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			OpCodeHandler handler;
			if (decoder.is64Mode)
				handler = handler64;
			else
				handler = handler1632;
			if (handler.HasModRM)
				decoder.ReadModRM();
			handler.Decode(decoder, ref instruction);
		}
	}

	sealed class OpCodeHandler_Bitness_DontReadModRM : OpCodeHandlerModRM {
		readonly OpCodeHandler handler1632;
		readonly OpCodeHandler handler64;

		public OpCodeHandler_Bitness_DontReadModRM(OpCodeHandler handler1632, OpCodeHandler handler64) {
			this.handler1632 = handler1632;
			this.handler64 = handler64;
		}

		public override void Decode(Decoder decoder, ref Instruction instruction) {
			OpCodeHandler handler;
			if (decoder.is64Mode)
				handler = handler64;
			else
				handler = handler1632;
			handler.Decode(decoder, ref instruction);
		}
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
}
#endif
