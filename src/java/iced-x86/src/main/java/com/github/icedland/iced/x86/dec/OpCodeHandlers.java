// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.dec;

import com.github.icedland.iced.x86.Instruction;
import com.github.icedland.iced.x86.internal.dec.StateFlags;

abstract class OpCodeHandler {
	final boolean hasModRM;

	protected OpCodeHandler() {
		hasModRM = false;
	}

	protected OpCodeHandler(boolean hasModRM) {
		this.hasModRM = hasModRM;
	}

	abstract void decode(Decoder decoder, Instruction instruction);
}

abstract class OpCodeHandlerModRM extends OpCodeHandler {
	protected OpCodeHandlerModRM() {
		super(true);
	}
}

final class OpCodeHandler_Invalid extends OpCodeHandlerModRM {
	static final OpCodeHandler_Invalid Instance = new OpCodeHandler_Invalid();

	OpCodeHandler_Invalid() {
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		decoder.setInvalidInstruction();
	}
}

final class OpCodeHandler_Invalid_NoModRM extends OpCodeHandler {
	static final OpCodeHandler_Invalid_NoModRM Instance = new OpCodeHandler_Invalid_NoModRM();

	OpCodeHandler_Invalid_NoModRM() {
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		decoder.setInvalidInstruction();
	}
}

final class OpCodeHandler_Simple extends OpCodeHandler {
	private final int code;

	OpCodeHandler_Simple(int code) {
		this.code = code;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		instruction.setCode(code);
	}
}

final class OpCodeHandler_Simple_ModRM extends OpCodeHandlerModRM {
	private final int code;

	OpCodeHandler_Simple_ModRM(int code) {
		this.code = code;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		instruction.setCode(code);
	}
}

final class OpCodeHandler_Group8x8 extends OpCodeHandlerModRM {
	private final OpCodeHandler[] tableLow;
	private final OpCodeHandler[] tableHigh;

	OpCodeHandler_Group8x8(OpCodeHandler[] tableLow, OpCodeHandler[] tableHigh) {
		if (tableLow.length != 8)
			throw new IllegalArgumentException("tableLow");
		if (tableHigh.length != 8)
			throw new IllegalArgumentException("tableHigh");
		this.tableLow = tableLow;
		this.tableHigh = tableHigh;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		OpCodeHandler handler;
		if (decoder.state_mod == 3)
			handler = tableHigh[decoder.state_reg];
		else
			handler = tableLow[decoder.state_reg];
		handler.decode(decoder, instruction);
	}
}

final class OpCodeHandler_Group8x64 extends OpCodeHandlerModRM {
	private final OpCodeHandler[] tableLow;
	private final OpCodeHandler[] tableHigh;

	OpCodeHandler_Group8x64(OpCodeHandler[] tableLow, OpCodeHandler[] tableHigh) {
		if (tableLow.length != 8)
			throw new IllegalArgumentException("tableLow");
		if (tableHigh.length != 64)
			throw new IllegalArgumentException("tableHigh");
		this.tableLow = tableLow;
		this.tableHigh = tableHigh;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		OpCodeHandler handler;
		if (decoder.state_mod == 3) {
			// A handler can be null in tableHigh, useful in 0F01 table and similar tables
			handler = tableHigh[decoder.state_modrm & 0x3F];
			if (handler == null)
				handler = tableLow[decoder.state_reg];
		}
		else
			handler = tableLow[decoder.state_reg];
		handler.decode(decoder, instruction);
	}
}

final class OpCodeHandler_Group extends OpCodeHandlerModRM {
	private final OpCodeHandler[] groupHandlers;

	OpCodeHandler_Group(OpCodeHandler[] groupHandlers) {
		if (groupHandlers == null)
			throw new NullPointerException();
		this.groupHandlers = groupHandlers;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		groupHandlers[decoder.state_reg].decode(decoder, instruction);
	}
}

final class OpCodeHandler_AnotherTable extends OpCodeHandler {
	private final OpCodeHandler[] otherTable;

	OpCodeHandler_AnotherTable(OpCodeHandler[] otherTable) {
		if (otherTable == null)
			throw new NullPointerException();
		this.otherTable = otherTable;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		decoder.decodeTable(otherTable, instruction);
	}
}

