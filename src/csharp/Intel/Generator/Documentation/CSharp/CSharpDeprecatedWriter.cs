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
					WriteDeprecated(writer, newValue.Name(idConverter));
				}
				else
					WriteDeprecated(writer, null);
			}
		}

		public override void WriteDeprecated(FileWriter writer, Constant value) {
			if (value.DeprecatedInfo.IsDeprecated) {
				if (value.DeprecatedInfo.NewName is not null) {
					var newValue = value.DeclaringType[value.DeprecatedInfo.NewName];
					WriteDeprecated(writer, newValue.Name(idConverter));
				}
				else
					WriteDeprecated(writer, null);
			}
		}

		public void WriteDeprecated(FileWriter writer, string? newMember, bool isMember = true, bool isError = true) {
			if (newMember is not null) {
				var errStr = isError ? "true" : "false";
				if (isMember)
					writer.WriteLine($"[System.Obsolete(\"Use \" + nameof({newMember}) + \" instead\", {errStr})]");
				else
					writer.WriteLine($"[System.Obsolete(\"Use {newMember} instead\", {errStr})]");
			}
			else {
				// Our code still sometimes needs to reference the deprecated values so don't pass in 'true'
				writer.WriteLine($"[System.Obsolete(\"DEPRECATED. Don't use it!\", false)]");
			}
			writer.WriteLine("[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]");
		}
	}
}
