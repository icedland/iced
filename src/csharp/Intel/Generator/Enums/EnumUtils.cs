// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

namespace Generator.Enums{
	static class EnumUtils {
		public static bool UppercaseTypeFields(string name) =>
			name switch {
				"Code" or "CpuidFeature" or "EncodingKind" or "Mnemonic" or "Register" or "RflagsBits" or
				"OpCodeOperandKind" or "OpCodeTableKind" or "TupleType" => true,
				_ => false,
			};

		public static (string name, string value) GetEnumNameValue(IdentifierConverter idConverter, EnumValue value, bool uppercaseRawName) {
			var numStr = value.DeclaringType.IsFlags ? NumberFormatter.FormatHexUInt32WithSep(value.Value) : value.Value.ToString();
			string valueName;
			if (uppercaseRawName)
				valueName = value.RawName.ToUpperInvariant();
			else
				valueName = value.Name(idConverter);
			return (valueName, numStr);
		}
	}
}