final class OpCodeHandler_MandatoryPrefix2 extends OpCodeHandlerModRM {
	private final OpCodeHandler[] handlers;

	OpCodeHandler_MandatoryPrefix2(OpCodeHandler handler) {
		this(handler, OpCodeHandler_Invalid.Instance, OpCodeHandler_Invalid.Instance, OpCodeHandler_Invalid.Instance);
	}

	OpCodeHandler_MandatoryPrefix2(OpCodeHandler handler, OpCodeHandler handler66, OpCodeHandler handlerF3, OpCodeHandler handlerF2) {
		if (handler == null)
			throw new NullPointerException();
		if (handler66 == null)
			throw new NullPointerException();
		if (handlerF3 == null)
			throw new NullPointerException();
		if (handlerF2 == null)
			throw new NullPointerException();
		handlers = new OpCodeHandler[] {
			handler,
			handler66,
			handlerF3,
			handlerF2,
		};
		assert handler.hasModRM == hasModRM;
		assert handler66.hasModRM == hasModRM;
		assert handlerF3.hasModRM == hasModRM;
		assert handlerF2.hasModRM == hasModRM;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		handlers[decoder.state_zs_mandatoryPrefix].decode(decoder, instruction);
	}
}

final class OpCodeHandler_MandatoryPrefix2_NoModRM extends OpCodeHandler {
	private final OpCodeHandler[] handlers;

	OpCodeHandler_MandatoryPrefix2_NoModRM(OpCodeHandler handler, OpCodeHandler handler66, OpCodeHandler handlerF3, OpCodeHandler handlerF2) {
		if (handler == null)
			throw new NullPointerException();
		if (handler66 == null)
			throw new NullPointerException();
		if (handlerF3 == null)
			throw new NullPointerException();
		if (handlerF2 == null)
			throw new NullPointerException();
		handlers = new OpCodeHandler[] {
			handler,
			handler66,
			handlerF3,
			handlerF2,
		};
		assert handler.hasModRM == hasModRM;
		assert handler66.hasModRM == hasModRM;
		assert handlerF3.hasModRM == hasModRM;
		assert handlerF2.hasModRM == hasModRM;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		handlers[decoder.state_zs_mandatoryPrefix].decode(decoder, instruction);
	}
}

final class OpCodeHandler_W extends OpCodeHandlerModRM {
	private final OpCodeHandler handlerW0;
	private final OpCodeHandler handlerW1;

	OpCodeHandler_W(OpCodeHandler handlerW0, OpCodeHandler handlerW1) {
		if (handlerW0 == null)
			throw new NullPointerException();
		if (handlerW1 == null)
			throw new NullPointerException();
		this.handlerW0 = handlerW0;
		this.handlerW1 = handlerW1;
		assert handlerW0.hasModRM == hasModRM;
		assert handlerW1.hasModRM == hasModRM;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		((decoder.state_zs_flags & StateFlags.W) != 0 ? handlerW1 : handlerW0).decode(decoder, instruction);
	}
}

final class OpCodeHandler_Bitness extends OpCodeHandler {
	private final OpCodeHandler handler1632;
	private final OpCodeHandler handler64;

	OpCodeHandler_Bitness(OpCodeHandler handler1632, OpCodeHandler handler64) {
		this.handler1632 = handler1632;
		this.handler64 = handler64;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		OpCodeHandler handler;
		if (decoder.is64bMode)
			handler = handler64;
		else
			handler = handler1632;
		if (handler.hasModRM)
			decoder.readModRM();
		handler.decode(decoder, instruction);
	}
}

final class OpCodeHandler_Bitness_DontReadModRM extends OpCodeHandlerModRM {
	private final OpCodeHandler handler1632;
	private final OpCodeHandler handler64;

	OpCodeHandler_Bitness_DontReadModRM(OpCodeHandler handler1632, OpCodeHandler handler64) {
		this.handler1632 = handler1632;
		this.handler64 = handler64;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		OpCodeHandler handler;
		if (decoder.is64bMode)
			handler = handler64;
		else
			handler = handler1632;
		handler.decode(decoder, instruction);
	}
}

final class OpCodeHandler_RM extends OpCodeHandlerModRM {
	private final OpCodeHandler reg;
	private final OpCodeHandler mem;

