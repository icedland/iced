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
using System.Linq;
using Generator.Enums;

namespace Generator.Tables {
	sealed class TupleTypeInfo {
		public EnumValue Value { get; }
		public uint N { get; }
		public uint Nbcst { get; }
		public TupleTypeInfo(EnumValue value, uint n, uint nbcst) {
			Value = value;
			N = n;
			Nbcst = nbcst;
		}
	}

	[TypeGen(TypeGenOrders.NoDeps)]
	sealed class TupleTypeTable {
		public readonly TupleTypeInfo[] Data;

		TupleTypeTable(GenTypes genTypes) {
			Data = CreateData(genTypes);
			genTypes.AddObject(TypeIds.TupleTypeTable, this);
		}

		static TupleTypeInfo[] CreateData(GenTypes genTypes) {
			var tupleType = genTypes[TypeIds.TupleType];
			var result = new List<TupleTypeInfo> {
				new TupleTypeInfo(tupleType[nameof(TupleType.N1)], 1, 1),
				new TupleTypeInfo(tupleType[nameof(TupleType.N2)], 2, 2),
				new TupleTypeInfo(tupleType[nameof(TupleType.N4)], 4, 4),
				new TupleTypeInfo(tupleType[nameof(TupleType.N8)], 8, 8),
				new TupleTypeInfo(tupleType[nameof(TupleType.N16)], 16, 16),
				new TupleTypeInfo(tupleType[nameof(TupleType.N32)], 32, 32),
				new TupleTypeInfo(tupleType[nameof(TupleType.N64)], 64, 64),
				new TupleTypeInfo(tupleType[nameof(TupleType.N8b4)], 8, 4),
				new TupleTypeInfo(tupleType[nameof(TupleType.N16b4)], 16, 4),
				new TupleTypeInfo(tupleType[nameof(TupleType.N32b4)], 32, 4),
				new TupleTypeInfo(tupleType[nameof(TupleType.N64b4)], 64, 4),
				new TupleTypeInfo(tupleType[nameof(TupleType.N16b8)], 16, 8),
				new TupleTypeInfo(tupleType[nameof(TupleType.N32b8)], 32, 8),
				new TupleTypeInfo(tupleType[nameof(TupleType.N64b8)], 64, 8),
			}.ToArray();
			if (result.Length != tupleType.Values.Length)
				throw new InvalidOperationException();
			if (result.Select(a => a.Value).ToHashSet().Count != tupleType.Values.Length)
				throw new InvalidOperationException();
			Array.Sort(result, (a, b) => a.Value.Value.CompareTo(b.Value.Value));
			return result;
		}
	}
}
