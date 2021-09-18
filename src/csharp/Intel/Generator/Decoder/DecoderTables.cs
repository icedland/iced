// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.Collections.Generic;
using Generator.Enums;

namespace Generator.Decoder {
	[TypeGen(TypeGenOrders.PreCreateInstructions)]
	sealed class DecoderTables : ICreatedInstructions {
		public (string name, object?[] handlers)[] Legacy {
			get {
				if (!filtered)
					throw new InvalidOperationException();
				return legacy;
			}
		}

		public (string name, object?[] handlers)[] VEX {
			get {
				if (!filtered)
					throw new InvalidOperationException();
				return vex;
			}
		}

		public (string name, object?[] handlers)[] EVEX {
			get {
				if (!filtered)
					throw new InvalidOperationException();
				return evex;
			}
		}

		public (string name, object?[] handlers)[] XOP {
			get {
				if (!filtered)
					throw new InvalidOperationException();
				return xop;
			}
		}

		public (string name, object?[] handlers)[] MVEX {
			get {
				if (!filtered)
					throw new InvalidOperationException();
				return mvex;
			}
		}

		(string name, object?[] handlers)[] legacy;
		(string name, object?[] handlers)[] vex;
		(string name, object?[] handlers)[] evex;
		(string name, object?[] handlers)[] xop;
		(string name, object?[] handlers)[] mvex;
		bool filtered;

		DecoderTables(GenTypes genTypes) {
			legacy = DecoderTable_Legacy.CreateHandlers(genTypes);
			vex = DecoderTable_VEX.CreateHandlers(genTypes);
			evex = DecoderTable_EVEX.CreateHandlers(genTypes);
			xop = DecoderTable_XOP.CreateHandlers(genTypes);
			mvex = DecoderTable_MVEX.CreateHandlers(genTypes);
			genTypes.AddObject(TypeIds.DecoderTables, this);
		}

		void ICreatedInstructions.OnCreatedInstructions(GenTypes genTypes, HashSet<EnumValue> filteredCodeValues) {
			legacy = new DecoderTableFilter(genTypes, filteredCodeValues, legacy, EncodingKind.Legacy).Filter();
			vex = new DecoderTableFilter(genTypes, filteredCodeValues, vex, EncodingKind.VEX).Filter();
			xop = new DecoderTableFilter(genTypes, filteredCodeValues, xop, EncodingKind.XOP).Filter();
			evex = new DecoderTableFilter(genTypes, filteredCodeValues, evex, EncodingKind.EVEX).Filter();
			mvex = new DecoderTableFilter(genTypes, filteredCodeValues, mvex, EncodingKind.MVEX).Filter();
			filtered = true;
		}
	}
}
