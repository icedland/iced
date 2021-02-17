// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;

namespace Generator {
	readonly struct TypeId : IEquatable<TypeId> {
		public string Id1 => id1;

		readonly string id1;
		readonly string id2;

		public TypeId(string id1) {
			this.id1 = id1;
			id2 = string.Empty;
		}

		public TypeId(string id1, string id2) {
			this.id1 = id1;
			this.id2 = id2;
		}

		public static bool operator ==(TypeId left, TypeId right) => left.Equals(right);
		public static bool operator !=(TypeId left, TypeId right) => !left.Equals(right);

		public bool Equals(TypeId other) =>
			id1 == other.id1 &&
			id2 == other.id2;

		public override bool Equals(object? obj) =>
			obj is TypeId other && Equals(other);

		public override int GetHashCode() =>
			HashCode.Combine(StringComparer.Ordinal.GetHashCode(id1 ?? string.Empty),
				StringComparer.Ordinal.GetHashCode(id2 ?? string.Empty));

		public override string ToString() => string.IsNullOrEmpty(id2) ? id1 : $"{id1} {id2}";
	}
}
