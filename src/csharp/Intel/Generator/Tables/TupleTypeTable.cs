// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

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
				new TupleTypeInfo(tupleType[nameof(TupleType.N4b2)], 4, 2),
				new TupleTypeInfo(tupleType[nameof(TupleType.N8b2)], 8, 2),
				new TupleTypeInfo(tupleType[nameof(TupleType.N16b2)], 16, 2),
				new TupleTypeInfo(tupleType[nameof(TupleType.N32b2)], 32, 2),
				new TupleTypeInfo(tupleType[nameof(TupleType.N64b2)], 64, 2),
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
