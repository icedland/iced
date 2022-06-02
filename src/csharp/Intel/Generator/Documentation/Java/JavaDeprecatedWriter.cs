// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using Generator.Constants;
using Generator.Enums;

namespace Generator.Documentation.Java {
	sealed class JavaDeprecatedWriter {
		readonly IdentifierConverter idConverter;

		public JavaDeprecatedWriter(IdentifierConverter idConverter) =>
			this.idConverter = idConverter;

		public string? GetDeprecatedString(EnumValue value) {
			if (value.DeprecatedInfo.IsDeprecated) {
				if (value.DeprecatedInfo.NewName is not null) {
					var newValue = value.DeclaringType[value.DeprecatedInfo.NewName];
					return GetDeprecatedString(newValue.Name(idConverter), value.DeprecatedInfo.Description, true);
				}
				else
					return GetDeprecatedString(null, value.DeprecatedInfo.Description, true);
			}
			else
				return null;
		}

		public string? GetDeprecatedString(Constant value) {
			if (value.DeprecatedInfo.IsDeprecated) {
				if (value.DeprecatedInfo.NewName is not null) {
					var newValue = value.DeclaringType[value.DeprecatedInfo.NewName];
					return GetDeprecatedString(newValue.Name(idConverter), value.DeprecatedInfo.Description, true);
				}
				else
					return GetDeprecatedString(null, value.DeprecatedInfo.Description, true);
			}
			else
				return null;
		}

		string GetDeprecatedString(string? newMember, string? description, bool isMember) {
			string deprecStr;
			if (description is not null)
				deprecStr = description;
			else if (newMember is not null) {
				if (isMember)
					deprecStr = $"Use \" + nameof({newMember}) + \" instead";
				else
					deprecStr = $"Use {newMember} instead";
			}
			else
				deprecStr = "DEPRECATED. Don't use it!";
			return deprecStr;
		}
	}
}
