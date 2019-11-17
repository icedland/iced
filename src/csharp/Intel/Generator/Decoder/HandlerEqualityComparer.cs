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

namespace Generator.Decoder {
	sealed class HandlerEqualityComparer : IEqualityComparer<object?> {
		readonly Func<string, object?[]> getReference;

		public HandlerEqualityComparer(Func<string, object?[]> getReference) {
			this.getReference = getReference;
		}

		public new bool Equals(object? x, object? y) {
			if (x is string xs)
				x = getReference(xs);
			if (y is string ys)
				y = getReference(ys);
			if (x is string || y is string)
				throw new InvalidOperationException();

			if (x == y)
				return true;
			if (x is null || y is null)
				return false;

			switch (x) {
			case object?[] xArray:
				if (!(y is object[] yArray))
					return false;
				if (xArray.Length != yArray.Length)
					return false;
				for (int i = 0; i < xArray.Length; i++) {
					if (!Equals(xArray[i], yArray[i]))
						return false;
				}
				return true;

			default:
				return object.Equals(x, y);
			}
		}

		public int GetHashCode(object? obj) => GetHashCodeCore(obj, 0);

		const int maxRecurseLevel = 3;
		int GetHashCodeCore(object? obj, int level) {
			if (level >= maxRecurseLevel)
				return 0;

			if (obj is string s)
				obj = getReference(s);
			if (obj is string)
				throw new InvalidOperationException();

			if (obj is null)
				return 0;

			switch (obj) {
			case object?[] array:
				int hc = 0;
				for (int i = 0; i < array.Length; i++)
					hc = HashCode.Combine(hc, GetHashCodeCore(array[i], level + 1));
				return hc;

			default:
				return obj.GetHashCode();
			}
		}
	}
}
