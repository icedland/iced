// SPDX-License-Identifier: MIT
// Copyright wtfsckgh@gmail.com
// Copyright iced contributors

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
					WriteDeprecated(writer, newValue.Name(idConverter), value.DeprecatedInfo.Description);
				}
				else
					WriteDeprecated(writer, null, value.DeprecatedInfo.Description);
			}
		}

		public override void WriteDeprecated(FileWriter writer, Constant value) {
			if (value.DeprecatedInfo.IsDeprecated) {
				if (value.DeprecatedInfo.NewName is not null) {
					var newValue = value.DeclaringType[value.DeprecatedInfo.NewName];
					WriteDeprecated(writer, newValue.Name(idConverter), value.DeprecatedInfo.Description);
				}
				else
					WriteDeprecated(writer, null, value.DeprecatedInfo.Description);
			}
		}

		public void WriteDeprecated(FileWriter writer, string? newMember, string? description, bool isMember = true, bool isError = true) {
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
