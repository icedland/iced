// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using Generator.Constants;
using Generator.Enums;
using Generator.IO;

namespace Generator.Documentation.CSharp {
	sealed class CSharpDeprecatedWriter : DeprecatedWriter {
		readonly IdentifierConverter idConverter;

		public CSharpDeprecatedWriter(IdentifierConverter idConverter) =>
			this.idConverter = idConverter;

		public override void WriteDeprecated(FileWriter writer, EnumValue value) {
			if (value.DeprecatedInfo.IsDeprecated) {
				if (value.DeprecatedInfo.NewName is not null) {
					var newValue = value.DeclaringType[value.DeprecatedInfo.NewName];
					WriteDeprecated(writer, newValue.Name(idConverter), value.DeprecatedInfo.Description, value.DeprecatedInfo.IsError, true);
				}
				else
					WriteDeprecated(writer, null, value.DeprecatedInfo.Description, value.DeprecatedInfo.IsError, true);
			}
		}

		public override void WriteDeprecated(FileWriter writer, Constant value) {
			if (value.DeprecatedInfo.IsDeprecated) {
				if (value.DeprecatedInfo.NewName is not null) {
					var newValue = value.DeclaringType[value.DeprecatedInfo.NewName];
					WriteDeprecated(writer, newValue.Name(idConverter), value.DeprecatedInfo.Description, value.DeprecatedInfo.IsError, true);
				}
				else
					WriteDeprecated(writer, null, value.DeprecatedInfo.Description, value.DeprecatedInfo.IsError, true);
			}
		}

		public void WriteDeprecated(FileWriter writer, string? newMember, string? description, bool isError, bool isMember) {
			var errStr = isError ? "true" : "false";
			string deprecStr;
			if (description is not null)
				deprecStr = description;
			else if (newMember is not null) {
				if (isMember)
					deprecStr = $"Use \" + nameof({newMember}) + \" instead";
				else
					deprecStr = $"Use {newMember} instead";
			}
			else {
				// Our code still sometimes needs to reference the deprecated values so don't pass in 'true'
				errStr = "false";
				deprecStr = "DEPRECATED. Don't use it!";
			}
			writer.WriteLine($"[System.Obsolete(\"{deprecStr}\", {errStr})]");
			writer.WriteLine("[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]");
		}
	}
}
