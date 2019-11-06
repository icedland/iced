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

namespace Generator.Constants {
	enum ConstantsTypeFlags {
		None			= 0,
		Public			= 0x00000001,
	}

	enum ConstantsTypeKind {
		IcedConstants,
	}

	sealed class ConstantsType {
		public ConstantsTypeKind Kind { get; }
		public string Name { get; }
		public string? Documentation { get; }
		public bool IsPublic { get; }
		public Constant[] Constants { get; }
		readonly Dictionary<string, Constant> toConstant;

		public Constant this[string name] {
			get {
				if (toConstant.TryGetValue(name, out var value))
					return value;
				throw new InvalidOperationException($"Couldn't find constant field {Name}.{value}");
			}
		}

		public bool IsMissingDocs {
			get {
				if (string.IsNullOrEmpty(Documentation))
					return true;
				foreach (var constant in Constants) {
					if (string.IsNullOrEmpty(constant.Documentation))
						return true;
				}
				return false;
			}
		}

		public ConstantsType(ConstantsTypeKind kind, ConstantsTypeFlags flags, string? documentation, Constant[] constants) {
			toConstant = new Dictionary<string, Constant>(StringComparer.Ordinal);
			Kind = kind;
			Name = kind.ToString();
			Documentation = documentation;
			IsPublic = (flags & ConstantsTypeFlags.Public) != 0;
			Constants = constants;

			foreach (var constant in constants) {
				constant.DeclaringType = this;
				toConstant.Add(constant.Name, constant);
			}
		}
	}

	enum ConstantKind {
		Int32,
		Register,
		MemorySize,
	}

	sealed class Constant {
		public ConstantKind Kind { get; }
		public string Name { get; }
		public string? Documentation { get; }
		public uint Value { get; }
		public bool IsPublic { get; }
		public ConstantsType DeclaringType { get; set; }

		public Constant(ConstantKind kind, string name, uint value, ConstantsTypeFlags flags, string? documentation) {
			DeclaringType = null!;
			Kind = kind;
			Name = name;
			Documentation = documentation;
			Value = value;
			IsPublic = (flags & ConstantsTypeFlags.Public) != 0;
		}
	}
}
