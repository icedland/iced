// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using Generator.Enums;

namespace Generator.Tables {
	readonly struct MvexLutData {
		public readonly TupleType TupleType;
		public readonly MvexConvDecorator Decorator;
		public readonly MemorySize MemorySize;

		MvexLutData(TupleType tupleType, MvexConvDecorator decorator, MemorySize memorySize) {
			TupleType = tupleType;
			Decorator = decorator;
			MemorySize = memorySize;
		}

		public static (MvexTupleTypeLutKind ttLutKind, MvexLutData[] data)[] GetLutData() {
			var lutKindValues = Enum.GetValues<MvexTupleTypeLutKind>();
			var result = new (MvexTupleTypeLutKind ttLutKind, MvexLutData[] data)[lutKindValues.Length];
			foreach (var kind in lutKindValues) {
				var data = kind switch {
					MvexTupleTypeLutKind.Int32 => new MvexLutData[8] {
						new(TupleType.N64, MvexConvDecorator.None, MemorySize.Packed512_Int32),
						new(TupleType.N4, MvexConvDecorator.Broadcast_1to16, MemorySize.Int32),
						new(TupleType.N16, MvexConvDecorator.Broadcast_4to16, MemorySize.Packed128_Int32),
						new(TupleType.N32, MvexConvDecorator.Float16, MemorySize.Packed256_Float16),
						new(TupleType.N16, MvexConvDecorator.Uint8, MemorySize.Packed128_UInt8),
						new(TupleType.N16, MvexConvDecorator.Sint8, MemorySize.Packed128_Int8),
						new(TupleType.N32, MvexConvDecorator.Uint16, MemorySize.Packed256_UInt16),
						new(TupleType.N32, MvexConvDecorator.Sint16, MemorySize.Packed256_Int16),
					},
					MvexTupleTypeLutKind.Int32_Half => new MvexLutData[8] {
						new(TupleType.N32, MvexConvDecorator.None, MemorySize.Packed256_Int32),
						new(TupleType.N4, MvexConvDecorator.Broadcast_1to8, MemorySize.Int32),
						new(TupleType.N16, MvexConvDecorator.Broadcast_4to8, MemorySize.Packed128_Int32),
						new(TupleType.N16, MvexConvDecorator.Float16, MemorySize.Packed128_Float16), // invalid
						new(TupleType.N8, MvexConvDecorator.Uint8, MemorySize.Packed64_UInt8), // invalid
						new(TupleType.N8, MvexConvDecorator.Sint8, MemorySize.Packed64_Int8), // invalid
						new(TupleType.N16, MvexConvDecorator.Uint16, MemorySize.Packed128_UInt16), // invalid
						new(TupleType.N16, MvexConvDecorator.Sint16, MemorySize.Packed128_Int16), // invalid
					},
					MvexTupleTypeLutKind.Int32_4to16 => new MvexLutData[8] {
						new(TupleType.N16, MvexConvDecorator.None, MemorySize.Packed128_Int32),
						new(TupleType.N1, MvexConvDecorator.None, MemorySize.Unknown), // invalid
						new(TupleType.N1, MvexConvDecorator.None, MemorySize.Unknown), // invalid
						new(TupleType.N8, MvexConvDecorator.Float16, MemorySize.Packed64_Float16),
						new(TupleType.N4, MvexConvDecorator.Uint8, MemorySize.Packed32_UInt8),
						new(TupleType.N4, MvexConvDecorator.Sint8, MemorySize.Packed32_Int8),
						new(TupleType.N8, MvexConvDecorator.Uint16, MemorySize.Packed64_UInt16),
						new(TupleType.N8, MvexConvDecorator.Sint16, MemorySize.Packed64_Int16),
					},
					MvexTupleTypeLutKind.Int32_1to16_or_elem => new MvexLutData[8] {
						new(TupleType.N4, MvexConvDecorator.None, MemorySize.Int32),
						new(TupleType.N1, MvexConvDecorator.None, MemorySize.Unknown), // invalid
						new(TupleType.N1, MvexConvDecorator.None, MemorySize.Unknown), // invalid
						new(TupleType.N2, MvexConvDecorator.Float16, MemorySize.Float16),
						new(TupleType.N1, MvexConvDecorator.Uint8, MemorySize.UInt8),
						new(TupleType.N1, MvexConvDecorator.Sint8, MemorySize.Int8),
						new(TupleType.N2, MvexConvDecorator.Uint16, MemorySize.UInt16),
						new(TupleType.N2, MvexConvDecorator.Sint16, MemorySize.Int16),
					},
					MvexTupleTypeLutKind.Int64 => new MvexLutData[8] {
						new(TupleType.N64, MvexConvDecorator.None, MemorySize.Packed512_Int64),
						new(TupleType.N8, MvexConvDecorator.Broadcast_1to8, MemorySize.Int64),
						new(TupleType.N32, MvexConvDecorator.Broadcast_4to8, MemorySize.Packed256_Int64),
						new(TupleType.N16, MvexConvDecorator.Float16, MemorySize.Packed128_Float16), // invalid
						new(TupleType.N8, MvexConvDecorator.Uint8, MemorySize.Packed64_UInt8), // invalid
						new(TupleType.N8, MvexConvDecorator.Sint8, MemorySize.Packed64_Int8), // invalid
						new(TupleType.N16, MvexConvDecorator.Uint16, MemorySize.Packed128_UInt16), // invalid
						new(TupleType.N16, MvexConvDecorator.Sint16, MemorySize.Packed128_Int16), // invalid
					},
					MvexTupleTypeLutKind.Int64_4to8 => new MvexLutData[8] {
						new(TupleType.N32, MvexConvDecorator.None, MemorySize.Packed256_Int64),
						new(TupleType.N1, MvexConvDecorator.None, MemorySize.Unknown), // invalid
						new(TupleType.N1, MvexConvDecorator.None, MemorySize.Unknown), // invalid
						new(TupleType.N8, MvexConvDecorator.Float16, MemorySize.Packed64_Float16), // invalid
						new(TupleType.N4, MvexConvDecorator.Uint8, MemorySize.Packed32_UInt8), // invalid
						new(TupleType.N4, MvexConvDecorator.Sint8, MemorySize.Packed32_Int8), // invalid
						new(TupleType.N8, MvexConvDecorator.Uint16, MemorySize.Packed64_UInt16), // invalid
						new(TupleType.N8, MvexConvDecorator.Sint16, MemorySize.Packed64_Int16), // invalid
					},
					MvexTupleTypeLutKind.Int64_1to8_or_elem => new MvexLutData[8] {
						new(TupleType.N8, MvexConvDecorator.None, MemorySize.Int64),
						new(TupleType.N1, MvexConvDecorator.None, MemorySize.Unknown), // invalid
						new(TupleType.N1, MvexConvDecorator.None, MemorySize.Unknown), // invalid
						new(TupleType.N2, MvexConvDecorator.Float16, MemorySize.Float16), // invalid
						new(TupleType.N1, MvexConvDecorator.Uint8, MemorySize.UInt8), // invalid
						new(TupleType.N1, MvexConvDecorator.Sint8, MemorySize.Int8), // invalid
						new(TupleType.N2, MvexConvDecorator.Uint16, MemorySize.UInt16), // invalid
						new(TupleType.N2, MvexConvDecorator.Sint16, MemorySize.Int16), // invalid
					},
					MvexTupleTypeLutKind.Float32 => new MvexLutData[8] {
						new(TupleType.N64, MvexConvDecorator.None, MemorySize.Packed512_Float32),
						new(TupleType.N4, MvexConvDecorator.Broadcast_1to16, MemorySize.Float32),
						new(TupleType.N16, MvexConvDecorator.Broadcast_4to16, MemorySize.Packed128_Float32),
						new(TupleType.N32, MvexConvDecorator.Float16, MemorySize.Packed256_Float16),
						new(TupleType.N16, MvexConvDecorator.Uint8, MemorySize.Packed128_UInt8),
						new(TupleType.N16, MvexConvDecorator.Sint8, MemorySize.Packed128_Int8),
						new(TupleType.N32, MvexConvDecorator.Uint16, MemorySize.Packed256_UInt16),
						new(TupleType.N32, MvexConvDecorator.Sint16, MemorySize.Packed256_Int16),
					},
					MvexTupleTypeLutKind.Float32_Half => new MvexLutData[8] {
						new(TupleType.N32, MvexConvDecorator.None, MemorySize.Packed256_Float32),
						new(TupleType.N4, MvexConvDecorator.Broadcast_1to8, MemorySize.Float32),
						new(TupleType.N16, MvexConvDecorator.Broadcast_4to8, MemorySize.Packed128_Float32),
						new(TupleType.N16, MvexConvDecorator.Float16, MemorySize.Packed128_Float16), // invalid
						new(TupleType.N8, MvexConvDecorator.Uint8, MemorySize.Packed64_UInt8), // invalid
						new(TupleType.N8, MvexConvDecorator.Sint8, MemorySize.Packed64_Int8), // invalid
						new(TupleType.N16, MvexConvDecorator.Uint16, MemorySize.Packed128_UInt16), // invalid
						new(TupleType.N16, MvexConvDecorator.Sint16, MemorySize.Packed128_Int16), // invalid
					},
					MvexTupleTypeLutKind.Float32_4to16 => new MvexLutData[8] {
						new(TupleType.N16, MvexConvDecorator.None, MemorySize.Packed128_Float32),
						new(TupleType.N1, MvexConvDecorator.None, MemorySize.Unknown), // invalid
						new(TupleType.N1, MvexConvDecorator.None, MemorySize.Unknown), // invalid
						new(TupleType.N8, MvexConvDecorator.Float16, MemorySize.Packed64_Float16),
						new(TupleType.N4, MvexConvDecorator.Uint8, MemorySize.Packed32_UInt8),
						new(TupleType.N4, MvexConvDecorator.Sint8, MemorySize.Packed32_Int8),
						new(TupleType.N8, MvexConvDecorator.Uint16, MemorySize.Packed64_UInt16),
						new(TupleType.N8, MvexConvDecorator.Sint16, MemorySize.Packed64_Int16),
					},
					MvexTupleTypeLutKind.Float32_1to16_or_elem => new MvexLutData[8] {
						new(TupleType.N4, MvexConvDecorator.None, MemorySize.Float32),
						new(TupleType.N1, MvexConvDecorator.None, MemorySize.Unknown), // invalid
						new(TupleType.N1, MvexConvDecorator.None, MemorySize.Unknown), // invalid
						new(TupleType.N2, MvexConvDecorator.Float16, MemorySize.Float16),
						new(TupleType.N1, MvexConvDecorator.Uint8, MemorySize.UInt8),
						new(TupleType.N1, MvexConvDecorator.Sint8, MemorySize.Int8),
						new(TupleType.N2, MvexConvDecorator.Uint16, MemorySize.UInt16),
						new(TupleType.N2, MvexConvDecorator.Sint16, MemorySize.Int16),
					},
					MvexTupleTypeLutKind.Float64 => new MvexLutData[8] {
						new(TupleType.N64, MvexConvDecorator.None, MemorySize.Packed512_Float64),
						new(TupleType.N8, MvexConvDecorator.Broadcast_1to8, MemorySize.Float64),
						new(TupleType.N32, MvexConvDecorator.Broadcast_4to8, MemorySize.Packed256_Float64),
						new(TupleType.N16, MvexConvDecorator.Float16, MemorySize.Packed128_Float16), // invalid
						new(TupleType.N8, MvexConvDecorator.Uint8, MemorySize.Packed64_UInt8), // invalid
						new(TupleType.N8, MvexConvDecorator.Sint8, MemorySize.Packed64_Int8), // invalid
						new(TupleType.N16, MvexConvDecorator.Uint16, MemorySize.Packed128_UInt16), // invalid
						new(TupleType.N16, MvexConvDecorator.Sint16, MemorySize.Packed128_Int16), // invalid
					},
					MvexTupleTypeLutKind.Float64_4to8 => new MvexLutData[8] {
						new(TupleType.N32, MvexConvDecorator.None, MemorySize.Packed256_Float64),
						new(TupleType.N1, MvexConvDecorator.None, MemorySize.Unknown), // invalid
						new(TupleType.N1, MvexConvDecorator.None, MemorySize.Unknown), // invalid
						new(TupleType.N8, MvexConvDecorator.Float16, MemorySize.Packed64_Float16), // invalid
						new(TupleType.N4, MvexConvDecorator.Uint8, MemorySize.Packed32_UInt8), // invalid
						new(TupleType.N4, MvexConvDecorator.Sint8, MemorySize.Packed32_Int8), // invalid
						new(TupleType.N8, MvexConvDecorator.Uint16, MemorySize.Packed64_UInt16), // invalid
						new(TupleType.N8, MvexConvDecorator.Sint16, MemorySize.Packed64_Int16), // invalid
					},
					MvexTupleTypeLutKind.Float64_1to8_or_elem => new MvexLutData[8] {
						new(TupleType.N8, MvexConvDecorator.None, MemorySize.Float64),
						new(TupleType.N1, MvexConvDecorator.None, MemorySize.Unknown), // invalid
						new(TupleType.N1, MvexConvDecorator.None, MemorySize.Unknown), // invalid
						new(TupleType.N2, MvexConvDecorator.Float16, MemorySize.Float16), // invalid
						new(TupleType.N1, MvexConvDecorator.Uint8, MemorySize.UInt8), // invalid
						new(TupleType.N1, MvexConvDecorator.Sint8, MemorySize.Int8), // invalid
						new(TupleType.N2, MvexConvDecorator.Uint16, MemorySize.UInt16), // invalid
						new(TupleType.N2, MvexConvDecorator.Sint16, MemorySize.Int16), // invalid
					},
					_ => throw new InvalidOperationException(),
				};
				if (data.Length != 8)
					throw new InvalidOperationException();
				result[(int)kind] = (kind, data);
			}
			foreach (var (_, data) in result) {
				if (data is null)
					throw new InvalidOperationException();
			}
			return result;
		}
	}
}
