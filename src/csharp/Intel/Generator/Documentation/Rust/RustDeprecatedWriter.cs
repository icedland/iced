// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using Generator.Constants;
using Generator.Enums;
using Generator.IO;

namespace Generator.Documentation.Rust {
	sealed class RustDeprecatedWriter : DeprecatedWriter {
		readonly IdentifierConverter idConverter;

		public RustDeprecatedWriter(IdentifierConverter idConverter) =>
			this.idConverter = idConverter;

		public override void WriteDeprecated(FileWriter writer, EnumValue value) =>
			WriteDeprecated(writer, value, null);

		public void WriteDeprecated(FileWriter writer, EnumValue value, string? notDeprecFeature) {
			if (value.DeprecatedInfo.IsDeprecated) {
				if (value.DeprecatedInfo.NewName is not null) {
					var newValue = value.DeclaringType[value.DeprecatedInfo.NewName];
					WriteDeprecated(writer, value.DeprecatedInfo.VersionStr, newValue.Name(idConverter), value.DeprecatedInfo.Description, notDeprecFeature);
				}
				else
					WriteDeprecated(writer, value.DeprecatedInfo.VersionStr, null, value.DeprecatedInfo.Description, notDeprecFeature);
			}
		}

		public override void WriteDeprecated(FileWriter writer, Constant value) =>
			WriteDeprecated(writer, value, null);

		public void WriteDeprecated(FileWriter writer, Constant value, string? notDeprecFeature) {
			if (value.DeprecatedInfo.IsDeprecated) {
				if (value.DeprecatedInfo.NewName is not null) {
					var newValue = value.DeclaringType[value.DeprecatedInfo.NewName];
					WriteDeprecated(writer, value.DeprecatedInfo.VersionStr, newValue.Name(idConverter), value.DeprecatedInfo.Description, notDeprecFeature);
				}
				else
					WriteDeprecated(writer, value.DeprecatedInfo.VersionStr, null, value.DeprecatedInfo.Description, notDeprecFeature);
			}
		}

		static void WriteDeprecated(FileWriter writer, string version, string? newName, string? description, string? notDeprecFeature) {
			string extra;
			if (description is not null)
				extra = description;
			else if (newName is not null)
				extra = $"Use {newName} instead";
			else
				extra = "Don't use it!";
			var deprecatedAttr = $"deprecated(since = \"{version}\", note = \"{extra}\")";
			if (notDeprecFeature is not null)
				writer.WriteLine($"#[cfg_attr(not(feature = \"{notDeprecFeature}\"), {deprecatedAttr})]");
			else
				writer.WriteLine($"#[{deprecatedAttr}]");
		}
	}
}