	OpCodeHandler_RM(OpCodeHandler reg, OpCodeHandler mem) {
		if (reg == null)
			throw new NullPointerException();
		if (mem == null)
			throw new NullPointerException();
		this.reg = reg;
		this.mem = mem;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		(decoder.state_mod == 3 ? reg : mem).decode(decoder, instruction);
	}
}

final class HandlerOptions {
	final OpCodeHandler handler;
	final int options;

	HandlerOptions(OpCodeHandler handler, int options) {
		this.handler = handler;
		this.options = options;
	}
}

final class OpCodeHandler_Options1632 extends OpCodeHandler {
	private final OpCodeHandler defaultHandler;
	private final HandlerOptions[] infos;
	private final int infoOptions;

	OpCodeHandler_Options1632(OpCodeHandler defaultHandler, OpCodeHandler handler1, int options1) {
		if (defaultHandler == null)
			throw new NullPointerException();
		this.defaultHandler = defaultHandler;
		infos = new HandlerOptions[] {
			new HandlerOptions(handler1, options1),
		};
		infoOptions = options1;
	}

	OpCodeHandler_Options1632(OpCodeHandler defaultHandler, OpCodeHandler handler1, int options1, OpCodeHandler handler2, int options2) {
		if (defaultHandler == null)
			throw new NullPointerException();
		if (handler1 == null)
			throw new NullPointerException();
		if (handler2 == null)
			throw new NullPointerException();
		this.defaultHandler = defaultHandler;
		infos = new HandlerOptions[] {
			new HandlerOptions(handler1, options1),
			new HandlerOptions(handler2, options2),
		};
		infoOptions = options1 | options2;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		OpCodeHandler handler = defaultHandler;
		int options = decoder.options;
		if (!decoder.is64bMode && (decoder.options & infoOptions) != 0) {
			for (HandlerOptions info : infos) {
				if ((options & info.options) != 0) {
					handler = info.handler;
					break;
				}
			}
		}
		if (handler.hasModRM)
			decoder.readModRM();
		handler.decode(decoder, instruction);
	}
}

final class OpCodeHandler_Options extends OpCodeHandler {
	private final OpCodeHandler defaultHandler;
	private final HandlerOptions[] infos;
	private final int infoOptions;

	OpCodeHandler_Options(OpCodeHandler defaultHandler, OpCodeHandler handler1, int options1) {
		if (defaultHandler == null)
			throw new NullPointerException();
		this.defaultHandler = defaultHandler;
		infos = new HandlerOptions[] {
			new HandlerOptions(handler1, options1),
		};
		infoOptions = options1;
	}

	OpCodeHandler_Options(OpCodeHandler defaultHandler, OpCodeHandler handler1, int options1, OpCodeHandler handler2, int options2) {
		if (defaultHandler == null)
			throw new NullPointerException();
		if (handler1 == null)
			throw new NullPointerException();
		if (handler2 == null)
			throw new NullPointerException();
		this.defaultHandler = defaultHandler;
		infos = new HandlerOptions[] {
			new HandlerOptions(handler1, options1),
			new HandlerOptions(handler2, options2),
		};
		infoOptions = options1 | options2;
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		OpCodeHandler handler = defaultHandler;
		int options = decoder.options;
		if ((decoder.options & infoOptions) != 0) {
			for (HandlerOptions info : infos) {
				if ((options & info.options) != 0) {
					handler = info.handler;
					break;
				}
			}
		}
		if (handler.hasModRM)
			decoder.readModRM();
		handler.decode(decoder, instruction);
	}
}

final class OpCodeHandler_Options_DontReadModRM extends OpCodeHandlerModRM {
	private final OpCodeHandler defaultHandler;
	private final HandlerOptions[] infos;

	OpCodeHandler_Options_DontReadModRM(OpCodeHandler defaultHandler, OpCodeHandler handler1, int options1) {
		if (defaultHandler == null)
			throw new NullPointerException();
		this.defaultHandler = defaultHandler;
		infos = new HandlerOptions[] {
			new HandlerOptions(handler1, options1),
		};
	}

	@Override
	void decode(Decoder decoder, Instruction instruction) {
		OpCodeHandler handler = defaultHandler;
		int options = decoder.options;
		for (HandlerOptions info : infos) {
			if ((options & info.options) != 0) {
				handler = info.handler;
				break;
			}
		}
		handler.decode(decoder, instruction);
	}
}
