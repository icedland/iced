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
