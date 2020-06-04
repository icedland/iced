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
using Generator.Constants;

namespace Generator.Enums {
	enum EnumTypeFlags : uint {
		None			= 0,
		Public			= 0x00000001,
		NoInitialize	= 0x00000002,
		Flags			= 0x00000004,
	}

	sealed class EnumType {
		public TypeId TypeId { get; }
		public string RawName { get; }
		public string Name(IdentifierConverter idConverter) => idConverter.Type(RawName);
		public string? Documentation { get; }
		public EnumValue[] Values => values;
		EnumValue[] values;
		readonly bool initialized;
		readonly Dictionary<string, EnumValue> toEnumValue;

		internal void ResetValues(EnumValue[] newValues) {
			foreach (var value in newValues) {
				if (value.DeclaringType != this)
					throw new InvalidOperationException();
			}
			Initialize(newValues);
		}

		public EnumValue this[string name] {
			get {
				if (toEnumValue.TryGetValue(name, out var value))
					return value;
				throw new InvalidOperationException($"Couldn't find enum field {RawName}.{name}");
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

		public EnumType(TypeId typeId, string? documentation, EnumValue[] values, EnumTypeFlags flags)
			: this(typeId.ToString(), typeId, documentation, values, flags) {
		}

		public EnumType(string name, TypeId typeId, string? documentation, EnumValue[] values, EnumTypeFlags flags) {
			toEnumValue = new Dictionary<string, EnumValue>(values.Length, StringComparer.Ordinal);
			IsPublic = (flags & EnumTypeFlags.Public) != 0;
			IsFlags = (flags & EnumTypeFlags.Flags) != 0;
			TypeId = typeId;
			RawName = name;
			Documentation = documentation;
			this.values = values;
			initialized = (flags & EnumTypeFlags.NoInitialize) == 0;
			Initialize(values);
		}

		void Initialize(EnumValue[] values) {
			toEnumValue.Clear();
			this.values = values;
			foreach (var value in values) {
				value.DeclaringType = this;
				toEnumValue.Add(value.RawName, value);
			}
			if (initialized) {
				if (IsFlags) {
					uint value = 0;
					foreach (var enumValue in values) {
						if (enumValue.DeprecatedInfo.IsDeprecated)
							continue;
						if (enumValue.RawName == "None")
							enumValue.Value = 0;
						else {
							if (value == 0)
								value = 1;
							else if (value == 0x80000000)
								throw new InvalidOperationException("Too many enum values");
							else
								value <<= 1;
							enumValue.Value = value;
						}
					}
				}
				else {
					for (int i = 0; i < values.Length; i++) {
						if (values[i].DeprecatedInfo.IsDeprecated)
							continue;
						values[i].Value = (uint)i;
					}
				}
			}
			foreach (var enumValue in values) {
				if (!enumValue.DeprecatedInfo.IsDeprecated)
					continue;
				if (!toEnumValue.TryGetValue(enumValue.DeprecatedInfo.NewName, out var newValue))
					throw new InvalidOperationException($"Couldn't find enum {enumValue.RawName}");
				enumValue.Value = newValue.Value;
				if (enumValue.Documentation is null)
					enumValue.Documentation = newValue.Documentation;
			}
		}

		public ConstantsType ToConstantsType(ConstantKind constantKind) {
			var flags = ConstantsTypeFlags.None;
			if (IsPublic)
				flags |= ConstantsTypeFlags.Public;
			if (IsFlags)
				flags |= ConstantsTypeFlags.Hex;

			var constants = new Constant[Values.Length];
			for (int i = 0; i < constants.Length; i++) {
				var value = Values[i];
				var constant = new Constant(constantKind, value.RawName, value.Value, flags, value.Documentation, value.DeprecatedInfo);
				constants[i] = constant;
			}

			return new ConstantsType(RawName, TypeId, flags, Documentation, constants);
		}
	}

	interface IEnumValue {
		EnumType DeclaringType { get; }
		uint Value { get; }
		string ToStringValue(IdentifierConverter idConverter);
	}

	sealed class OrEnumValue : IEnumValue {
		public EnumValue[] Values => values;
		readonly EnumValue[] values;

		public EnumType DeclaringType { get; }
		public uint Value { get; }
		public string ToStringValue(IdentifierConverter idConverter) {
			var sb = new StringBuilder();
			foreach (var value in values) {
				if (sb.Length > 0)
					sb.Append(", ");
				sb.Append(value.Name(idConverter));
			}
			return sb.ToString();
		}

		public OrEnumValue(EnumType enumType, params string[] values) {
			DeclaringType = enumType;
			var newValues = new EnumValue[values.Length];
			for (int i = 0; i < values.Length; i++) {
				var value = values[i];
				var enumValue = enumType[value];
				newValues[i] = enumValue;
				Value |= enumValue.Value;
			}
			this.values = newValues;
		}

		// Need to override this since the decoder/formatter table generators call Equals()
		public override bool Equals(object? obj) {
			if (!(obj is OrEnumValue other))
				return false;
			if (DeclaringType != other.DeclaringType)
				return false;
			return Value == other.Value;
		}

		public override int GetHashCode() => DeclaringType.GetHashCode() ^ (int)Value;
	}

	sealed class EnumValue : IEnumValue {
		public EnumType DeclaringType { get; set; }
		public uint Value { get; set; }
		public string RawName { get; }
		public string Name(IdentifierConverter idConverter) => idConverter.EnumField(RawName);
		public string ToStringValue(IdentifierConverter idConverter) => idConverter.EnumField(RawName);
		public string? Documentation { get; internal set; }
		public DeprecatedInfo DeprecatedInfo { get; }

		public EnumValue(uint value, string name, string? documentation)
			: this(value, name, documentation, default) {
		}

		public EnumValue(uint value, string name, string? documentation, DeprecatedInfo deprecatedInfo) {
			DeclaringType = null!;
			Value = value;
			RawName = name;
			Documentation = documentation;
			DeprecatedInfo = deprecatedInfo;
		}
	}
}
