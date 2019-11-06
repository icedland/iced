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
using System.Text;

namespace Generator.Enums {
	enum EnumTypeFlags : uint {
		None			= 0,
		Public			= 0x00000001,
		NoInitialize	= 0x00000002,
		Flags			= 0x00000004,
	}

	enum EnumKind {
		Code,
		CodeSize,
		CpuidFeature,
		CpuidFeatureInternal,
		DecoderOptions,
		EvexOpCodeHandlerKind,
		HandlerFlags,
		LegacyHandlerFlags,
		MemorySize,
		OpCodeHandlerKind,
		PseudoOpsKind,
		Register,
		SerializedDataKind,
		TupleType,
		VexOpCodeHandlerKind,
	}

	sealed class EnumType {
		public EnumKind EnumKind { get; }
		public string Name { get; }
		public string? Documentation { get; }
		public EnumValue[] Values { get; }
		readonly Dictionary<string, EnumValue> toEnumValue;

		public EnumValue this[string name] {
			get {
				if (toEnumValue.TryGetValue(name, out var value))
					return value;
				throw new InvalidOperationException($"Couldn't find enum value {Name}.{value}");
			}
		}

		public bool IsPublic { get; }
		public bool IsFlags { get; }

		public bool IsMissingDocs {
			get {
				if (string.IsNullOrEmpty(Documentation))
					return true;
				foreach (var value in Values) {
					if (string.IsNullOrEmpty(value.Documentation))
						return true;
				}
				return false;
			}
		}

		public EnumType(EnumKind enumKind, string? documentation, EnumValue[] values, EnumTypeFlags flags) {
			toEnumValue = new Dictionary<string, EnumValue>(values.Length, StringComparer.Ordinal);
			IsPublic = (flags & EnumTypeFlags.Public) != 0;
			IsFlags = (flags & EnumTypeFlags.Flags) != 0;
			EnumKind = enumKind;
			Name = enumKind.ToString();
			Documentation = documentation;
			Values = values;
			if ((flags & EnumTypeFlags.NoInitialize) == 0) {
				if ((flags & EnumTypeFlags.Flags) != 0) {
					uint value = 0;
					for (int i = 0; i < values.Length; i++) {
						if (values[i].Name == "None")
							values[i].Value = 0;
						else {
							if (value == 0)
								value = 1;
							else if (value == 0x80000000)
								throw new InvalidOperationException("Too many enum value");
							else
								value <<= 1;
							values[i].Value = value;
						}
					}
				}
				else {
					for (int i = 0; i < values.Length; i++)
						values[i].Value = (uint)i;
				}
			}
			foreach (var value in values) {
				value.EnumType = this;
				toEnumValue.Add(value.Name, value);
			}
		}
	}

	interface IEnumValue {
		EnumType EnumType { get; }
		uint Value { get; }
		string ToStringValue { get; }
	}

	sealed class OrEnumValue : IEnumValue {
		public EnumType EnumType { get; }
		public uint Value { get; }
		public string ToStringValue { get; }

		public OrEnumValue(EnumType enumType, params string[] values) {
			EnumType = enumType;
			var sb = new StringBuilder();
			foreach (var value in values) {
				if (sb.Length > 0)
					sb.Append(", ");
				sb.Append(enumType[value].Name);
				Value |= enumType[value].Value;
			}
			ToStringValue = sb.ToString();
		}
	}

	sealed class EnumValue : IEnumValue {
		public EnumType EnumType { get; set; }
		public uint Value { get; set; }
		public string Name { get; }
		public string ToStringValue => Name;
		public string? Documentation { get; internal set; }

		public EnumValue(string name) {
			EnumType = null!;
			Value = 0;
			Name = name;
			Documentation = null;
		}

		public EnumValue(string name, string? documentation) {
			EnumType = null!;
			Value = 0;
			Name = name;
			Documentation = documentation;
		}
	}
}
