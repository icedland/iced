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

		(string name, object?[] handlers)[] legacy;
		(string name, object?[] handlers)[] vex;
		(string name, object?[] handlers)[] evex;
		(string name, object?[] handlers)[] xop;
		bool filtered;

		DecoderTables(GenTypes genTypes) {
			legacy = DecoderTable_Legacy.CreateHandlers(genTypes);
			vex = DecoderTable_VEX.CreateHandlers(genTypes);
			evex = DecoderTable_EVEX.CreateHandlers(genTypes);
			xop = DecoderTable_XOP.CreateHandlers(genTypes);
			genTypes.AddObject(TypeIds.DecoderTables, this);
		}

		void ICreatedInstructions.OnCreatedInstructions(GenTypes genTypes, HashSet<EnumValue> filteredCodeValues) {
			legacy = new DecoderTableFilter(genTypes, filteredCodeValues, legacy, EncodingKind.Legacy).Filter();
			vex = new DecoderTableFilter(genTypes, filteredCodeValues, vex, EncodingKind.VEX).Filter();
			xop = new DecoderTableFilter(genTypes, filteredCodeValues, xop, EncodingKind.XOP).Filter();
			evex = new DecoderTableFilter(genTypes, filteredCodeValues, evex, EncodingKind.EVEX).Filter();
			filtered = true;
		}
	}
}
