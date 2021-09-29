// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.Collections.Generic;

namespace Generator.Constants {
	enum ConstantsTypeFlags {
		None			= 0,
		Public			= 0x00000001,
		Hex				= 0x00000002,
	}

	sealed class ConstantsType {
		public TypeId TypeId { get; }
		public string RawName { get; }
		public string Name(IdentifierConverter idConverter) => idConverter.Type(RawName);
		public LanguageDocumentation Documentation { get; }
		public bool IsPublic { get; }
		public Constant[] Constants { get; }
		readonly Dictionary<string, Constant> toConstant;

		public Constant this[string name] {
			get {
				if (toConstant.TryGetValue(name, out var value))
					return value;
				throw new InvalidOperationException($"Couldn't find constant field {RawName}.{value}");
			}
		}

		public bool IsMissingDocs {
			get {
				if (!Documentation.HasDefaultComment)
					return true;
				foreach (var constant in Constants) {
					if (!constant.Documentation.HasDefaultComment)
						return true;
				}
				return false;
			}
		}

		public ConstantsType(TypeId typeId, ConstantsTypeFlags flags, LanguageDocumentation documentation, Constant[] constants)
			: this(typeId.ToString(), typeId, flags, documentation, constants) {
		}

		public ConstantsType(string name, TypeId typeId, ConstantsTypeFlags flags, LanguageDocumentation documentation, Constant[] constants) {
			toConstant = new Dictionary<string, Constant>(StringComparer.Ordinal);
			TypeId = typeId;
			RawName = name;
			Documentation = documentation;
			IsPublic = (flags & ConstantsTypeFlags.Public) != 0;
			Constants = constants;

			foreach (var constant in constants) {
				constant.DeclaringType = this;
				toConstant.Add(constant.RawName, constant);
			}
		}
	}

	enum ConstantKind {
		Char,
		String,
		Int32,
		UInt32,
		UInt64,
		Register,
		MemorySize,
		Index,
	}

	sealed class Constant {
		public ConstantKind Kind { get; }
		public string RawName { get; }
		public string Name(IdentifierConverter idConverter) => idConverter.Constant(RawName);
		public LanguageDocumentation Documentation { get; }
		public DeprecatedInfo DeprecatedInfo { get; }
		public ulong ValueUInt64 { get; }
		public object? RefValue { get; }
		public bool IsPublic { get; }
		public bool UseHex { get; }
		public ConstantsType DeclaringType { get; set; }

		public Constant(ConstantKind kind, string name, object value, ConstantsTypeFlags flags = ConstantsTypeFlags.None)
			: this(kind, name, value, flags, default, default) {
		}

		public Constant(ConstantKind kind, string name, object value, ConstantsTypeFlags flags, LanguageDocumentation documentation, DeprecatedInfo deprecatedInfo) {
			if (value is not null && value.GetType().IsValueType)
				throw new ArgumentException();
			DeclaringType = null!;
			Kind = kind;
			RawName = name;
			Documentation = documentation;
			DeprecatedInfo = deprecatedInfo;
			ValueUInt64 = 0;
			RefValue = value;
			IsPublic = (flags & ConstantsTypeFlags.Public) != 0;
			UseHex = (flags & ConstantsTypeFlags.Hex) != 0;
		}

		public Constant(ConstantKind kind, string name, ulong value, ConstantsTypeFlags flags = ConstantsTypeFlags.None)
			: this(kind, name, value, flags, default, default) {
		}

		public Constant(ConstantKind kind, string name, ulong value, ConstantsTypeFlags flags, LanguageDocumentation documentation, DeprecatedInfo deprecatedInfo) {
			DeclaringType = null!;
			Kind = kind;
			RawName = name;
			Documentation = documentation;
			DeprecatedInfo = deprecatedInfo;
			ValueUInt64 = value;
			RefValue = null;
			IsPublic = (flags & ConstantsTypeFlags.Public) != 0;
			UseHex = (flags & ConstantsTypeFlags.Hex) != 0;
		}
	}
}
